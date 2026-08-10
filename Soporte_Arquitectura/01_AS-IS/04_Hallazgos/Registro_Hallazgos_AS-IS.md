# Registro de hallazgos — Diagnóstico AS-IS

**Sistema:** Bib_Hacienda + p_mvcHacienda  
**Fase:** 1 — Diagnóstico del sistema actual  
**Roles:** Arquitecto de dependencias · Integrador y evidencia · Arquitecto de dominio  

---

## 1. Criterio del inventario

Cada hallazgo se formula con ubicación en el código, síntoma, principio SOLID comprometido, impacto y severidad. Se distinguen:

- **Propio:** identificado por lectura manual del código del equipo.  
- **Consolidado:** resultado de contrastar el análisis del equipo con revisiones complementarias del mismo código.  
- **Refutado:** propuesta asistida por herramienta o patrón genérico que el equipo evaluó y rechazó con base en el código.

---

## 2. Inventario de hallazgos

| ID | Ubicación | Síntoma observado | Principio | Impacto | Severidad | Origen |
|----|-----------|-------------------|-----------|---------|-----------|--------|
| **H-01** | `Hacienda.cs` (p. ej. instanciación y uso de publishers en flujos de alimentación y vacunación) | La clase de dominio instancia publishers concretos (`PublisherPesoMin`, etc.) y gestiona suscripción de eventos dentro de métodos de negocio. | DIP | Cambiar el canal de notificación obliga a modificar y recompilar el núcleo del dominio. | Alta | Propio |
| **H-02** | `Hacienda.cs` (declaración de la clase) | `Hacienda` implementa a la vez `IVacunacion`, `IVentaRes` e `ICreacionVacuna`. | ISP | Quien solo vende o solo vacuna depende de toda la superficie de la clase; dificulta evolución independiente (SC-1, SC-3). | Alta | Propio |
| **H-03** | `Hacienda.cs` (clase completa, ~558 líneas); `Program.cs` (registro Singleton) | Concentra potreros, reses, venta, alimentación, vacunas, eventos y búsquedas. Se registra como Singleton concreto; los Services dependen de la clase concreta. | SRP, DIP | Las tres SC futuras impactan esta clase. Máximo costo de cambio y riesgo de regresión. | Alta | Propio |
| **H-04** | `Potrero.cs` (alta de reses y eventos) | `Potrero` también instancia publishers concretos y replica lógica de suscripción en `anadir_res`. | DIP, SRP | El mecanismo de notificación está duplicado en dos clases de dominio. | Media | Propio |
| **H-05** | `PersistenciaService.cs` (~643 líneas) | Mezcla lectura/escritura de archivos, construcción de proxies Castle, validadores concretos y, en el diseño original, conocimiento de contexto web. | SRP, DIP | Cambiar medio de persistencia o validación obliga a tocar una clase híbrida de gran tamaño. | Media-Alta | Propio / Consolidado |
| **H-06** | Jerarquía `Res` → `Ternero` / `Cebon` / `Novillo` | Sugerencia asistida frecuente: eliminar la herencia y sustituirla por composición + Strategy. | Ninguno (refutado) | La jerarquía satisface LSP (restricción de edad en construcción). Eliminarla añade complejidad sin beneficio claro para las SC. | Baja | Refutado |
| **H-07** | `Hacienda.aplicar_vacuna` / decisiones por tipo | Uso de `is Ternero` / `is Cebon` / `is Novillo` y análogos sobre `Vacuna` para aplicar reglas. | OCP | Incorporar un nuevo tipo de res o vacuna tiende a modificar métodos existentes en lugar de extender por polimorfismo. | Media | Consolidado |
| **H-08** | `Potrero.anadir_res` | Creación del subtipo de `Res` mediante `switch` / decisión por tipo de potrero. | OCP, SRP | Un nuevo tipo de potrero o res obliga a modificar el método de alta. | Media | Consolidado |
| **H-09** | `IValidarInformacion` e implementaciones | Una interfaz obliga a validar varias entidades; implementaciones parciales con operaciones no usadas. | ISP | Clientes e implementadores dependen de una superficie mayor a la necesaria. | Media | Consolidado |
| **H-10** | Listas públicas en `Hacienda` y `Potrero` (`L_potreros`, `L_reses`, `L_vacunas`, etc.) | Colecciones internas expuestas con acceso amplio desde fuera del agregado. | Encapsulamiento / SRP | Riesgo de romper invariantes al manipular estado desde capas superiores. | Media | Consolidado |
| **H-11** | Controladores MVC (`PotreroController`, `ResController`, `VacunaController`, etc.) | Mezclan validación de entrada, orquestación de dominio y, en el AS-IS, cercanía a persistencia concreta. | SRP, DIP | Baja testabilidad y acoplamiento de la presentación al detalle de implementación. | Media | Consolidado |
| **H-12** | `Program.cs` + consumo en Services/Controllers | Registro y consumo predominante de clases concretas (`Hacienda`, `PersistenciaService`) sin puertos de repositorio. | DIP | Impide sustituir implementaciones (pruebas, otro almacenamiento) sin modificar consumidores. | Alta | Consolidado |

---

## 3. Hallazgos propios (lectura manual del equipo)

Los siguientes se obtuvieron por inspección directa del código fuente, con trazabilidad a clases y responsabilidades observables:

- **H-01** — Publishers concretos en el dominio (`Hacienda`).  
- **H-02** — Múltiples interfaces implementadas por una sola clase gorda.  
- **H-03** — God Class + Singleton concreto compartido por la capa de aplicación.  
- **H-04** — Misma dependencia de publishers en `Potrero`.  
- **H-05** — `PersistenciaService` monolítico (persistencia + validación + infraestructura).  

Cumplen el requisito de al menos tres hallazgos propios.

---

## 4. Hallazgo refutado

### H-06 — Eliminar la herencia `Res` → `Ternero` / `Cebon` / `Novillo`

**Propuesta asistida:** sustituir la jerarquía por composición y estrategias, bajo el argumento genérico de que la herencia es indeseable.

**Verificación del equipo:**

- Las subclases refuerzan precondiciones de construcción (rangos de edad).  
- No se observó debilitamiento de postcondiciones ni ruptura de invariantes del tipo `Res` en el uso actual.  
- Las solicitudes de cambio previstas no exigen reemplazar este modelo por composición.

**Decisión:** se rechaza la eliminación de la jerarquía. Se conserva en el TO-BE y se documenta la verificación LSP en el diseño correspondiente.

---

## 5. Síntesis por principio SOLID

| Principio | Hallazgos relacionados | Lectura breve |
|---------|------------------------|---------------|
| **SRP** | H-03, H-04, H-05, H-08, H-10, H-11 | Responsabilidades concentradas en `Hacienda`, `Potrero`, persistencia y controladores. |
| **OCP** | H-07, H-08 | Extensiones de tipos tienden a modificar métodos centrales. |
| **LSP** | H-06 (refutado); riesgo residual en H-07 | La jerarquía de `Res` se mantiene; el riesgo está en el código cliente que discrimina tipos. |
| **ISP** | H-02, H-09 | Interfaces anchas o varias interfaces concentradas en un solo implementador. |
| **DIP** | H-01, H-03, H-04, H-05, H-12 | Dominio y aplicación dependen de concretos de infraestructura y de `Hacienda`. |

---

## 6. Relación con los puntos de dolor priorizados

| Punto de dolor | Hallazgos que lo sustentan |
|----------------|----------------------------|
| #1 God Class `Hacienda` + falta de inversión de dependencias | H-01, H-02, H-03, H-12 |
| #2 Interfaces de aplicación mal segregadas | H-02, H-09 |
| #3 Capa de aplicación acoplada a implementación concreta | H-03, H-05, H-11, H-12 |

El detalle de priorización y argumentación de los tres puntos de dolor se presenta en el documento `Puntos_de_Dolor.md`.

---

## 7. Conclusión del inventario

El AS-IS es funcional, pero el costo de cambio se concentra en un orquestador de dominio (`Hacienda`), en dependencias concretas de eventos y persistencia, y en interfaces que no separan capacidades. El diseño TO-BE y los ADR asociados responden de forma directa a H-01 a H-05 y H-12, conservan la jerarquía validada en H-06 y reducen la presión de H-07 y H-08 mediante fronteras de aplicación y dominio más claras.
