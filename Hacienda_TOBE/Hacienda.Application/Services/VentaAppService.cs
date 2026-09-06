using Hacienda.Application.Abstractions;
using Hacienda.Domain.Entities;
using Hacienda.Domain.Factories;
using Hacienda.Domain.Ports;
using Hacienda.Domain.Strategies;

namespace Hacienda.Application.Services;

public class VentaAppService : IVentaAppService
{
    private readonly IPotreroRepository _potreros;
    private readonly IVentaRepository _ventas;
    private readonly IProductoFactoryProvider _productoFactoryProvider;
    private readonly ICalculadoraProvider _calculadoras;

    public VentaAppService(
        IPotreroRepository potreros,
        IVentaRepository ventas,
        IProductoFactoryProvider productoFactoryProvider,
        ICalculadoraProvider calculadoras)
    {
        _potreros = potreros;
        _ventas = ventas;
        _productoFactoryProvider = productoFactoryProvider;
        _calculadoras = calculadoras;
    }

    public async Task<string> VenderResAsync(
        string potreroId,
        string nombreRes,
        decimal monto,
        string? estrategiaCobro = null,
        CancellationToken ct = default)
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

        var nombreEstrategia = string.IsNullOrWhiteSpace(estrategiaCobro) ? "Nacional" : estrategiaCobro;
        var calculadora = _calculadoras.Obtener(nombreEstrategia);
        var cobro = calculadora.Calcular(monto);

        var venta = new Venta(potreroId, DateTime.Today, res.Nombre, res.Peso, res.Edad, tipo, cobro.Total);
        if (!potrero.RemoverRes(nombreRes))
            throw new InvalidOperationException("No se pudo remover la res del potrero.");

        await _potreros.SaveAsync(potrero, ct);
        await _ventas.AddAsync(venta, ct);

        return $"Res '{nombreRes}' vendida. {cobro}";
    }

    public async Task<string> VenderProductoAsync(
        string potreroId,
        string tipoProducto,
        string nombreProducto,
        decimal cantidad,
        string unidad,
        decimal precioUnitario,
        string? atributoEspecifico = null,
        string? estrategiaCobro = null,
        CancellationToken ct = default)
    {
        var potrero = await _potreros.GetByIdAsync(potreroId, ct)
            ?? throw new InvalidOperationException($"Potrero '{potreroId}' no encontrado.");

        var factory = _productoFactoryProvider.ObtenerFactory(tipoProducto);
        var producto = factory.Crear(nombreProducto, cantidad, unidad, precioUnitario, atributoEspecifico);

        var subtotal = producto.CalcularMonto();
        var nombreEstrategia = string.IsNullOrWhiteSpace(estrategiaCobro) ? "Nacional" : estrategiaCobro;
        var calculadora = _calculadoras.Obtener(nombreEstrategia);
        var cobro = calculadora.Calcular(subtotal);

        var venta = new Venta(potreroId, DateTime.Today, producto, cobro.Total);
        await _ventas.AddAsync(venta, ct);

        return $"Producto '{producto.Nombre}' ({producto.Tipo}) vendido: {producto.Cantidad} {producto.Unidad}. {cobro}";
    }

    public Task<IReadOnlyList<Venta>> ListarAsync(CancellationToken ct = default) =>
        _ventas.GetAllAsync(ct);
}