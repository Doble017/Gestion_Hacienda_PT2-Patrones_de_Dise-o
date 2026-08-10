# ADR-01 — Partición de la God Class `Hacienda`

| Campo | Valor |
|-------|-------|
| **Estado** | Aceptado |
| **Fecha** | Agosto 2026 |
| **Principios involucrados** | SRP, ISP, DIP |
| **Hallazgo(s) del inventario** | **H-02** (tres interfaces de capacidad implementadas por una sola clase), **H-03** (God Class registrada como Singleton concreto y consumida por todos los services) |

---

## 1. Contexto y evidencia

En el AS-IS, `Hacienda.cs` (~558 líneas) concentraba:

- creación y consulta de potreros,
- alta, búsqueda y alimentación de reses,
- creación y aplicación de vacunas,
- registro de ventas,
- emisión de eventos de dominio.

Se registraba en el contenedor como **Singleton concreto**. `PotreroService`, `ResService`, `VacunaService` y `VentaService` dependían de esa instancia. Cualquier Solicitud de Cambio (SC) de vacunación o de ventas obligaba a tocar casi el mismo archivo y a revalidar comportamientos colaterales.

Evidencia: inventario de hallazgos H-02 y H-03; inspección de constructores de services AS-IS y de `Program.cs` / registro DI original.

---

## 2. Alternativas evaluadas

### Alternativa A — Mantener `Hacienda` y extraer helpers privados
- **Descripción:** dejar la clase pública y mover bloques a métodos o clases internas `partial`.
- **Pros:** cambio mínimo de superficie.
- **Contras:** SRP sigue violado; los clientes siguen acoplados al mismo tipo concreto; las SC siguen concentradas en un solo artefacto.
- **Resultado:** **Descartada** (no resuelve H-02/H-03).

### Alternativa B — Partir por capacidad de negocio en Application Services + repositorios por agregado
- **Descripción:** eliminar el orquestador global; un App Service por capacidad (potrero, res, vacunación, venta, usuario) y un repositorio por agregado de persistencia.
- **Pros:** frontera alineada a módulos de UI y a SC; permite ISP y DIP de forma natural.
- **Contras:** más tipos; hace falta un Composition Root explícito.
- **Resultado:** **Elegida**.

### Alternativa C — Microservicios por capacidad
- **Descripción:** desplegar vacunación, ventas, etc. como procesos separados.
- **Pros:** aislamiento de despliegue.
- **Contras:** sobre-ingeniería para el tamaño del sistema, el medio de persistencia en archivos y el alcance del reto de ingreso.
- **Resultado:** **Descartada**.

---

## 3. Decisión

Se elimina `Hacienda` como orquestador global del dominio.

Capacidades de aplicación:

- `PotreroAppService`
- `ResAppService`
- `VacunacionAppService`
- `VentaAppService`
- `UsuarioAppService`

Persistencia por agregado (detalle en ADR-02):

- `IPotreroRepository`, `IVacunaRepository`, `IVentaRepository`, `IUsuarioRepository`

---

## 4. Costo / consecuencia negativa aceptada

- Mayor número de tipos y registros en el contenedor DI.
- Curva de aprendizaje inicial para quien conocía solo el AS-IS monolítico.
- Necesidad de coordinar transacciones lógicas entre servicios cuando un caso de uso toca más de un agregado (p. ej. venta = actualizar potrero + escribir venta).

---

## 5. Principios involucrados y ganancia

| Principio | Cómo se aplica |
|-----------|----------------|
| **SRP** | Una capacidad de negocio por App Service |
| **ISP** | Cada cliente depende solo del contrato de su capacidad (ADR-03) |
| **DIP** | Los services ya no dependen de un concreto global |

**Ganancia:** una SC de vacunas no obliga a modificar ni recompilar el módulo de ventas ni el de autenticación.
