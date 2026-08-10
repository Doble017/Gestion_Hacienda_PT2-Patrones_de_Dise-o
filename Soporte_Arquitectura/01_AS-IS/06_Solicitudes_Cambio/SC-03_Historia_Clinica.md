# SC-03 — Historia clínica de cada res

**Código:** SC-3  
**Descripción:** Además de las vacunas, se va a requerir tener la historia clínica de cada res en un futuro (diagnósticos, tratamientos, observaciones, intervenciones, etc.).

---

## 1. Situación actual (AS-IS)

Actualmente solo se registran vacunas aplicadas (`List<Vacuna>` dentro de `Res`). La lógica de aplicación de vacunas y los límites por tipo de res están concentrados en el método `aplicar_vacuna` de la clase `Hacienda`, una de las reglas de negocio más complejas del sistema.

---

## 2. Métricas de impacto sobre el código actual

| Concepto                    | Cantidad estimada |
|----------------------------|-------------------|
| Clases a modificar         | 7 – 9             |
| Archivos a modificar       | 9 – 11            |
| Clases nuevas mínimas      | 2 – 4             |

---

## 3. Archivos / clases que habría que modificar hoy

| Capa              | Archivos impactados                                                                 |
|-------------------|-------------------------------------------------------------------------------------|
| Dominio           | `Res.cs`, `Hacienda.cs` (método `aplicar_vacuna` + contadores), `Vacuna.cs`, `Bacteriana.cs`, `Viva.cs` |
| Eventos           | `PublisherVacunacionCompletada.cs` (posible extensión)                              |
| Persistencia      | `PersistenciaService.cs` (Guardar/Cargar vacunas aplicadas → generalizar)           |
| Validaciones      | `ValidarVacuna.cs` + nuevos validadores de registros clínicos                       |
| Servicios         | `VacunaService.cs`, `ResService.cs`                                                 |
| Controllers / UI  | `VacunaController.cs`, `ResController.cs` + vistas                                  |

---

## 4. Comportamiento existente en riesgo de romperse

- Lógica de aplicación de vacunas y límites por tipo de res (`aplicar_vacuna` en `Hacienda`).
- Contadores de vacunas bacterianas y vivas por animal.
- Eventos de vacunación completada y vacuna vencida.
- Persistencia de vacunas aplicadas.
- Cualquier pantalla o reporte que muestre el esquema de vacunación actual.

---

## 5. Severidad de impacto

**ALTA** — toca la lógica de negocio más compleja existente (límites de vacunas por tipo de res).

---

## 6. Relación con hallazgos de Fase 1

Este cambio presiona fuertemente los hallazgos **H-01, H-02 y H-03** (dependencias de eventos, ISP y God Class), y evidencia la necesidad de aplicar OCP sobre el registro sanitario del animal.