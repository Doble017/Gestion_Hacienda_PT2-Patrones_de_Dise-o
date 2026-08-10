# Matriz comparativa de impacto — Solicitudes de cambio (AS-IS)

| Métrica | SC-1 Productos derivados | SC-2 Chips de geolocalización | SC-3 Historia clínica |
|---------|--------------------------|-------------------------------|------------------------|
| Clases a modificar | 8 – 10 | 6 – 8 | 7 – 9 |
| Archivos a modificar | 10 – 12 | 8 – 10 | 9 – 11 |
| Clases nuevas estimadas | 3 – 5 | 1 – 2 | 2 – 4 |
| Severidad | Alta | Media-Alta | Alta |
| Interviene `Hacienda` | Sí | Sí | Sí |
| Interviene `PersistenciaService` | Sí | Sí | Sí |
| Interviene modelo de `Venta` | Sí (núcleo) | No | No |
| Interviene entidad `Res` | Indirecto | Sí (núcleo) | Sí |
| Interviene `aplicar_vacuna` | No | No | Sí (núcleo) |
| Riesgo sobre comportamiento actual | Alto | Medio-Alto | Alto |
| Hallazgos del diagnóstico asociados | H-01, H-02, H-03 | H-03, H-05 | H-01, H-02, H-03 |

---

## Lectura consolidada

Las tres solicitudes resultan costosas en el AS-IS porque el impacto no queda localizado en un único módulo: atraviesa la clase `Hacienda`, la persistencia monolítica y, según el caso, el modelo de ventas o el de vacunación.

SC-1 y SC-3 presentan severidad alta. SC-2 presenta severidad media-alta, con superficie de cambio más acotada al agregado de reses y su persistencia.