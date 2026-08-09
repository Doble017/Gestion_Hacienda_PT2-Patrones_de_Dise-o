using Hacienda.Domain.Events;
using Hacienda.Domain.Ports;
using Microsoft.Extensions.Logging;

namespace Hacienda.Infrastructure.Events;

/// <summary>Adaptador de infraestructura: publica eventos de dominio (ADR-04).</summary>
public class LoggingEventPublisher : IEventPublisher
{
    private readonly ILogger<LoggingEventPublisher> _logger;

    public LoggingEventPublisher(ILogger<LoggingEventPublisher> logger) => _logger = logger;

    public Task PublishAsync(DomainEvent domainEvent, CancellationToken ct = default)
    {
        _logger.LogInformation("DomainEvent {EventType}: {@Event}", domainEvent.GetType().Name, domainEvent);
        return Task.CompletedTask;
    }
}
