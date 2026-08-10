# Capas y paquetes TO-BE

## 1. Proyectos de la solution

| Proyecto | Capa | Dependencias de proyecto |
|----------|------|---------------------------|
| `Hacienda.Domain` | Domain | Ninguna (núcleo) |
| `Hacienda.Application` | Application | → Domain |
| `Hacienda.Infrastructure` | Infrastructure | → Domain, → Application |
| `Hacienda.Web` | Presentation | → Application, → Infrastructure |

## 2. Regla de dependencia

```
Web (Presentation)
    │  usa abstracciones de Application
    ▼
Application (casos de uso)
    │  usa entidades + puertos de Domain
    ▼
Domain (entidades, reglas, puertos)
    ▲
    │  implementa puertos
Infrastructure (File*Repository, LoggingEventPublisher)
```

Composition Root: `Hacienda.Web/Program.cs` → `AddHaciendaInfrastructure(...)`.

## 3. Contenido por carpeta

### Domain
- `Entities/` — Potrero, Res hierarchy, Vacuna hierarchy, Venta, Usuario
- `Rules/` — ReglaPotrero, ReglaRes, ReglaVacuna
- `Events/` — DomainEvent + 6 records
- `Ports/` — I*Repository, IEventPublisher
- `Factories/` — IResFactory, DefaultResFactory
- `Policies/` — IResPolicy, DefaultResPolicy

### Application
- `Abstractions/` — IPotreroAppService, IResAppService, IVacunacionAppService, IVentaAppService, IUsuarioAppService
- `Services/` — implementaciones de esos contratos

### Infrastructure
- `Persistence/` — FileStoragePaths, File*Repository
- `Events/` — LoggingEventPublisher
- `DependencyInjection.cs` — registro de concretos y app services

### Web
- `Controllers/` — Account, Home, Potrero, Res, Vacuna, Venta, Usuario
- `Views/`, `Models/`, `Datos/*.txt`
