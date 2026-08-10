# SC-01 — Venta de productos derivados del ganado

**Código:** SC-1  
**Descripción:** La hacienda en el futuro va a comenzar a vender productos derivados del ganado como: lácteos, carne, piel.

---

## 1. Situación actual (AS-IS)

Hoy el sistema **solo vende reses vivas**. La clase `Venta` está acoplada directamente a `Res` + `Potrero`. No existe el concepto de producto derivado ni de ítem vendible genérico.

---

## 2. Métricas de impacto sobre el código actual

| Concepto                    | Cantidad estimada |
|----------------------------|-------------------|
| Clases a modificar         | 8 – 10            |
| Archivos a modificar       | 10 – 12           |
| Clases nuevas mínimas      | 3 – 5             |

---

## 3. Archivos / clases que habría que modificar hoy

| Capa              | Archivos impactados                                                                 |
|-------------------|-------------------------------------------------------------------------------------|
| Dominio           | `Venta.cs`, `Hacienda.cs` (método `vender_res` + lista `l_ventas`), posible nueva jerarquía de productos |
| Interfaces        | `IVentaRes.cs` (ampliar o crear nueva interfaz segregada)                           |
| Persistencia      | `PersistenciaService.cs` (nuevos métodos Guardar/Cargar + nuevo archivo plano)      |
| Validaciones      | `ValidarVenta.cs` + posible nuevo validador de productos                            |
| Servicios         | `VentaService.cs`                                                                   |
| Controllers / UI  | `VentaController.cs` + nuevas vistas                                                |
| Composition Root  | `Program.cs`                                                                        |

---

## 4. Comportamiento existente en riesgo de romperse

- Lógica actual de `vender_res` (remover res del potrero + crear objeto `Venta`).
- Cálculos y estadísticas de ventas (`VentaService.ObtenerEstadisticas`).
- Persistencia de ventas en archivo plano.
- Cualquier reporte o listado que asuma que toda venta tiene obligatoriamente una `Res` asociada.

---

## 5. Severidad de impacto

**ALTA** — toca el núcleo del modelo de venta.

---

## 6. Relación con hallazgos de Fase 1

Este cambio presiona directamente los hallazgos **H-01, H-02 y H-03** (God Class `Hacienda`, violación de ISP y falta de inversión de dependencias).