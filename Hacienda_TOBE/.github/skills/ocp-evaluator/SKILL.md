---
name: ocp-evaluator
user-invocable: true
description: "Evalúa clases para verificar el cumplimiento del Principio de Abierto/Cerrado (OCP) de SOLID y recomienda estrategias de extensión sin modificación."
---

# Habilidad de Evaluación OCP

Utiliza esta habilidad para evaluar si las clases están diseñadas para ser abiertas a la extensión pero cerradas a la modificación, y para generar refactorizaciones que promuevan esta práctica.

## Lo que evalúa esta habilidad
- Clases que requieren modificaciones internas para agregar nueva funcionalidad
- Uso excesivo de condicionales (if/else, switch) para manejar variaciones de comportamiento
- Dependencias directas en implementaciones concretas en lugar de abstracciones
- Métodos que cambian frecuentemente por nuevos requisitos
- Dificultad para extender el comportamiento sin tocar el código existente
- Violación del patrón Strategy o Template Method
- Acoplamiento a tipos específicos que limitan la extensibilidad

## Protocolo de evaluación
1. La clase debe poder extenderse sin modificar su código fuente.
2. Las nuevas funcionalidades deben agregarse mediante herencia, composición o inyección de dependencias.
3. Las abstracciones (interfaces/clases abstractas) deben ser estables y no cambiar frecuentemente.
4. Los condicionales que cambian según el tipo deben reemplazarse con polimorfismo.
5. Los métodos deben aceptar parámetros de tipos abstractos, no concretos.
6. Las dependencias deben orientarse a interfaces, no a implementaciones.
7. Los cambios en requisitos deben requerir agregar nuevas clases, no modificar las existentes.
8. Proporcionar estrategias concretas para hacer el código extensible.

## Uso recomendado
- Analizar clases Java para verificar el cumplimiento de OCP de SOLID.
- Identificar puntos de extensión y sugerir abstracciones.
- Recomendar patrones de diseño como Strategy, Template Method, Factory Method o Decorator.

> Ejemplo: "Evalúa esta clase para OCP y sugiere cómo hacerla extensible para nuevos tipos de [funcionalidad] sin modificar su código."

## Formato de respuesta sugerido

Al analizar una clase, estructura tu respuesta de la siguiente manera:

### 🔍 ANÁLISIS DE OCP

**Clase evaluada:** [Nombre de la clase]

**Estado general:** ✅ Cumple OCP / ⚠️ Violaciones menores / ❌ Violaciones graves

---

### 📋 VIOLACIONES ENCONTRADAS

[Lista numerada de cada violación encontrada, indicando:]
- **Violación #1:** [Descripción clara]
  - **Ubicación:** [Método/línea específica]
  - **Tipo de violación:** [Modificación requerida / Condicionales / Dependencia concreta]
  - **Razón:** [Por qué esto viola OCP]
  - **Impacto:** [Qué problemas causa al agregar nuevas funcionalidades]

---

### 🛠️ RECOMENDACIONES DE REFACTORIZACIÓN

**Opción 1: [Patrón de diseño sugerido]**
- **Abstracción propuesta:** [Interfaz/Clase abstracta a crear]
- **Implementaciones concretas:** [Clases que implementarán la abstracción]
- **Cómo extender:** [Pasos para agregar nueva funcionalidad]

