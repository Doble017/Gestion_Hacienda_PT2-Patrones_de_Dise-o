# Implementación de solicitud de cambio SC-2 y métricas comparativas

**Proyecto:** Sistema de Gestión de Hacienda  
**Fase:** 4 — Implementación  
**Solicitud implementada:** SC-2 — Chips de geolocalización en las reses  

---

## 1. Criterio de selección

| Criterio | SC-2 Chips | SC-3 Historia clínica |
|----------|------------|------------------------|
| Riesgo sobre reglas complejas | Bajo | Alto (límites de vacunación) |
| Demuestra OCP con claridad | Sí | Sí, pero más invasivo |
| Encaje con el diseño TO-BE | Extiende `Res` y un repositorio | Atraviesa el flujo de vacunación de punta a punta |

**Decisión:** se implementó **SC-2**. Es la prueba más directa y acotada de que el principio abierto/cerrado quedó aplicado: el cambio es mayormente aditivo y no obliga a reabrir el núcleo de ventas ni la política completa de vacunas.

Las solicitudes SC-1 y SC-3 permanecen documentadas como línea base de impacto sobre el AS-IS y como escenarios de extensión futura.

---

## 2. Alcance implementado en TO-BE

| Acción | Elemento |
|--------|----------|
| **Nuevo** | `ChipGeolocalizacion` y `EstadoChip` |
| **Modificado (mínimo)** | `Res` (propiedad de chip y operaciones de asignación/retiro) |
| **Modificado** | `FilePotreroRepository` (columnas opcionales de persistencia del chip) |
| **Modificado** | `IResAppService` / `ResAppService` (`AsignarChipAsync`, `QuitarChipAsync`) |
| **Modificado** | `ResController` y vistas de reses (columna y modal de chip) |

**No se modificaron:** vacunación, ventas, usuarios, Composition Root, ni un orquestador tipo `Hacienda` (ya eliminado en el TO-BE).

---

## 3. Métrica comparativa (prueba empírica de OCP)

| Métrica | AS-IS (línea base Fase 2) | TO-BE (implementación real) |
|---------|---------------------------|------------------------------|
| Clases modificadas | 6 – 8 | 4 |
| Archivos modificados | 8 – 10 | 5 |
| Clases nuevas | 1 – 2 | 2 |
| ¿Interviene `Hacienda`? | Sí | No |
| ¿Interviene `PersistenciaService` monolítico? | Sí | No |
| Riesgo sobre vender / vacunar | Posible | Nulo |
| ¿Cambio mayormente aditivo? | No | Sí |

### Lectura de la métrica

En el AS-IS, incorporar el chip obligaba a tocar la entidad central, la cascada de alta en `Hacienda` / `Potrero`, la persistencia monolítica y la capa MVC.  

En el TO-BE el cambio se limitó a un tipo nuevo, una extensión mínima de `Res`, un repositorio de archivos y el flujo de aplicación/presentación de reses. Esa reducción de clases y archivos modificados, junto con la creación de tipos nuevos sin reescribir el núcleo, constituye la evidencia empírica de OCP.

---

## 4. Verificación funcional del chip

```bash
cd Hacienda_TOBE/Hacienda.Web
dotnet run
```

Credenciales de prueba: `santi` / `santi11`.

Pasos observados:

1. Listar o detallar reses.  
2. Asignar chip a una res (modal o flujo expuesto en la UI).  
3. Comprobar persistencia tras recargar (dato presente en la vista / archivo de datos).  
4. Quitar chip y verificar el estado actualizado.

---

## 5. Relación con caracterización

Los casos C-01 a C-08 documentados en `Caracterizacion/Casos_Caracterizacion.md` verifican la preservación del comportamiento base. La presente SC se evalúa de forma adicional como extensión controlada sobre el TO-BE, sin invalidar esos escenarios.
