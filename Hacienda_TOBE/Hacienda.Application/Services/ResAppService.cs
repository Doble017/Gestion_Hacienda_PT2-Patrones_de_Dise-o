using Hacienda.Application.Abstractions;
using Hacienda.Domain.Entities;
using Hacienda.Domain.Events;
using Hacienda.Domain.Ports;
using Hacienda.Domain.Policies;

namespace Hacienda.Application.Services;

public class ResAppService : IResAppService
{
    private readonly IPotreroRepository _potreros;
    private readonly IEventPublisher _events;
    private readonly IResPolicy _policy;

    public ResAppService(IPotreroRepository potreros, IEventPublisher events, IResPolicy? policy = null)
    {
        _potreros = potreros;
        _events = events;
        _policy = policy ?? new DefaultResPolicy();
    }

    public async Task<IReadOnlyList<(Potrero Potrero, Res Res)>> ListarTodasAsync(CancellationToken ct = default)
    {
        var potreros = await _potreros.GetAllAsync(ct);
        var list = new List<(Potrero, Res)>();
        foreach (var p in potreros)
            foreach (var r in p.Reses)
                list.Add((p, r));
        return list;
    }

    public async Task<Res?> BuscarAsync(string potreroId, string nombreRes, CancellationToken ct = default)
    {
        var potrero = await _potreros.GetByIdAsync(potreroId, ct);
        return potrero?.BuscarRes(nombreRes);
    }

    public async Task<string> AlimentarAsync(string potreroId, string nombreRes, uint incremento, CancellationToken ct = default)
    {
        var potrero = await _potreros.GetByIdAsync(potreroId, ct)
            ?? throw new InvalidOperationException($"Potrero '{potreroId}' no encontrado.");
        var res = potrero.BuscarRes(nombreRes)
            ?? throw new InvalidOperationException($"Res '{nombreRes}' no encontrada.");

        res.Alimentar(incremento);

        uint pesoMin = _policy.GetPesoMinimo(res);
        uint pesoIdeal = _policy.GetPesoIdealVenta(res);

        if (res.Peso >= pesoMin)
            await _events.PublishAsync(new PesoMinimoAlcanzado(res.Nombre, res.Peso, DateTime.UtcNow), ct);
        if (res.Peso >= pesoIdeal)
            await _events.PublishAsync(new PesoIdealVenta(res.Nombre, res.Peso, DateTime.UtcNow), ct);

        await _potreros.SaveAsync(potrero, ct);
        return $"Res '{nombreRes}' alimentada. Peso actual: {res.Peso} kg.";
    }
}
