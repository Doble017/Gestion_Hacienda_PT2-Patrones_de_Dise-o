# ADR-03 — Segregación de interfaces de aplicación

| Campo | Valor |
|-------|-------|
| **Estado** | Aceptado |
| **Fecha** | Agosto 2026 |
| **Principios involucrados** | ISP, DIP |
| **Hallazgo(s) del inventario** | **H-02** (`Hacienda` implementaba `IVacunacion`, `IVentaRes` e `ICreacionVacuna`; cualquier cliente dependía de la superficie completa) |

---

## 1. Contexto y evidencia

En el AS-IS, aunque existían tres interfaces de capacidad, **una sola clase** las implementaba todas. Los services y, en la práctica, el grafo de dependencias trataban a `Hacienda` como contrato único. Un consumidor de ventas quedaba expuesto a miembros de vacunación y creación de vacunas, violando ISP y endureciendo el acoplamiento.

Evidencia: hallazgo H-02; lista de interfaces implementadas por `Hacienda` en el inventario.

---

## 2. Alternativas evaluadas

### Alternativa A — Una sola `IHaciendaService` con todos los métodos
- **Descripción:** formalizar el monolito como un único contrato de aplicación.
- **Pros:** un solo registro DI.
- **Contras:** ISP se viola de forma explícita; cualquier cambio de firma impacta a todos los clientes.
- **Resultado:** **Descartada**.

### Alternativa B — Interfaces segregadas por capacidad de negocio
- **Descripción:** `IPotreroAppService`, `IResAppService`, `IVacunacionAppService`, `IVentaAppService`, `IUsuarioAppService`.
- **Pros:** cada controller depende solo de lo que usa; alinea ISP con SRP de ADR-01.
- **Contras:** más registros en el contenedor.
- **Resultado:** **Elegida**.

### Alternativa C — Una interfaz por método (extrema)
- **Descripción:** `ICrearPotrero`, `IListarPotreros`, etc.
- **Pros:** segregación máxima.
- **Contras:** explosión de tipos; ruido sin beneficio claro para el tamaño del sistema.
- **Resultado:** **Descartada**.

---

## 3. Decisión

Se publican cinco contratos de aplicación en `Hacienda.Application/Abstractions`:

| Contrato | Consumidores principales |
|----------|--------------------------|
| `IPotreroAppService` | `PotreroController` |
| `IResAppService` | `ResController`, `VacunaController` |
| `IVacunacionAppService` | `VacunaController` |
| `IVentaAppService` | `VentaController`, `ResController` |
| `IUsuarioAppService` | `AccountController`, `UsuarioController` |

Las implementaciones viven en `Hacienda.Application/Services` y se registran como **Scoped** en el Composition Root.

---

## 4. Costo / consecuencia negativa aceptada

- Más interfaces y más líneas de registro DI.
- Algunos controllers inyectan 2–3 contratos (p. ej. `ResController` usa res + potrero + venta); es aceptable frente a un super-contrato.

---

## 5. Principios involucrados y ganancia

| Principio | Cómo se aplica |
|-----------|----------------|
| **ISP** | Ningún cliente se ve forzado a depender de métodos que no usa |
| **DIP** | Controllers dependen de abstracciones, no de clases concretas de Application |

**Ganancia:** un cambio en la firma de vacunación no obliga a recompilar ni a mockear el módulo de usuarios.
