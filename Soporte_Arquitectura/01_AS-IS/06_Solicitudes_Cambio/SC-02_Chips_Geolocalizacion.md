# SC-02 — Chips de geolocalización en las reses

**Código:** SC-2  
**Descripción:** La hacienda tiene la necesidad de conectar a las reses chips para la geolocalización.

---

## 1. Situación actual (AS-IS)

Cada `Res` no tiene ningún atributo ni relación con un dispositivo de geolocalización. La entidad central del dominio (`Res` y sus subclases) y todos los flujos de alta, búsqueda y persistencia se verían afectados.

---

## 2. Métricas de impacto sobre el código actual

| Concepto                    | Cantidad estimada |
|----------------------------|-------------------|
| Clases a modificar         | 6 – 8             |
| Archivos a modificar       | 8 – 10            |
| Clases nuevas mínimas      | 1 – 2             |

---

## 3. Archivos / clases que habría que modificar hoy

| Capa              | Archivos impactados                                                                 |
|-------------------|-------------------------------------------------------------------------------------|
| Dominio           | `Res.cs` (nuevo atributo + lógica), posiblemente `Ternero.cs`, `Cebon.cs`, `Novillo.cs`, `Hacienda.cs`, `Potrero.cs` |
| Persistencia      | `PersistenciaService.cs` (serialización de reses + nuevo campo del chip)            |
| Validaciones      | `ValidarRes.cs`                                                                     |
| Servicios         | `ResService.cs`                                                                     |
| Controllers / UI  | `ResController.cs` + vistas de reses                                                |
| Composition Root  | `Program.cs` (carga inicial de datos)                                               |

---

## 4. Comportamiento existente en riesgo de romperse

- Alta de reses (`Potrero.anadir_res` y `Hacienda.anadir_res_potrero`).
- Búsqueda de reses por nombre.
- Persistencia y carga de reses desde archivos planos.
- Estadísticas de reses (`ResService.ObtenerEstadisticas`).
- Validaciones existentes de edad y peso.

---

## 5. Severidad de impacto

**MEDIA-ALTA** — impacta la entidad central `Res` y todos los flujos que la tocan.

---

## 6. Relación con hallazgos de Fase 1

Este cambio presiona los hallazgos **H-03 y H-05** (God Class y acoplamiento de persistencia), además de revelar la falta de extensibilidad en la entidad `Res`.