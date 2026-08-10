# Verificación LSP — Jerarquías Res y Vacuna (ADR-05)

## 1. Jerarquías conservadas

```
Res «abstract»
 ├── Ternero
 ├── Cebon
 └── Novillo

Vacuna «abstract»
 ├── Bacteriana
 └── Viva
```

Fuente: `Hacienda.Domain/Entities/Res.cs`, `Vacuna.cs`.

---

## 2. Contrato del tipo base `Res`

| Aspecto | Contrato |
|---------|----------|
| Precondiciones de construcción | `nombre` no vacío |
| Estado observable | `Nombre`, `Peso`, `Edad`, `VacunasAplicadas` (solo lectura) |
| Operaciones | `Alimentar`, `RegistrarVacuna`, `CargarVacunasAplicadas` |
| Postcondiciones | `Alimentar` incrementa `Peso`; `RegistrarVacuna` agrega a la lista |
| Excepciones base | `ArgumentException` / `ArgumentNullException` en validaciones de entrada |

---

## 3. Subtipos de `Res`

| Subtipo | Precondición adicional (edad) | ¿Debilita postcondiciones del base? | ¿Introduce excepciones en ops base? |
|---------|-------------------------------|-------------------------------------|-------------------------------------|
| `Ternero` | edad ≤ 12 | No | No |
| `Cebon` | 13 ≤ edad ≤ 48 | No | No |
| `Novillo` | edad > 48 | No | No |

Las restricciones de edad se aplican **solo en construcción** (setter de `Edad` en el constructor). Una vez construido el objeto, `Alimentar` y `RegistrarVacuna` se comportan como en el tipo base.

**Conclusión LSP:** un `Ternero` puede usarse donde el código espera `Res` (listados, vacunación, venta, persistencia) sin romper invariantes del cliente.

---

## 4. Contrato del tipo base `Vacuna`

| Aspecto | Contrato |
|---------|----------|
| Estado | Nombre, Lote, fechas |
| Operación | `EstaVencida(DateTime)` |
| Subtipos | `Bacteriana` añade `PeriodoAplicacion`; `Viva` añade `GradoAtenuacion` |

Los subtipos no alteran `EstaVencida` ni invalidan el uso polimórfico en listas `IReadOnlyList<Vacuna>` o en `RegistrarVacuna`.

---

## 5. Por qué no se reemplazó por composición + Strategy

| Alternativa | Costo | Beneficio para SC actuales |
|-------------|-------|----------------------------|
| Herencia actual | Bajo (ya existe, LSP OK) | Suficiente para SC-2/SC-3 acotadas al agregado |
| Composición + Strategy de “etapa de vida” | Más tipos, más indirección | No requerido por SC-1/2/3 de forma inmediata |

Decisión: **conservar herencia** (refutación H-06 / ADR-05). Si el número de tipos crece mucho, se puede introducir Strategy **solo** en políticas de vacunación sin borrar la jerarquía de identidad.

---

## 6. Evidencia de uso polimórfico en Application

- `ResAppService.ListarTodasAsync` retorna `Res` sin discriminar en la firma.
- `VacunacionAppService.AplicarVacunaAsync` recibe `Res` y aplica límites con `switch` de tipos solo para **leer** constantes de `ReglaVacuna` (no para romper el contrato del base).
- `VentaAppService` usa `res switch` únicamente para etiquetar el snapshot `TipoRes` en `Venta`.
