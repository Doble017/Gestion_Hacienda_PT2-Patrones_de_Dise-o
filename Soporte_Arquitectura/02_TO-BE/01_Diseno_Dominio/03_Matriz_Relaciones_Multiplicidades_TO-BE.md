# Matriz de relaciones y multiplicidades TO-BE

Fuente: código en `Hacienda.Domain` y `Hacienda.Application` / `Infrastructure`.

## 1. Generalizaciones

| Superclase | Subclase | Evidencia |
|------------|----------|-----------|
| `Res` | `Ternero` | `class Ternero : Res` |
| `Res` | `Cebon` | `class Cebon : Res` |
| `Res` | `Novillo` | `class Novillo : Res` |
| `Vacuna` | `Bacteriana` | `class Bacteriana : Vacuna` |
| `Vacuna` | `Viva` | `class Viva : Vacuna` |
| `DomainEvent` | 6 records de evento | `record X(...) : DomainEvent` |

## 2. Realizaciones

| Interfaz | Implementación | Capa |
|----------|----------------|------|
| `IPotreroRepository` | `FilePotreroRepository` | Infrastructure |
| `IVacunaRepository` | `FileVacunaRepository` | Infrastructure |
| `IVentaRepository` | `FileVentaRepository` | Infrastructure |
| `IUsuarioRepository` | `FileUsuarioRepository` | Infrastructure |
| `IEventPublisher` | `LoggingEventPublisher` | Infrastructure |
| `IPotreroAppService` | `PotreroAppService` | Application |
| `IResAppService` | `ResAppService` | Application |
| `IVacunacionAppService` | `VacunacionAppService` | Application |
| `IVentaAppService` | `VentaAppService` | Application |
| `IUsuarioAppService` | `UsuarioAppService` | Application |
| `IResFactory` | `DefaultResFactory` | Domain |
| `IResPolicy` | `DefaultResPolicy` | Domain |

## 3. Asociaciones de dominio (con multiplicidad)

| Origen | Destino | Multiplicidad | Evidencia |
|--------|---------|---------------|-----------|
| `Potrero` | `Res` | 1 → 0..150 | `_reses` + `ReglaPotrero.MaxReses` |
| `Res` | `Vacuna` | 1 → 0..\* | `_vacunasAplicadas` |
| `Potrero` | `TipoPotrero` | 1 → 1 | propiedad `Tipo` |
| `Viva` | `GradoAtenuacion` | 1 → 1 | propiedad |

## 4. Asociaciones Application / Presentation

| Origen | Destino | Tipo |
|--------|---------|------|
| `PotreroController` | `IPotreroAppService` | Asociación (inyección) |
| `ResController` | `IResAppService`, `IPotreroAppService`, `IVentaAppService` | Asociación |
| `VacunaController` | `IVacunacionAppService`, `IPotreroAppService`, `IResAppService` | Asociación |
| `VentaController` | `IVentaAppService`, `IResAppService` | Asociación |
| `UsuarioController` / `AccountController` | `IUsuarioAppService` | Asociación |
| `*AppService` | `I*Repository` / `IEventPublisher` | Asociación |

## 5. Política sobre composición

No se documentan relaciones de **composición UML** para colecciones internas (`List<Res>`, etc.), coherente con la matriz AS-IS: la existencia de colección no implica por sí sola composition.
