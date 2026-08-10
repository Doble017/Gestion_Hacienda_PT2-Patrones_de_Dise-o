# ADR-05 — Conservar jerarquías `Res` y `Vacuna` (LSP)

| Campo | Valor |
|-------|-------|
| **Estado** | Aceptado |
| **Fecha** | Agosto 2026 |
| **Principios involucrados** | LSP, OCP (evitar sobre-ingeniería) |
| **Hallazgo(s) del inventario** | **H-06** (propuesta de herramientas/IA de “eliminar herencia a favor de composición + Strategy”) — **refutado tras verificación LSP** |

---

## 1. Contexto y evidencia

Existen jerarquías estables en el AS-IS:

```
Res ← Ternero | Cebon | Novillo
Vacuna ← Bacteriana | Viva
```

Un hallazgo automatizado (H-06) sugiere eliminar esa herencia. Antes de aceptar un rediseño costoso, se verificó LSP sobre el código real: precondiciones, postcondiciones, invariantes y excepciones (ver `04_LSP/01_Verificacion_LSP_Res_Vacuna.md`).

Resultado de la verificación: los subtipos **son sustituibles** por el tipo base en los usos actuales (listados, alimentación, vacunación, venta, persistencia). Las restricciones de edad/periodo se aplican en **construcción**, no debilitan postcondiciones de `Alimentar` / `RegistrarVacuna` / `EstaVencida`.

---

## 2. Alternativas evaluadas

### Alternativa A — Eliminar herencia y modelar etapa de vida con composición + Strategy
- **Descripción:** `Res` mantiene datos; un objeto `IEtapaVida` o similar encapsula reglas de edad y vacunación.
- **Pros:** flexibilidad si el número de “etapas” crece mucho.
- **Contras:** reescritura amplia; más tipos; no hay SC actual que lo exija; riesgo de regresión en comportamiento observable.
- **Resultado:** **Descartada** en esta iteración.

### Alternativa B — Conservar herencia y documentar LSP
- **Descripción:** mantener jerarquías; justificar por qué no composición; publicar verificación de sustituibilidad.
- **Pros:** preserva modelo mental del dominio; bajo riesgo; alineado al código que ya funciona.
- **Contras:** algunos `switch` por tipo en políticas de vacunación/venta (acotados).
- **Resultado:** **Elegida**.

### Alternativa C — Aplanar a una sola clase `Res` con enum de tipo
- **Descripción:** eliminar subclases; discriminar con `TipoRes`.
- **Pros:** menos tipos.
- **Contras:** concentra validaciones de edad en un solo lugar con condicionales; empeora SRP local; pierde polimorfismo natural.
- **Resultado:** **Descartada**.

---

## 3. Decisión

Se **conservan** las jerarquías `Res` y `Vacuna` tal como están en `Hacienda.Domain/Entities`.

En el UML de notación extendida aparecen en **negro/gris** (elementos conservados del AS-IS).

Si en el futuro el número de tipos crece o las SC de vacunación se vuelven muy divergentes, se podrá introducir Strategy **solo** en políticas (`IResPolicy` / reglas de vacunación) **sin** borrar la jerarquía de identidad.

---

## 4. Costo / consecuencia negativa aceptada

- Persistencia de algunos `switch`/`is` por tipo en Application al etiquetar ventas o aplicar topes de vacunas.
- Quien espere un rediseño total hacia composición puede interpretar la decisión como “conservadurismo”; se mitiga con este ADR y la verificación LSP publicada.

---

## 5. Principios involucrados y ganancia

| Principio | Cómo se aplica |
|-----------|----------------|
| **LSP** | Subtipos verificados como sustituibles; no se fuerza un reemplazo injustificado |
| **OCP** | Se evita abrir el diseño a una refactorización masiva no motivada por SC |

**Ganancia:** se invierte el esfuerzo de diseño en DIP/SRP/ISP (donde el inventario sí mostraba dolor), no en reescribir un modelo de herencia que **sí** pasa LSP.
