# Documentación TO-BE — Sistema Hacienda

Cumple los requisitos de **Diseño de la nueva arquitectura (TO-BE)** del reto:

1. UML en notación extendida con convención de color de clase.
2. SOLID argumentado (qué se partió, frontera, ganancia).
3. Herencia justificada + verificación LSP.
4. DIP con alto nivel, bajo nivel, abstracción y Composition Root.
5. **Mínimo 5 ADR** con alternativas descartadas, costo aceptado y principios.

## Convención de color (UML principal)

| Color | Significado |
|-------|-------------|
| **Negro / gris** | Conservado del diseño AS-IS |
| **Verde** | SRP — partición de responsabilidades |
| **Morado** | ISP + DIP — abstracciones / puertos nuevos |
| **Naranja** | DIP — adaptadores de bajo nivel (Infrastructure) |
| **Azul** | Presentation desacoplada + Composition Root |

## Estructura de carpetas

| Carpeta | Contenido |
|---------|-----------|
| **01_Diseno_Dominio** | Modelo de dominio, capas, matriz de relaciones |
| **02_Diagramas** | UML (empezar por `00_UML_TO-BE_Notacion_Extendida.drawio`) |
| **03_SOLID** | Argumentación de los 5 principios |
| **04_LSP** | Verificación LSP Res / Vacuna |
| **05_DIP** | Tabla maestra de inversiones |
| **06_ADR** | **ADR-01 … ADR-05** (+ índice) |
| **07_Trazabilidad** | Mapa AS-IS → TO-BE |

## ADR — checklist del enunciado

Cada ADR incluye:

- Contexto y evidencia (referencia a hallazgo H-0x)
- ≥ 2 alternativas evaluadas (al menos una **descartada**)
- Decisión tomada
- Costo / consecuencia negativa aceptada
- Principio(s) involucrado(s)

| ADR | Decisión |
|-----|----------|
| 01 | Partir God Class `Hacienda` |
| 02 | Puertos de persistencia por agregado |
| 03 | Segregar interfaces de aplicación |
| 04 | Extraer publishers a adaptadores |
| 05 | Conservar herencia Res/Vacuna (LSP) |

## Orden recomendado de lectura / defensa

1. `02_Diagramas/00_UML_TO-BE_Notacion_Extendida.drawio`
2. `03_SOLID/01_Aplicacion_SOLID_TO-BE.md`
3. `04_LSP` + `05_DIP`
4. `06_ADR` (uno por pregunta estructural del evaluador)
