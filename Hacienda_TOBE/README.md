# Hacienda TO-BE — Arquitectura SOLID

Modernización del sistema según diseño TO-BE (Fase 3).

## Estructura

- **Hacienda.Domain** — Entidades, reglas, eventos, puertos
- **Hacienda.Application** — App Services segregados (ISP)
- **Hacienda.Infrastructure** — File repositories + EventPublisher
- **Hacienda.Web** — ASP.NET Core MVC

Regla: Presentation → Application → Domain ← Infrastructure

## SOLID

| Principio | Intervención |
|-----------|---------------|
| SRP | God Class Hacienda eliminada; App Services + repositorios por agregado |
| OCP | IEventPublisher; extensión sin modificar núcleo |
| LSP | Jerarquías Res y Vacuna conservadas (ADR-05) |
| ISP | IPotrero/Res/Vacunacion/Venta/Usuario AppService |
| DIP | Controllers y App Services dependen de puertos; Composition Root en Program.cs |

## Ejecutar

```bash
cd Hacienda.Web
dotnet run
```

Login: `santi` / `santi11`

Datos en `Hacienda.Web/Datos/*.txt`
