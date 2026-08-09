using Hacienda.Domain.Events;

namespace Hacienda.Domain.Ports;

/// <summary>Puerto de publicación de eventos de dominio (DIP / ADR-04).</summary>
public interface IEventPublisher
{
    Task PublishAsync(DomainEvent domainEvent, CancellationToken ct = default);
}
