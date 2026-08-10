# ADR-04 — Extracción de publishers a adaptadores

| Campo | Valor |
|-------|-------|
| **Estado** | Aceptado |
| **Fecha** | Agosto 2026 |
| **Principios involucrados** | DIP, OCP |
| **Hallazgo(s) del inventario** | **H-01** (`new PublisherPesoMin()` y similares dentro del dominio), **H-04** (suscripciones y lógica de eventos duplicada / enganchada en `Potrero` y `Hacienda`) |

---

## 1. Contexto y evidencia

El dominio AS-IS **instanciaba** publishers concretos (`new Publisher*`) y en algunos puntos suscribía lambdas dentro de métodos de negocio. Eso acoplaba el núcleo de dominio a tipos de notificación y hacía imposible cambiar el canal (log, email, bus) sin editar entidades u orquestadores de dominio.

Evidencia: hallazgos H-01 y H-04; referencias a `new Publisher` en el inventario de código AS-IS.

---

## 2. Alternativas evaluadas

### Alternativa A — Inyectar publishers concretos en el dominio
- **Descripción:** pasar `PublisherPesoMin` por constructor a `Hacienda`/`Potrero`.
- **Pros:** elimina el `new` directo.
- **Contras:** el dominio sigue dependiendo de tipos de infraestructura; no hay abstracción estable.
- **Resultado:** **Descartada**.

### Alternativa B — Puerto `IEventPublisher` + implementación en Infrastructure
- **Descripción:** Domain define `DomainEvent` y records; Application publica vía `IEventPublisher`; Infrastructure implementa (`LoggingEventPublisher`).
- **Pros:** DIP correcto; OCP para nuevos canales; dominio libre de I/O de notificación.
- **Contras:** un nivel más de indirección al depurar.
- **Resultado:** **Elegida**.

### Alternativa C — Introducir MediatR / bus completo de integración
- **Descripción:** pipeline de handlers, notificaciones out-of-process, etc.
- **Pros:** potencia para sistemas grandes.
- **Contras:** sobre-ingeniería para el alcance actual y el comportamiento observable (logging).
- **Resultado:** **Descartada**.

---

## 3. Decisión

- **Domain:** `DomainEvent` + eventos (`PesoMinimoAlcanzado`, `PesoIdealVenta`, `PotreroMitadCapacidad`, `PotreroLleno`, `VacunacionCompletada`, `VacunaVencidaDetectada`) y puerto `IEventPublisher`.
- **Application:** los App Services publican tras mutaciones relevantes.
- **Infrastructure:** `LoggingEventPublisher`.
- **Composition Root:** registro `IEventPublisher → LoggingEventPublisher` (Singleton).

---

## 4. Costo / consecuencia negativa aceptada

- Indirección adicional al seguir el flujo de un evento en depuración.
- El publisher actual solo registra en log (suficiente para el TO-BE); canales ricos quedan para una SC futura vía nueva implementación del puerto.

---

## 5. Principios involucrados y ganancia

| Principio | Cómo se aplica |
|-----------|----------------|
| **DIP** | Alto nivel (App Services) → `IEventPublisher` ← bajo nivel (`LoggingEventPublisher`) |
| **OCP** | Nuevo canal = nueva clase + registro; sin editar Domain ni App Services de negocio |

**Ganancia:** el dominio deja de conocer infraestructura de notificación; H-01 y H-04 quedan cerrados.
