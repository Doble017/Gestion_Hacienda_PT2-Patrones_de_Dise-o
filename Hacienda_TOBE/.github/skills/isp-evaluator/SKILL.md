---
name: isp-evaluator
user-invocable: true
description: "Evalúa interfaces para verificar el cumplimiento del Principio de Segregación de Interfaces (ISP) de SOLID y recomienda dividir interfaces grandes en interfaces más pequeñas y específicas."
---

# Habilidad de Evaluación ISP

Utiliza esta habilidad para evaluar si las interfaces están correctamente segregadas, asegurando que las clases no se vean forzadas a implementar métodos que no necesitan.

## Lo que evalúa esta habilidad
- Interfaces con demasiados métodos no relacionados
- Clases que implementan métodos vacíos o que lanzan excepciones
- Interfaces que mezclan responsabilidades de diferentes dominios
- Clientes que dependen de métodos que no utilizan
- Interfaces "gordas" o "fat interfaces" que deberían dividirse
- Alto acoplamiento causado por interfaces demasiado amplias
- Dependencias innecesarias en métodos no utilizados
- Violación del principio de cohesión de interfaces
- Interfaces que cambian frecuentemente afectando a múltiples clientes

## Protocolo de evaluación
1. Una interfaz debe tener un propósito único y bien definido.
2. Los clientes no deben depender de métodos que no utilizan.
3. Las interfaces deben ser específicas para cada tipo de cliente.
4. Las interfaces grandes deben dividirse en interfaces más pequeñas y enfocadas.
5. Las clases no deben implementar métodos que no tienen sentido para ellas.
6. Los cambios en una interfaz no deben afectar a clientes que no usan esos métodos.
7. La segregación debe basarse en los diferentes roles o comportamientos.
8. Proporcionar recomendaciones concretas para dividir interfaces existentes.

## Uso recomendado
- Analizar interfaces en cualquier lenguaje de programación que las soporte.
- Identificar interfaces que violan ISP y causan acoplamiento innecesario.
- Recomendar divisiones de interfaces basadas en roles y responsabilidades.

> Ejemplo: "Analiza esta interfaz y sugiere cómo dividirla en interfaces más específicas para cumplir con ISP."

## Formato de respuesta sugerido

Al analizar una interfaz, estructura tu respuesta de la siguiente manera:

### 🔍 ANÁLISIS DE ISP

**Interfaz evaluada:** [Nombre de la interfaz]

**Lenguaje detectado:** [Java/C#/Python/PHP/TypeScript/etc.]

**Estado general:** ✅ Cumple ISP / ⚠️ Violaciones menores / ❌ Violaciones graves

---

### 📋 VIOLACIONES ENCONTRADAS

[Lista numerada de cada violación encontrada, indicando:]
- **Violación #1:** [Descripción clara]
  - **Ubicación:** [Método(s) problemático(s)]
  - **Tipo de violación:** [Interfaz muy grande / Métodos no relacionados / Clientes forzados]
  - **Razón:** [Por qué esto viola ISP]
  - **Impacto:** [Qué problemas causa: acoplamiento, cambios innecesarios, implementaciones vacías]
  - **Clientes afectados:** [Qué clases/cliente se ven forzados a implementar métodos no deseados]

---

### 🛠️ RECOMENDACIONES DE REFACTORIZACIÓN

**Opción 1: Segregación basada en roles**
- **Interfaz original:** [Nombre de la interfaz]
- **Interfaces propuestas:** [Lista de nuevas interfaces]
- **Criterio de división:** [Basado en qué responsabilidades/roles]
- **Beneficio:** [Qué problemas resuelve]

**Opción 2: Segregación basada en clientes**
- **Interfaz original:** [Nombre de la interfaz]
- **Interfaces propuestas:** [Lista de interfaces específicas para cada cliente]
- **Criterio de división:** [Basado en las necesidades de los clientes]
- **Beneficio:** [Qué problemas resuelve]

**Opción 3: Uso de herencia de interfaces**
- **Interfaz base:** [Interfaz común con métodos compartidos]
- **Interfaces extendidas:** [Interfaces que añaden métodos específicos]
- **Cuándo usar:** [Cuando hay funcionalidad común y especializada]

**Opción 4: Adaptadores o fachadas**
- **Cuándo usar:** [Cuando no se puede modificar la interfaz original]
- **Cómo implementar:** [Crear adaptadores que implementen interfaces específicas]

---

### 🎯 INTERFACES PROPUESTAS

| Nueva Interfaz | Métodos | Propósito | Clientes que la usarían |
|----------------|---------|-----------|-------------------------|
| [Interfaz 1] | [Lista de métodos] | [Propósito] | [Clientes] |
| [Interfaz 2] | [Lista de métodos] | [Propósito] | [Clientes] |
| [Interfaz 3] | [Lista de métodos] | [Propósito] | [Clientes] |

---


### 🚨 SEÑALES DE ALERTA ISP

- [ ] Interfaces con más de [número] métodos
- [ ] Métodos que lanzan excepciones de "no implementado"
- [ ] Métodos vacíos en implementaciones
- [ ] Interfaces con "y" en el nombre (ej: "XandYInterface")
- [ ] Interfaz que mezcla lógica de negocio con persistencia
- [ ] Interfaz que mezcla validación con procesamiento
- [ ] Cambios en la interfaz que afectan a múltiples clientes no relacionados
- [ ] Documentación que dice "este método es opcional"

---

### 📝 PLAN DE MIGRACIÓN

1. **Paso 1:** [Identificar todos los clientes de la interfaz y qué métodos usan]
2. **Paso 2:** [Agrupar métodos por roles o responsabilidades]
3. **Paso 3:** [Diseñar nuevas interfaces específicas]
4. **Paso 4:** [Hacer que las clases implementen las nuevas interfaces]
5. **Paso 5:** [Refactorizar clientes para usar interfaces específicas]
6. **Paso 6:** [Eliminar métodos no utilizados de implementaciones]
7. **Paso 7:** [Marcar la interfaz original como obsoleta (deprecate)]

---

### 🔄 ANTES Y DESPUÉS (Conceptual)

**ANTES - Violación de ISP:**
- Una interfaz única con métodos de diferentes dominios
- Clientes forzados a implementar métodos que no necesitan
- Métodos vacíos o con excepciones
- Cambios en la interfaz afectan a todos los clientes

**DESPUÉS - Cumple ISP:**
- Múltiples interfaces específicas y enfocadas
- Cada cliente implementa solo lo que necesita
- Todas las implementaciones son significativas
- Cambios en una interfaz solo afectan a clientes relevantes

---

## Notas adicionales para el agente

- **Detecta el lenguaje automáticamente:** Analiza la sintaxis para identificar si usa interfaces (Java, C#, TypeScript, PHP, etc.) o duck typing (Python, Ruby).
- **Adapta las recomendaciones:** Para lenguajes sin soporte explícito de interfaces, sugiere clases abstractas, protocolos o tipos.
- **Identifica problemas sutiles:** A veces las interfaces grandes son aceptables si todos los métodos están relacionados.
- **Considera el contexto del dominio:** La división debe tener sentido en el dominio del negocio, no solo técnicamente.
- **Prioriza la cohesión:** Agrupa métodos que cambian juntos y están relacionados funcionalmente.
- **Sugiere nombres significativos:** Las nuevas interfaces deben tener nombres que reflejen claramente su propósito.
- **Menciona el impacto de cambios:** Explica cómo la segregación reduce el impacto de cambios futuros.
- **Evalúa la frecuencia de cambio:** Interfaz que cambia frecuentemente debe ser más pequeña y estable.
- **Considera la evolución futura:** Diseña interfaces pensando en cómo podrían evolucionar.
- **Distingue entre ISP y SRP:** ISP se enfoca en interfaces (contratos), SRP en clases (implementaciones).

## Principios relacionados

- **Single Responsibility (SRP):** Las interfaces deben tener una única responsabilidad
- **Liskov Substitution (LSP):** Interfaces segregadas facilitan la sustitución correcta
- **Dependency Inversion (DIP):** Depender de interfaces específicas, no de implementaciones concretas
- **Composición sobre herencia:** Usar interfaces para composición de comportamientos


