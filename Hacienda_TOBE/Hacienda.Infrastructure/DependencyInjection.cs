using Hacienda.Application.Abstractions;
using Hacienda.Application.Services;
using Hacienda.Domain.Ports;
using Hacienda.Infrastructure.Events;
using Hacienda.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace Hacienda.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddHaciendaInfrastructure(this IServiceCollection services, string dataDirectory)
    {
        services.AddSingleton(new FileStoragePaths(dataDirectory));
        services.AddSingleton<IPotreroRepository, FilePotreroRepository>();
        services.AddSingleton<IVacunaRepository, FileVacunaRepository>();
        services.AddSingleton<IVentaRepository, FileVentaRepository>();
        services.AddSingleton<IUsuarioRepository, FileUsuarioRepository>();
        services.AddSingleton<IEventPublisher, LoggingEventPublisher>();
        services.AddSingleton<Hacienda.Domain.Policies.IResPolicy, Hacienda.Domain.Policies.DefaultResPolicy>();
        services.AddSingleton<Hacienda.Domain.Factories.IResFactory, Hacienda.Domain.Factories.DefaultResFactory>();

        services.AddScoped<IPotreroAppService, PotreroAppService>();
        services.AddScoped<IResAppService, ResAppService>();
        services.AddScoped<IVacunacionAppService, VacunacionAppService>();
        services.AddScoped<IVentaAppService, VentaAppService>();
        services.AddScoped<IUsuarioAppService, UsuarioAppService>();

        return services;
    }
}
