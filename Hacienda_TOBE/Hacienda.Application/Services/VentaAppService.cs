using Hacienda.Application.Abstractions;
using Hacienda.Domain.Entities;
using Hacienda.Domain.Factories;
using Hacienda.Domain.Ports;

namespace Hacienda.Application.Services;

public class VentaAppService : IVentaAppService
{
    private readonly IPotreroRepository _potreros;
    private readonly IVentaRepository _ventas;
    private readonly IProductoFactoryProvider _productoFactoryProvider;

    public VentaAppService(
        IPotreroRepository potreros,
        IVentaRepository ventas,
        IProductoFactoryProvider productoFactoryProvider)
    {
        _potreros = potreros;
        _ventas = ventas;
        _productoFactoryProvider = productoFactoryProvider;
    }

    public async Task<string> VenderResAsync(string potreroId, string nombreRes, decimal monto, CancellationToken ct = default)
    {
        var potrero = await _potreros.GetByIdAsync(potreroId, ct)
            ?? throw new InvalidOperationException($"Potrero '{potreroId}' no encontrado.");
        var res = potrero.BuscarRes(nombreRes)
            ?? throw new InvalidOperationException($"Res '{nombreRes}' no encontrada.");

        string tipo = res switch
        {
            Ternero => "Ternero",
            Cebon => "Cebon",
            Novillo => "Novillo",
            _ => "Res"
        };

        var venta = new Venta(potreroId, DateTime.Today, res.Nombre, res.Peso, res.Edad, tipo, monto);
        if (!potrero.RemoverRes(nombreRes))
            throw new InvalidOperationException("No se pudo remover la res del potrero.");

        await _potreros.SaveAsync(potrero, ct);
        await _ventas.AddAsync(venta, ct);
        return $"Res '{nombreRes}' vendida por {monto:C}.";
    }

    public async Task<string> VenderProductoAsync(
        string potreroId,
        string tipoProducto,
        string nombreProducto,
        decimal cantidad,
        string unidad,
        decimal precioUnitario,
        string? atributoEspecifico = null,
        CancellationToken ct = default)
    {
        // Validamos que el potrero exista (contexto de la hacienda)
        var potrero = await _potreros.GetByIdAsync(potreroId, ct)
            ?? throw new InvalidOperationException($"Potrero '{potreroId}' no encontrado.");

        // 1. Obtener la fábrica concreta según el tipo (Factory Method + Provider)
        var factory = _productoFactoryProvider.ObtenerFactory(tipoProducto);

        // 2. Crear el producto
        var producto = factory.Crear(nombreProducto, cantidad, unidad, precioUnitario, atributoEspecifico);

        // 3. Calcular monto
        var monto = producto.CalcularMonto();

        // 4. Registrar la venta de producto
        var venta = new Venta(potreroId, DateTime.Today, producto, monto);
        await _ventas.AddAsync(venta, ct);

        return $"Producto '{producto.Nombre}' ({producto.Tipo}) vendido: {producto.Cantidad} {producto.Unidad} por {monto:C}.";
    }

    public Task<IReadOnlyList<Venta>> ListarAsync(CancellationToken ct = default) =>
        _ventas.GetAllAsync(ct);
} 