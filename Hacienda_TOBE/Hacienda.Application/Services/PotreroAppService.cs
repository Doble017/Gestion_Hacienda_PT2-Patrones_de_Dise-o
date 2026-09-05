using Hacienda.Application.Abstractions;
using Hacienda.Domain.Entities;
using Hacienda.Domain.Events;
using Hacienda.Domain.Factories;
using Hacienda.Domain.Ports;
using Hacienda.Domain.Rules;

namespace Hacienda.Application.Services;

public class PotreroAppService : IPotreroAppService
{
    private readonly IPotreroRepository _potreros;
    private readonly IEventPublisher _events;
    private readonly IResFactoryProvider _resFactoryProvider;

    public PotreroAppService(
        IPotreroRepository potreros,
        IEventPublisher events,
        IResFactoryProvider resFactoryProvider)
    {
        _potreros = potreros;
        _events = events;
        _resFactoryProvider = resFactoryProvider;
    }

    public async Task<string> CrearPotreroAsync(string identificacion, TipoPotrero tipo, CancellationToken ct = default)
    {
        var existentes = await _potreros.GetAllAsync(ct);
        if (existentes.Any(p => p.Identificacion.Equals(identificacion, StringComparison.OrdinalIgnoreCase)))
            throw new InvalidOperationException($"Ya existe el potrero '{identificacion}'.");

        // El Provider entrega la fábrica concreta según el tipo de potrero
        var factory = _resFactoryProvider.ObtenerFactory(tipo);
        var potrero = new Potrero(identificacion, tipo, factory);

        await _potreros.SaveAsync(potrero, ct);
        return $"Potrero '{identificacion}' creado ({tipo}).";
    }

    public Task<IReadOnlyList<Potrero>> ListarAsync(CancellationToken ct = default) =>
        _potreros.GetAllAsync(ct);

    public Task<Potrero?> ObtenerAsync(string identificacion, CancellationToken ct = default) =>
        _potreros.GetByIdAsync(identificacion, ct);

    public async Task<string> AgregarResAsync(string potreroId, string nombre, ushort edad, uint peso, CancellationToken ct = default)
    {
        var potrero = await _potreros.GetByIdAsync(potreroId, ct)
            ?? throw new InvalidOperationException($"Potrero '{potreroId}' no encontrado.");

        var msg = potrero.AnadirRes(nombre, edad, peso);

        int count = potrero.Reses.Count;
        if (count == ReglaPotrero.MaxReses / 2)
            await _events.PublishAsync(new PotreroMitadCapacidad(potreroId, count, DateTime.UtcNow), ct);
        if (count >= ReglaPotrero.MaxReses)
            await _events.PublishAsync(new PotreroLleno(potreroId, count, DateTime.UtcNow), ct);

        await _potreros.SaveAsync(potrero, ct);
        return msg;
    }
}