# ADR-02 — Puertos de persistencia por agregado

| Campo | Valor |
|-------|-------|
| **Estado** | Aceptado |
| **Fecha** | Agosto 2026 |
| **Principios involucrados** | DIP, SRP |
| **Hallazgo(s) del inventario** | **H-05** (`PersistenciaService` monolítico ~643 líneas: serialización a archivos, proxies Castle, validación y conocimiento de contexto web mezclados) |

---

## 1. Contexto y evidencia

El AS-IS centralizaba I/O en `PersistenciaService`, usado desde controllers y services. Ese tipo conocía rutas de archivo, formato de líneas, creación de proxies dinámicos y, en algunos flujos, detalles ajenos al almacenamiento puro. Cambiar el medio de persistencia (p. ej. de `.txt` a base de datos) implicaba reescribir un bloque enorme y arriesgar regresiones en todos los módulos.

Evidencia: hallazgo H-05; tamaño y responsabilidades de `PersistenciaService` en el inventario AS-IS.

---

## 2. Alternativas evaluadas

### Alternativa A — Mantener un único `PersistenciaService` inyectable
- **Descripción:** extraer la clase tal cual a Infrastructure y registrarla como singleton.
- **Pros:** migración rápida.
- **Contras:** SRP sigue violado; un cambio de formato de vacunas puede romper potreros; no hay puertos por agregado.
- **Resultado:** **Descartada**.

### Alternativa B — Repositorios por agregado detrás de interfaces de dominio (puertos)
- **Descripción:** `IPotreroRepository`, `IVacunaRepository`, `IVentaRepository`, `IUsuarioRepository` en Domain; implementaciones `File*` en Infrastructure.
- **Pros:** DIP real; SC de medio de almacenamiento localizadas; App Services dependen de abstracciones.
- **Contras:** más archivos; algo de código de serialización repetible.
- **Resultado:** **Elegida**.

### Alternativa C — Migrar de inmediato a EF Core + SQL
- **Descripción:** abandonar archivos planos en esta iteración.
- **Pros:** consultas más ricas a medio plazo.
- **Contras:** cambia el comportamiento observable y el medio de persistencia exigido por el reto; fuera de alcance.
- **Resultado:** **Descartada** (preservar medio archivo).

---

## 3. Decisión

- **Puertos (Domain):** `IPotreroRepository`, `IVacunaRepository`, `IVentaRepository`, `IUsuarioRepository`.
- **Adaptadores (Infrastructure):** `FilePotreroRepository`, `FileVacunaRepository`, `FileVentaRepository`, `FileUsuarioRepository` + `FileStoragePaths`.
- **Composition Root:** `Hacienda.Web/Program.cs` invoca `AddHaciendaInfrastructure(dataPath)` (`Hacienda.Infrastructure/DependencyInjection.cs`), único lugar que enlaza puerto ↔ adaptador.

---

## 4. Costo / consecuencia negativa aceptada

- Más tipos de repositorio que mantener.
- Duplicación parcial de lógica de lectura/escritura de archivos (mitigable con helpers compartidos en una iteración posterior).
- Transacciones entre agregados siguen siendo coordinación en Application, no una unidad de trabajo de infraestructura.

---

## 5. Principios involucrados y ganancia

| Principio | Cómo se aplica |
|-----------|----------------|
| **DIP** | Alto nivel (`*AppService`) depende de puertos; bajo nivel (`File*`) implementa |
| **SRP** | Un repositorio = un agregado de persistencia |

**Ganancia:** sustituir archivos por SQL implica un nuevo adaptador + un cambio de registro en el Composition Root, **sin** modificar App Services ni Controllers.
