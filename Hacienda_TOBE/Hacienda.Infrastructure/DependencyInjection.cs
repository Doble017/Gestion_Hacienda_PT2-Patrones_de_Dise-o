using Hacienda.Application.Abstractions;
using Hacienda.Application.Services;
using Hacienda.Domain.Entities;
using Hacienda.Domain.Factories;
using Hacienda.Domain.Ports;
using Hacienda.Infrastructure.Events;
using Hacienda.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace Hacienda.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddHaciendaInfrastructure(this IServiceCollection services, string dataDirectory)
    {
        services.AddSingleton(new FileStoragePaths(dataDirectory));
        services.AddSingleton<IVacunaRepository, FileVacunaRepository>();
        services.AddSingleton<IVentaRepository, FileVentaRepository>();
        services.AddSingleton<IUsuarioRepository, FileUsuarioRepository>();
        services.AddSingleton<IEventPublisher, LoggingEventPublisher>();
        services.AddSingleton<Hacienda.Domain.Policies.IResPolicy, Hacienda.Domain.Policies.DefaultResPolicy>();

        // --- Fábricas de reses (Factory Method + Provider) ---
        services.AddSingleton<TerneroFactory>();
        services.AddSingleton<CebonFactory>();
        services.AddSingleton<NovilloFactory>();

        services.AddSingleton<IResFactoryProvider>(sp =>
        {
            var fabricas = new Dictionary<TipoPotrero, IResFactory>
            {
                [TipoPotrero.Ternero] = sp.GetRequiredService<TerneroFactory>(),
                [TipoPotrero.Cebon]   = sp.GetRequiredService<CebonFactory>(),
                [TipoPotrero.Novillo] = sp.GetRequiredService<NovilloFactory>()
            };
            return new ResFactoryProvider(fabricas);
        });

        // FilePotreroRepository necesita el provider para reconstruir potreros
        services.AddSingleton<IPotreroRepository>(sp =>
            new FilePotreroRepository(
                sp.GetRequiredService<FileStoragePaths>(),
                sp.GetRequiredService<IResFactoryProvider>()));

        // --- Fábricas de productos derivados (Factory Method + Provider) ---
        services.AddSingleton<CarneFactory>();
        services.AddSingleton<PielFactory>();
        services.AddSingleton<LacteoFactory>();

        services.AddSingleton<IProductoFactoryProvider>(sp =>
        {
            var fabricas = new Dictionary<string, IProductoFactory>(StringComparer.OrdinalIgnoreCase)
            {
                ["Carne"]  = sp.GetRequiredService<CarneFactory>(),
                ["Piel"]   = sp.GetRequiredService<PielFactory>(),
                ["Lacteo"] = sp.GetRequiredService<LacteoFactory>()
            };
            return new ProductoFactoryProvider(fabricas);
        });

        services.AddScoped<IPotreroAppService, PotreroAppService>();
        services.AddScoped<IResAppService, ResAppService>();
        services.AddScoped<IVacunacionAppService, VacunacionAppService>();
        services.AddScoped<IVentaAppService, VentaAppService>();
        services.AddScoped<IUsuarioAppService, UsuarioAppService>();

        return services;
    }
}