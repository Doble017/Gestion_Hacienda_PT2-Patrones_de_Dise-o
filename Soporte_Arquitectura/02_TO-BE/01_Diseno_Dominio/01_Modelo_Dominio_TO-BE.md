# Diseño de Dominio TO-BE

**Sistema:** Hacienda (Bib_Hacienda modernizada)  
**Proyecto de código:** `Hacienda.Domain`  
**Fecha:** Agosto 2026  
**Alcance:** Entidades, value objects implícitos, reglas, eventos, puertos y fábricas/políticas presentes en el código.

---

## 1. Objetivo

Documentar el modelo de dominio **tal como está implementado** en `Hacienda.Domain`, sin idealizar ni anticipar extensiones no codificadas.

---

## 2. Bounded context

Un único contexto: **Gestión operativa de hacienda ganadera**.

Responsabilidades del dominio:

- Representar potreros, reses, vacunas, ventas y usuarios.
- Aplicar reglas de capacidad, edad y límites de vacunación.
- Emitir hechos de dominio (eventos) sin conocer infraestructura.
- Exponer **puertos** (interfaces) que Application e Infrastructure consumen o implementan.

---

## 3. Catálogo de tipos de dominio

### 3.1. Entidades

| Tipo | Archivo | Naturaleza | Responsabilidad |
|------|---------|------------|-----------------|
| `Potrero` | `Entities/Potrero.cs` | Agregado raíz de reses | Identidad, tipo, colección de reses, alta/búsqueda/remoción |
| `Res` | `Entities/Res.cs` | Abstracta | Nombre, peso, edad abstracta, vacunas aplicadas |
| `Ternero` | `Entities/Res.cs` | Especialización | Edad ≤ 12 meses |
| `Cebon` | `Entities/Res.cs` | Especialización | Edad 13–48 meses |
| `Novillo` | `Entities/Res.cs` | Especialización | Edad > 48 meses |
| `Vacuna` | `Entities/Vacuna.cs` | Abstracta | Nombre, lote, fechas, vencimiento |
| `Bacteriana` | `Entities/Vacuna.cs` | Especialización | Periodo de aplicación |
| `Viva` | `Entities/Vacuna.cs` | Especialización | Grado de atenuación |
| `Venta` | `Entities/Venta.cs` | Entidad de registro | Snapshot de venta (potrero, res, monto, fecha) |
| `Usuario` | `Entities/Usuario.cs` | Entidad | Credenciales y validación |

### 3.2. Enumeraciones

| Tipo | Valores | Uso |
|------|---------|-----|
| `TipoPotrero` | Ternero, Cebon, Novillo | Clasifica el potrero y condiciona el tipo de res creada |
| `GradoAtenuacion` | Baja, Media, Alta | Atributo de `Viva` |

### 3.3. Reglas estáticas

| Clase | Constantes relevantes |
|-------|----------------------|
| `ReglaPotrero` | `MaxReses = 150` |
| `ReglaRes` | pesos mínimos/recomendados; `EdadMaxTernero = 12`; `EdadMaxCebon = 48` |
| `ReglaVacuna` | máximos bacterianas/vivas por tipo de res |

### 3.4. Eventos de dominio

Jerarquía: `DomainEvent` (record abstracto) ← eventos concretos.

| Evento | Cuándo se publica (desde Application) |
|--------|----------------------------------------|
| `PesoMinimoAlcanzado` | Tras alimentar si peso ≥ mínimo del tipo |
| `PesoIdealVenta` | Tras alimentar si peso ≥ ideal de venta |
| `PotreroMitadCapacidad` | Al añadir res y count == Max/2 |
| `PotreroLleno` | Al añadir res y count ≥ Max |
| `VacunacionCompletada` | Tras aplicar vacuna exitosamente |
| `VacunaVencidaDetectada` | Al intentar aplicar vacuna vencida |

### 3.5. Puertos (interfaces de dominio)

| Puerto | Implementación en Infrastructure |
|--------|----------------------------------|
| `IPotreroRepository` | `FilePotreroRepository` |
| `IVacunaRepository` | `FileVacunaRepository` |
| `IVentaRepository` | `FileVentaRepository` |
| `IUsuarioRepository` | `FileUsuarioRepository` |
| `IEventPublisher` | `LoggingEventPublisher` |

### 3.6. Fábrica y política

| Tipo | Rol |
|------|-----|
| `IResFactory` / `DefaultResFactory` | Creación de `Res` según `TipoPotrero` |
| `IResPolicy` / `DefaultResPolicy` | Política de pesos mínimos/ideales por tipo |

---

## 4. Relaciones estructurales del dominio (evidencia en código)

| Origen | Destino | Tipo UML | Evidencia |
|--------|---------|----------|-----------|
| `Res` | `Ternero`, `Cebon`, `Novillo` | Generalización | `class Ternero : Res` |
| `Vacuna` | `Bacteriana`, `Viva` | Generalización | `class Bacteriana : Vacuna` |
| `Potrero` | `Res` | Asociación 1 → 0..\* | `List<Res> _reses` + `IReadOnlyList` |
| `Res` | `Vacuna` | Asociación 1 → 0..\* | `List<Vacuna> _vacunasAplicadas` |
| `Potrero` | `TipoPotrero` | Asociación | propiedad `Tipo` |
| `Viva` | `GradoAtenuacion` | Asociación | propiedad `GradoAtenuacion` |
| `Venta` | (datos escalares) | — | No mantiene referencia viva a `Res`/`Potrero`; guarda ids/nombres (snapshot) |
| `DefaultResFactory` | `IResFactory` | Realización | `sealed class DefaultResFactory : IResFactory` |
| `DefaultResPolicy` | `IResPolicy` | Realización | `sealed class DefaultResPolicy : IResPolicy` |
| `File*Repository` | `I*Repository` | Realización | Infrastructure |
| `LoggingEventPublisher` | `IEventPublisher` | Realización | Infrastructure |

> **Nota metodológica:** No se usa composición UML para colecciones internas. El AS-IS y el TO-BE documentan asociaciones cuando hay referencia estructural; el ciclo de vida de las partes no está formalizado como composition en el código.

---

## 5. Invariantes de dominio

1. Nombre de res y potrero no vacíos.
2. Edad de subtipo dentro del rango de `ReglaRes`.
3. Un potrero no supera `ReglaPotrero.MaxReses` reses.
4. No hay dos reses con el mismo nombre (case-insensitive) en el mismo potrero.
5. Límites de vacunas bacterianas/vivas por tipo de res (`ReglaVacuna`).
6. No se aplica vacuna con lote duplicado a la misma res.
7. No se aplica vacuna vencida (se emite `VacunaVencidaDetectada` y se lanza excepción).

---

## 6. Lo que el dominio NO conoce

- ASP.NET Core / HTTP / cookies.
- Rutas de archivos ni formato `.txt`.
- Controllers ni ViewModels.
- Implementaciones concretas de repositorios o del publisher (solo puertos).

Eso se garantiza por la dirección de referencias de proyectos: `Domain` no referencia a `Application`, `Infrastructure` ni `Web`.
