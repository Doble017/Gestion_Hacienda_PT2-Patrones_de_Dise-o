---
name: lsp-evaluator
user-invocable: true
description: "Evalúa jerarquías de herencia para verificar el cumplimiento del Principio de Sustitución de Liskov (LSP) de SOLID y recomienda refactorizaciones para garantizar que las subclases sean sustituibles por sus clases base."
---

# Habilidad de Evaluación LSP

Utiliza esta habilidad para evaluar si las jerarquías de herencia respetan el Principio de Sustitución de Liskov, asegurando que las subclases puedan reemplazar a sus clases base sin alterar el comportamiento esperado del programa.

## Lo que evalúa esta habilidad
- Subclases que sobrescriben métodos y cambian su comportamiento esperado
- Métodos que lanzan excepciones no declaradas o no soportadas
- Precondiciones más estrictas en subclases (parámetros más restrictivos)
- Postcondiciones más débiles en subclases (resultados menos específicos)
- Invariantes de clase que se rompen en la herencia
- Relaciones "es-un" que no son verdaderas (herencia forzada)
- Uso de verificaciones de tipo para manejar tipos específicos
- Métodos que no tienen implementación significativa en subclases
- Propiedades o comportamientos que se debilitan en la jerarquía
- Violación del principio de diseño por contrato (Design by Contract)

## Protocolo de evaluación
1. Una subclase debe poder reemplazar a su superclase sin afectar el comportamiento correcto.
2. Las precondiciones no pueden ser más estrictas en la subclase.
3. Las postcondiciones no pueden ser más débiles en la subclase.
4. Los invariantes de la superclase deben mantenerse en la subclase.
5. La subclase no debe lanzar excepciones que la superclase no declare.
6. No debe haber métodos que no tengan sentido o sean inválidos en la subclase.
7. La herencia debe representar una verdadera relación "es-un".
8. Proporcionar alternativas como composición o interfaces cuando la herencia no es apropiada.

## Uso recomendado
- Analizar jerarquías de herencia en cualquier lenguaje de programación orientado a objetos.
- Identificar violaciones de LSP que causan bugs difíciles de detectar.
- Recomendar refactorizaciones para corregir problemas de herencia.

> Ejemplo: "Analiza esta jerarquía de clases y verifica si cumple con LSP. Identifica violaciones y sugiere correcciones."

## Formato de respuesta sugerido

Al analizar una jerarquía de clases, estructura tu respuesta de la siguiente manera:

### 🔍 ANÁLISIS DE LSP

**Jerarquía evaluada:** [Clase base → Subclase(s)]

**Lenguaje detectado:** [Java/C#/Python/PHP/C++/etc.]

**Estado general:** ✅ Cumple LSP / ⚠️ Violaciones menores / ❌ Violaciones graves

---

### 📋 VIOLACIONES ENCONTRADAS

[Lista numerada de cada violación encontrada, indicando:]
- **Violación #1:** [Descripción clara]
  - **Ubicación:** [Clase/Método específico]
  - **Tipo de violación:** [Precondición/Postcondición/Invariante/Excepción/Comportamiento]
  - **Razón:** [Por qué esto viola LSP]
  - **Consecuencia:** [Qué problemas causa en tiempo de ejecución]
  - **Ejemplo de fallo:** [Describe un escenario que demostraría el problema sin código específico]

---

### 🛠️ RECOMENDACIONES DE REFACTORIZACIÓN

**Opción 1: Rediseño de jerarquía**
- **Problema raíz:** [Qué causa la violación]
- **Solución propuesta:** [Cómo rediseñar la herencia]
- **Concepto:** [Explicación de la solución de forma conceptual]

**Opción 2: Composición en lugar de herencia**
- **Cuándo usar:** [Situaciones donde la herencia no es apropiada]
- **Cómo implementar:** [Pasos para cambiar a composición]

**Opción 3: Extraer interfaces**
- **Cuándo usar:** [Cuando las subclases no comparten todo el comportamiento de la base]
- **Cómo implementar:** [Pasos para segregar responsabilidades]


---

### 🚨 SEÑALES DE ALERTA LSP

- [ ] Verificaciones de tipo para manejar casos específicos
- [ ] Métodos que lanzan excepciones de operación no soportada
- [ ] Métodos sobrescritos que no hacen nada (vacíos)
- [ ] Conversiones de tipo descendentes en el código cliente
- [ ] Subclases que eliminan funcionalidad de la superclase
- [ ] Nombres de clases que NO reflejan una verdadera relación "es-un"
- [ ] Métodos getter/setter que cambian comportamiento en subclases
- [ ] Subclases que no pueden ser usadas donde se espera la superclase

---

### 📝 PLAN DE MIGRACIÓN

1. **Paso 1:** [Identificar todas las violaciones de LSP en la jerarquía]
2. **Paso 2:** [Evaluar si la herencia es realmente apropiada o se necesita composición]
3. **Paso 3:** [Crear nuevas abstracciones (interfaces/clases abstractas) según el lenguaje]
4. **Paso 4:** [Refactorizar código cliente para usar nuevas abstracciones]
5. **Paso 5:** [Eliminar herencia problemática y migrar a la nueva estructura]
6. **Paso 6:** [Pruebas exhaustivas de comportamiento y regresión]

---

## Notas adicionales para el agente

- **Detecta el lenguaje automáticamente:** Analiza la sintaxis del código proporcionado para identificar el lenguaje de programación (Java, C#, Python, C++, PHP, Ruby, etc.).
- **Adapta las recomendaciones:** Ofrece soluciones específicas para el lenguaje detectado (ej: interfaces en Java/C#, clases abstractas en C++, duck typing en Python, traits en PHP).
- **Identifica problemas sutiles:** A veces las violaciones de LSP no son obvias y solo aparecen en casos específicos o con ciertos datos.
- **Proporciona contraejemplos conceptuales:** Describe escenarios que demostrarían el fallo en tiempo de ejecución sin usar código específico de un lenguaje.
- **Evalúa el diseño por contrato:** Examina precondiciones, postcondiciones e invariantes de forma conceptual.
- **Sugiere alternativas:** No siempre la herencia es la respuesta; recomienda composición, delegación o interfaces cuando sea apropiado.
- **Considera el contexto del dominio:** Algunas violaciones de LSP son aceptables en ciertos dominios o contextos específicos; menciónalo si es relevante.
- **Menciona testing:** Sugiere pruebas unitarias que ayudarían a detectar violaciones de LSP (ej: pruebas que usan la clase base y verifican comportamiento consistente).
- **Distingue entre violaciones técnicas y semánticas:** No todas las violaciones son igualmente graves; prioriza las que causan bugs reales.
- **Reconoce herencia incorrecta:** Si la relación "es-un" no es verdadera, recomienda cambiar la estructura fundamental.

## Principios relacionados

- **Design by Contract (DBC):** Los contratos deben preservarse en la herencia
- **Composición sobre herencia:** Usar composición cuando la herencia no es apropiada
- **Interface Segregation (ISP):** Crear interfaces específicas para comportamientos particulares
- **Polimorfismo:** El verdadero polimorfismo requiere LSP
- **Open/Closed (OCP):** LSP facilita la extensión sin modificación

