using Hacienda.Application.Abstractions;
using Hacienda.Application.Services;
using Hacienda.Domain.Entities;
using Hacienda.Domain.Factories;
using Hacienda.Domain.Ports;
using Hacienda.Infrastructure.Events;
using Hacienda.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using Hacienda.Domain.Strategies;


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

        // --- Strategy de cobro: todas disponibles; el usuario elige en la UI ---
        services.AddSingleton<SinDescuento>();
        services.AddSingleton<ConDescuento>(_ => new ConDescuento(10m)); // 10 % fijo
        services.AddSingleton<Nacional>();
        services.AddSingleton<Internacional>();

        services.AddSingleton<ICalculadoraProvider>(sp =>
        {
            var mapa = new Dictionary<string, ICalculadora>(StringComparer.OrdinalIgnoreCase)
            {
                ["SinDescuento"]   = sp.GetRequiredService<SinDescuento>(),
                ["ConDescuento"]   = sp.GetRequiredService<ConDescuento>(),
                ["Nacional"]       = sp.GetRequiredService<Nacional>(),
                ["Internacional"]  = sp.GetRequiredService<Internacional>()
            };
            return new CalculadoraProvider(mapa);
        });

        services.AddScoped<IPotreroAppService, PotreroAppService>();
        services.AddScoped<IResAppService, ResAppService>();
        services.AddScoped<IVacunacionAppService, VacunacionAppService>();
        services.AddScoped<IVentaAppService, VentaAppService>();
        services.AddScoped<IUsuarioAppService, UsuarioAppService>();

        services.AddScoped<IPotreroAppService, PotreroAppService>();
        services.AddScoped<IResAppService, ResAppService>();
        services.AddScoped<IVacunacionAppService, VacunacionAppService>();
        services.AddScoped<IVentaAppService, VentaAppService>();
        services.AddScoped<IUsuarioAppService, UsuarioAppService>();

        // --- Fachada (único punto de acceso desde Presentation) ---
        services.AddScoped<IHaciendaFachada, HaciendaFachada>();

        return services;
    }
}