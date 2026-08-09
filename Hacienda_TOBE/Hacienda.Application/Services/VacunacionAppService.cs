using Hacienda.Application.Abstractions;
using Hacienda.Domain.Entities;
using Hacienda.Domain.Events;
using Hacienda.Domain.Ports;
using Hacienda.Domain.Policies;

namespace Hacienda.Application.Services;

public class VacunacionAppService : IVacunacionAppService
{
    private readonly IPotreroRepository _potreros;
    private readonly IVacunaRepository _vacunas;
    private readonly IEventPublisher _events;
    private readonly IResPolicy _policy;

    public VacunacionAppService(IPotreroRepository potreros, IVacunaRepository vacunas, IEventPublisher events, IResPolicy? policy = null)
    {
        _potreros = potreros;
        _vacunas = vacunas;
        _events = events;
        _policy = policy ?? new DefaultResPolicy();
    }

    public async Task<string> CrearVacunaBacterianaAsync(string nombre, string lote, DateTime vencimiento, DateTime aplicacion, uint periodo, CancellationToken ct = default)
    {
        var vacuna = new Bacteriana(nombre, lote, vencimiento, aplicacion, periodo);
        var list = (await _vacunas.GetDisponiblesAsync(ct)).ToList();
        list.Add(vacuna);
        await _vacunas.SaveDisponiblesAsync(list, ct);
        return $"Vacuna bacteriana '{nombre}' lote {lote} registrada.";
    }

    public async Task<string> CrearVacunaVivaAsync(string nombre, string lote, DateTime vencimiento, DateTime aplicacion, GradoAtenuacion grado, CancellationToken ct = default)
    {
        var vacuna = new Viva(nombre, lote, vencimiento, aplicacion, grado);
        var list = (await _vacunas.GetDisponiblesAsync(ct)).ToList();
        list.Add(vacuna);
        await _vacunas.SaveDisponiblesAsync(list, ct);
        return $"Vacuna viva '{nombre}' lote {lote} registrada.";
    }

    public async Task<string> AplicarVacunaAsync(string potreroId, string nombreRes, string loteVacuna, CancellationToken ct = default)
    {
        var potrero = await _potreros.GetByIdAsync(potreroId, ct)
            ?? throw new InvalidOperationException($"Potrero '{potreroId}' no encontrado.");
        var res = potrero.BuscarRes(nombreRes)
            ?? throw new InvalidOperationException($"Res '{nombreRes}' no encontrada.");

        var disponibles = await _vacunas.GetDisponiblesAsync(ct);
        var vacuna = disponibles.FirstOrDefault(v => v.Lote.Equals(loteVacuna, StringComparison.OrdinalIgnoreCase))
            ?? throw new InvalidOperationException($"Vacuna con lote '{loteVacuna}' no disponible.");

        if (vacuna.EstaVencida(DateTime.Today))
        {
            await _events.PublishAsync(new VacunaVencidaDetectada(vacuna.Nombre, vacuna.Lote, DateTime.UtcNow), ct);
            throw new InvalidOperationException($"La vacuna '{vacuna.Nombre}' lote {vacuna.Lote} está vencida.");
        }

        int contBac = res.VacunasAplicadas.Count(v => v is Bacteriana);
        int contViv = res.VacunasAplicadas.Count(v => v is Viva);

        var (maxBac, maxViv) = _policy.GetMaxVacunas(res);

        if (vacuna is Bacteriana && contBac >= maxBac)
            throw new InvalidOperationException($"No se puede aplicar más vacunas bacterianas a '{res.Nombre}'. Ya tiene las {maxBac} permitidas.");
        if (vacuna is Viva && contViv >= maxViv)
            throw new InvalidOperationException($"No se puede aplicar más vacunas vivas a '{res.Nombre}'. Ya tiene las {maxViv} permitidas.");

        if (res.VacunasAplicadas.Any(v => v.Lote.Equals(vacuna.Lote, StringComparison.OrdinalIgnoreCase)))
            throw new InvalidOperationException($"La vacuna lote '{vacuna.Lote}' ya fue aplicada a '{res.Nombre}'.");

        res.RegistrarVacuna(vacuna);
        await _potreros.SaveAsync(potrero, ct);
        await _vacunas.SaveAplicadasAsync(potreroId, nombreRes, res.VacunasAplicadas, ct);
        await _events.PublishAsync(new VacunacionCompletada(res.Nombre, vacuna.Lote, DateTime.UtcNow), ct);

        return $"Vacuna '{vacuna.Nombre}' aplicada a '{res.Nombre}'.";
    }

    public Task<IReadOnlyList<Vacuna>> ListarDisponiblesAsync(CancellationToken ct = default) =>
        _vacunas.GetDisponiblesAsync(ct);
}
