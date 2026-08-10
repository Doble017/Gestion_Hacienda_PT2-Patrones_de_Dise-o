# Inversión de dependencias (DIP) — TO-BE

## 1. Definición operativa

> Los módulos de alto nivel no deben depender de módulos de bajo nivel. Ambos deben depender de abstracciones. Las abstracciones no deben depender de detalles.

En este sistema:

- **Alto nivel:** `Hacienda.Application` (casos de uso), `Hacienda.Web` (controllers).
- **Abstracciones:** puertos en `Hacienda.Domain.Ports` e interfaces en `Hacienda.Application.Abstractions`.
- **Bajo nivel:** `File*Repository`, `LoggingEventPublisher`.

---

## 2. Tabla maestra DIP

| Módulo alto nivel | Abstracción (puerto) | Módulo bajo nivel | Dónde se resuelve |
|-------------------|----------------------|-------------------|-------------------|
| `PotreroAppService` | `IPotreroRepository` | `FilePotreroRepository` | `DependencyInjection.cs` |
| `PotreroAppService` | `IEventPublisher` | `LoggingEventPublisher` | idem |
| `ResAppService` | `IPotreroRepository` | `FilePotreroRepository` | idem |
| `ResAppService` | `IEventPublisher` | `LoggingEventPublisher` | idem |
| `VacunacionAppService` | `IPotreroRepository`, `IVacunaRepository`, `IEventPublisher` | File* + Logging* | idem |
| `VentaAppService` | `IPotreroRepository`, `IVentaRepository` | File* | idem |
| `UsuarioAppService` | `IUsuarioRepository` | `FileUsuarioRepository` | idem |
| `PotreroController` | `IPotreroAppService` | `PotreroAppService` | Scoped DI |
| `ResController` | `IResAppService`, `IPotreroAppService`, `IVentaAppService` | *AppService | Scoped DI |
| `VacunaController` | `IVacunacionAppService`, … | *AppService | Scoped DI |
| `VentaController` | `IVentaAppService`, `IResAppService` | *AppService | Scoped DI |
| `AccountController` | `IUsuarioAppService` | `UsuarioAppService` | Scoped DI |

---

## 3. Composition Root

**Archivo:** `Hacienda.Web/Program.cs`

```csharp
var dataPath = Path.Combine(builder.Environment.ContentRootPath, "Datos");
builder.Services.AddHaciendaInfrastructure(dataPath);
```

**Archivo:** `Hacienda.Infrastructure/DependencyInjection.cs`

- Singleton: `FileStoragePaths`, `IPotreroRepository→FilePotreroRepository`, …, `IEventPublisher→LoggingEventPublisher`, `IResPolicy`, `IResFactory`
- Scoped: `IPotreroAppService→PotreroAppService`, …

Único lugar autorizado para acoplar abstracción ↔ concreto.

---

## 4. Dirección de referencias de proyecto (evidencia estructural)

```
Hacienda.Web          → Application, Infrastructure
Hacienda.Application  → Domain
Hacienda.Infrastructure → Domain, Application
Hacienda.Domain       → (ninguna)
```

Domain **no** referencia Infrastructure. Eso es la garantía estática de DIP a nivel de ensamblados.

---

## 5. Contraste con AS-IS

| AS-IS | TO-BE |
|-------|-------|
| Services inyectan `Hacienda` concreta | App Services inyectan `I*Repository` |
| Controllers inyectan `Hacienda` + `PersistenciaService` | Controllers inyectan solo `I*AppService` |
| `new Publisher*` en dominio | `IEventPublisher` inyectado |
| Un `PersistenciaService` de 643 líneas | Repositorios por agregado detrás de puertos |

---

## 6. Prueba mental de sustituibilidad

Para sustituir archivos planos por una base de datos:

1. Crear `SqlPotreroRepository : IPotreroRepository`.
2. Cambiar el registro en `DependencyInjection` / `Program.cs`.
3. **No** modificar `PotreroAppService` ni Controllers.

Eso es DIP operativo.
