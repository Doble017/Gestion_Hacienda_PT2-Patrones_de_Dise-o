using Hacienda.Application.Abstractions;
using Hacienda.Domain.Entities;
using Hacienda.Domain.Ports;

namespace Hacienda.Application.Services;

public class VentaAppService : IVentaAppService
{
    private readonly IPotreroRepository _potreros;
    private readonly IVentaRepository _ventas;

    public VentaAppService(IPotreroRepository potreros, IVentaRepository ventas)
    {
        _potreros = potreros;
        _ventas = ventas;
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

    public Task<IReadOnlyList<Venta>> ListarAsync(CancellationToken ct = default) =>
        _ventas.GetAllAsync(ct);
}
