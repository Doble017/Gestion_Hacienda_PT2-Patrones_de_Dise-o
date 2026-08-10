# Trazabilidad AS-IS → TO-BE

## 1. Tipos eliminados o reemplazados

| AS-IS | TO-BE | Motivo |
|-------|-------|--------|
| `Hacienda` (God Class) | App Services + repositorios | ADR-01 / SRP |
| `IVacunacion`, `IVentaRes`, `ICreacionVacuna` en una clase | `IVacunacionAppService`, `IVentaAppService`, … | ADR-03 / ISP |
| `PersistenciaService` | `FilePotreroRepository`, `FileVacunaRepository`, `FileVentaRepository`, `FileUsuarioRepository` | ADR-02 |
| `PublisherPesoMin`, … (instanciados en dominio) | `DomainEvent` + `IEventPublisher` / `LoggingEventPublisher` | ADR-04 |
| `Interceptor*` + Castle en persistencia | No portados al TO-BE (validación en entidades/reglas/app services) | Simplificación consciente |
| `IValidarInformacion` + jerarquía Validacion | Reglas en entidades + `Regla*` + validación en App Services | SRP/ISP |

## 2. Tipos conservados (con posible refactor menor)

| Concepto | AS-IS | TO-BE |
|----------|-------|-------|
| Jerarquía Res | `Res`, `Ternero`, `Cebon`, `Novillo` | Igual (LSP) |
| Jerarquía Vacuna | `Vacuna`, `Bacteriana`, `Viva` | Igual |
| Potrero | `Potrero` + enum tipos | `Potrero` + `TipoPotrero` |
| Venta | `Venta` | `Venta` (snapshot) |
| Usuario | `Usuario` | `Usuario` |
| Reglas | `ReglaPotrero/Res/Vacuna` | Igual (static) |
| Controllers | mismos módulos | mismos módulos, dependen de interfaces |

## 3. Trazabilidad de relaciones UML

| Relación AS-IS documentada | Relación TO-BE | ¿Cambió el tipo UML? |
|----------------------------|----------------|----------------------|
| Hacienda → Potrero (asociación) | Eliminada (no hay Hacienda); Potrero vive en repositorio | N/A |
| Potrero → Res 0..150 | Potrero → Res 0..150 | No (asociación) |
| Res → Vacuna 0..\* | Res → Vacuna 0..\* | No |
| Res ← Ternero/Cebon/Novillo | Igual | No |
| Vacuna ← Bacteriana/Viva | Igual | No |
| Services → Hacienda concreta | AppServices → I*Repository | Sí (DIP) |
| Controllers → Hacienda/Persistencia | Controllers → I*AppService | Sí (DIP/ISP) |

## 4. Trazabilidad de funcionalidad UI

| Función original | Endpoint TO-BE |
|------------------|----------------|
| Listar/crear/detalle potrero | `PotreroController` Index/Create/Details |
| Listar/crear/alimentar/vender res | `ResController` Index/Create/Alimentar/Vender |
| Detalle vacunas de res | `ResController.DetalleVacunas` |
| Inventario/crear/aplicar vacuna | `VacunaController` Index/Create/Aplicar |
| Listar/crear venta | `VentaController` Index/Create |
| Listar/crear usuario | `UsuarioController` Index/Create |
| Login/logout | `AccountController` |

## 5. Archivos clave de evidencia

| Tema | Ruta |
|------|------|
| Entidades | `Hacienda.Domain/Entities/*` |
| Puertos | `Hacienda.Domain/Ports/*` |
| App contracts | `Hacienda.Application/Abstractions/IAppServices.cs` |
| DI / Composition Root | `Hacienda.Infrastructure/DependencyInjection.cs`, `Hacienda.Web/Program.cs` |
| UML capas | `02_Diagramas/01_UML_TO-BE_Capas.drawio` |
| UML dominio | `02_Diagramas/02_UML_TO-BE_Dominio.drawio` |
| UML DIP | `02_Diagramas/03_UML_TO-BE_DIP_y_Application.drawio` |
