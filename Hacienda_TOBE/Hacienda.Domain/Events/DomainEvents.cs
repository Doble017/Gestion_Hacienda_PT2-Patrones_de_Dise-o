namespace Hacienda.Domain.Events;

public abstract record DomainEvent(DateTime OccurredAt);

public record PesoMinimoAlcanzado(string NombreRes, uint Peso, DateTime OccurredAt) : DomainEvent(OccurredAt);
public record PesoIdealVenta(string NombreRes, uint Peso, DateTime OccurredAt) : DomainEvent(OccurredAt);
public record PotreroMitadCapacidad(string PotreroId, int Cantidad, DateTime OccurredAt) : DomainEvent(OccurredAt);
public record PotreroLleno(string PotreroId, int Cantidad, DateTime OccurredAt) : DomainEvent(OccurredAt);
public record VacunacionCompletada(string NombreRes, string LoteVacuna, DateTime OccurredAt) : DomainEvent(OccurredAt);
public record VacunaVencidaDetectada(string NombreVacuna, string Lote, DateTime OccurredAt) : DomainEvent(OccurredAt);
