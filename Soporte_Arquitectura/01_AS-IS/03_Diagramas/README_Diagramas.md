# Diagramas UML AS-IS — Sistema de Gestión de Hacienda

Archivos generados a partir del **código fuente real** y del inventario documentado en `Soporte_Arquitectura`.

## Cómo abrirlos

1. Ve a [https://app.diagrams.net](https://app.diagrams.net) (draw.io)
2. **File → Open from → Device** y selecciona el `.drawio`
3. O instala la extensión **Draw.io Integration** en VS Code y ábrelos directamente

También puedes importarlos en Lucidchart, Visual Paradigm (import draw.io) o exportarlos a PNG/PDF/SVG desde diagrams.net.

## Diagrama oficial único (evaluar solo este)

| Archivo | Qué muestra | Uso |
|---------|-------------|-----|
| `UML_AS-IS_Dominio_Unico.drawio` + `UML_AS-IS_Dominio_Unico.md` | **Único diagrama oficial**: solo biblioteca `Bib_Hacienda` (Hacienda, Potrero, Res x3, Vacuna x2, Venta, Usuario, Reglas, Services en biblioteca). Sin front/infra. Asociaciones sin rombos. | Defensa y evaluación |
| `02_UML_AS-IS_Dominio.drawio` | Base fiel de la que deriva el único (conservar como respaldo) | Respaldo |

## Anexos históricos (no evaluar, generados con IA)

| Archivo | Estado |
|---------|--------|
| `01_UML_AS-IS_General.drawio` | Anexo: no es UML de clases, no evaluar |
| `03_UML_AS-IS_Validacion_Eventos_AOP.drawio` | Anexo: detalle transversal resumido en 1 línea del único |
| `04_UML_AS-IS_MVC_Servicios.drawio` | Anexo: front fuera de alcance (clase pedía solo biblioteca); Services pertenecen a biblioteca |
| `Diagrama_Dependencias_AS-IS.drawio` | Anexo |

## Criterios de fidelidad (rúbrica)

- Refleja lo que **está escrito** en el código, no un diseño idealizado.
- Generalizaciones: `Res ← Ternero|Cebon|Novillo`, `Vacuna ← Bacteriana|Viva`, `Validacion ← Validador*`.
- Realizaciones: `Hacienda` implementa `IVacunacion`, `IVentaRes`, `ICreacionVacuna`; `Autenticacion` implementa `IAutenticacion`; validadores implementan `IValidarInformacion`.
- Asociaciones (corrección: sin rombos de composición/agregación por falta de evidencia de ciclo de vida): `Hacienda`→Potrero/Venta/Vacuna; `Potrero`→Res; `Res`→vacunas aplicadas.
- Multiplicidad de reses por potrero acotada por `ReglaPotrero.max_reses_potrero = 150`.
- Dependencias de servicios hacia `Hacienda` y `PersistenciaService` son referencias concretas (punto de dolor para DIP).

## Convención de color (sugerida para el video)

| Color | Significado |
|-------|-------------|
| Amarillo | Entidades de dominio |
| Morado | Interfaces / contratos |
| Azul claro | Especializaciones / controllers |
| Rojo claro | Validación / AOP / composition root |
| Verde | Eventos / capa MVC de aplicación |
| Naranja | Reglas estáticas / dominio referenciado |
| Gris | Infraestructura externa (Castle, ASP.NET) |

## Próximos pasos sugeridos

1. Abrir cada diagrama en draw.io y ajustar posiciones si hace falta para la impresión/video.
2. Exportar PNG de alta resolución para el entregable `/01-diagnostico`.
3. Usar el diagrama de dominio como base para el inventario de hallazgos SOLID (SRP de Hacienda, dependencias concretas de servicios, jerarquía Res vs LSP, etc.).
