# Decisión de solicitud de cambio a implementar

**Proyecto:** Sistema de Gestión de Hacienda  
**Fase:** Tras el diseño TO-BE e implementación de la arquitectura objetivo  

---

## Solicitud seleccionada

**SC-2 — Chips de geolocalización en las reses**

---

## Alternativas consideradas

| Solicitud | Evaluación breve |
|-----------|------------------|
| SC-1 Productos derivados | Demuestra bien OCP sobre ventas, pero exige rediseñar el núcleo de `Venta` y ampliar persistencia y UI de forma más amplia. |
| SC-2 Chips de geolocalización | Extiende el agregado `Res` y su persistencia con un cambio localizable. |
| SC-3 Historia clínica | Valiosa para OCP sanitario, pero interviene la lógica más compleja del sistema (`aplicar_vacuna`) y eleva el riesgo de regresión. |

---

## Criterios de decisión

1. **Superficie de cambio:** menor número estimado de clases y archivos a modificar en comparación con SC-1 y SC-3.  
2. **Riesgo sobre comportamiento existente:** no reescribe el modelo de ventas ni la política de límites de vacunación.  
3. **Capacidad de demostrar mejora arquitectónica:** en TO-BE el cambio puede acotarse al agregado `Res`, su repositorio y la presentación, evidenciando que el costo dejó de propagarse por un orquestador global.  
4. **Viabilidad en el plazo del reto:** permite completar implementación, métricas antes/después y evidencia sin comprometer la estabilización del resto del sistema.

---

## Decisión

Se implementa **SC-2** sobre la arquitectura TO-BE.

Las solicitudes SC-1 y SC-3 quedan documentadas como línea base de impacto sobre el AS-IS y como escenarios de extensión futura habilitados por el rediseño (abstracciones de venta y registro sanitario), sin implementarse en esta entrega.

---

## Métricas que se reportarán tras la implementación

| Métrica | Valor en AS-IS (línea base) | Valor en TO-BE (tras SC-2) |
|---------|-----------------------------|----------------------------|
| Clases modificadas | 6 – 8 | *A completar con conteo real* |
| Archivos modificados | 8 – 10 | *A completar con conteo real* |
| Clases nuevas | 1 – 2 | *A completar con conteo real* |
| ¿Cambio mayormente aditivo? | No | *Sí / No según evidencia* |