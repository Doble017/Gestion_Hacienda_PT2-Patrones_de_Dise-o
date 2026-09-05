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

## Ejecutar (defensa oficial: consola de dominio, pedido en clase)

```bash
cd Hacienda.Consola
dotnet run
```

Menú: 1 listar · 2 crear potrero · 3 agregar res · 4 alimentar · 5 aplicar vacuna ·
6 asignar chip GPS (SC-02) · 7 quitar chip · 8 vender · 0 salir.
Datos compartidos en `Hacienda.Web/Datos/*.txt` (mismo `FileStoragePaths`, DIP intacto).

Verificación rápida: `dotnet test` (5 pruebas: 2 potrero + 3 chips SC-02).

## Anexo Web (no parte de la defensa)

```bash
cd Hacienda.Web
dotnet run
```

Login: `santi` / `santi11`. Se conserva como anexo funcional (columna `Chip GPS` en Reses).
