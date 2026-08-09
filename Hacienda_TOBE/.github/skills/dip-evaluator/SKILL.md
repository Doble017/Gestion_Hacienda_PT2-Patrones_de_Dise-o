---
name: dip-evaluator
user-invocable: true
description: "Evalúa dependencias entre módulos para verificar el cumplimiento del Principio de Inversión de Dependencias (DIP) de SOLID y recomienda estrategias para invertir dependencias hacia abstracciones."
---

# Habilidad de Evaluación DIP

Utiliza esta habilidad para evaluar si las dependencias entre módulos siguen el Principio de Inversión de Dependencias, asegurando que los módulos de alto nivel no dependan de módulos de bajo nivel, sino que ambos dependan de abstracciones.

## Lo que evalúa esta habilidad
- Módulos de alto nivel que dependen directamente de módulos de bajo nivel
- Dependencias en implementaciones concretas en lugar de abstracciones
- Clases que instancian directamente sus dependencias
- Uso de clases concretas en lugar de interfaces o clases abstractas
- Cadena de dependencias rígidas que dificultan el testing
- Violación del patrón de inyección de dependencias
- Módulos con alto acoplamiento a implementaciones específicas
- Dificultad para cambiar o reemplazar implementaciones
- Dependencias transitivas que propagan cambios
- Módulos que no pueden ser probados de forma aislada

## Protocolo de evaluación
1. Los módulos de alto nivel no deben depender de módulos de bajo nivel; ambos deben depender de abstracciones.
2. Las abstracciones no deben depender de detalles; los detalles deben depender de abstracciones.
3. Las dependencias deben apuntar hacia las abstracciones, no hacia las implementaciones.
4. Los módulos deben depender de interfaces o clases abstractas, no de clases concretas.
5. La creación de dependencias debe estar fuera del módulo que las usa (inversión de control).
6. Los cambios en implementaciones concretas no deben afectar a módulos de alto nivel.
7. Las abstracciones deben ser estables y los detalles pueden cambiar.
8. Proporcionar estrategias concretas para invertir dependencias.

## Uso recomendado
- Analizar dependencias en cualquier lenguaje de programación.
- Identificar dependencias concretas que violan DIP y sugieren inversión.
- Recomendar patrones de inyección de dependencias y estrategias de desacoplamiento.

> Ejemplo: "Analiza estas dependencias y sugiere cómo invertirlas para cumplir con DIP."

## Formato de respuesta sugerido

Al analizar dependencias, estructura tu respuesta de la siguiente manera:

### 🔍 ANÁLISIS DE DIP

**Módulo(s) evaluado(s):** [Nombre de los módulos/clases]

**Lenguaje detectado:** [Java/C#/Python/PHP/TypeScript/C++/etc.]

**Estado general:** ✅ Cumple DIP / ⚠️ Violaciones menores / ❌ Violaciones graves

---

### 📋 VIOLACIONES ENCONTRADAS

[Lista numerada de cada violación encontrada, indicando:]
- **Violación #1:** [Descripción clara]
  - **Ubicación:** [Clase/Método donde ocurre la dependencia]
  - **Dependencia problemática:** [Clase concreta de la que depende]
  - **Tipo de violación:** [Dependencia directa / Instanciación interna / Cadena rígida]
  - **Razón:** [Por qué esto viola DIP]
  - **Impacto:** [Qué problemas causa: acoplamiento, testabilidad, mantenibilidad]

---

### 📊 ANÁLISIS DE DEPENDENCIAS

| Módulo Alto Nivel | Dependencia | Tipo | ¿Es Abstracción? | Nivel de Acoplamiento |
|-------------------|-------------|------|------------------|----------------------|
| [Módulo 1] | [Clase concreta] | [Concreto/Abstracto] | ✅/❌ | [Alto/Medio/Bajo] |
| [Módulo 2] | [Clase concreta] | [Concreto/Abstracto] | ✅/❌ | [Alto/Medio/Bajo] |

---

### 🔗 DIAGRAMA DE DEPENDENCIAS (Conceptual)

[Representación textual de las dependencias actuales y propuestas]

---

### 🛠️ RECOMENDACIONES DE REFACTORIZACIÓN

**Opción 1: Inyección de dependencias**
- **Problema:** [Dependencia concreta difícil de reemplazar]
- **Solución propuesta:** [Inyectar una interfaz o clase abstracta]
- **Beneficio:** [Mejor testabilidad y flexibilidad]

**Opción 2: Introducir abstracciones**
- **Problema:** [Módulo depende de detalles concretos]
- **Solución propuesta:** [Crear interfaz o clase abstracta para el servicio o componente]
- **Beneficio:** [Desacoplamiento de implementaciones]

**Opción 3: Inversión de control**
- **Problema:** [El módulo crea sus propias dependencias]
- **Solución propuesta:** [Mover la creación de dependencias a un contenedor o factory]
- **Beneficio:** [Menor acoplamiento y mayor control externo]

**Opción 4: Desacoplar capas**
- **Problema:** [Dependencia transitiva entre capas]
- **Solución propuesta:** [Separar interfaces de dominio y persistencia]
- **Beneficio:** [Mayor mantenibilidad y menor impacto de cambios]

---

### 🎯 ABSTRACCIONES PROPUESTAS

| Abstracción | Implementaciones | Uso esperado |
|-------------|-------------------|--------------|
| [Interfaz/Clase abstracta] | [Clases concretas] | [Qué módulo debería usarla] |

---

### 💡 PATRONES DE DISEÑO RELACIONADOS

- **Dependency Injection:** [Inyectar dependencias desde el exterior]
- **Factory Pattern:** [Centralizar creación de dependencias]
- **Service Locator:** [Uso controlado, con cautela]
- **Repository Pattern:** [Separar acceso a datos de la lógica de negocio]
- **Adapter Pattern:** [Adaptar implementaciones concretas a abstracciones]

---

### 🚨 SEÑALES DE ALERTA DIP

- [ ] Clases que crean sus propias dependencias con new
- [ ] Dependencia directa a clases concretas en el constructor o método
- [ ] Módulos que no pueden probarse sin cargar todo el sistema
- [ ] Cambios en una implementación afectan a muchos módulos
- [ ] Implementaciones concretas usadas en el código de negocio
- [ ] Acoplamiento fuerte entre capas de aplicación y datos
- [ ] Múltiples if/switch para elegir implementaciones concretas

---

### 📝 PLAN DE MIGRACIÓN

1. **Paso 1:** [Identificar dependencias concretas y su impacto]
2. **Paso 2:** [Definir abstracciones estables para esas dependencias]
3. **Paso 3:** [Aplicar inyección de dependencias o inversión de control]
4. **Paso 4:** [Refactorizar clientes para depender de interfaces o abstracciones]
5. **Paso 5:** [Aislar implementaciones concretas en módulos separados]
6. **Paso 6:** [Validar con pruebas unitarias y de integración]

---

## Notas adicionales para el agente

- **Detecta el lenguaje automáticamente:** Analiza la sintaxis para identificar si usa interfaces, clases abstractas, módulos o protocolos.
- **Adapta las recomendaciones:** Para lenguajes sin soporte explícito de interfaces, sugiere clases abstractas, protocolos, traits o tipos.
- **Identifica problemas sutiles:** A veces la violación de DIP no es obvia y aparece como acoplamiento oculto entre capas.
- **Considera el contexto del dominio:** Algunas dependencias concretas pueden ser aceptables si el diseño lo justifica, pero deben explicarse.
- **Prioriza la testabilidad:** Un buen diseño DIP facilita pruebas aisladas y reemplazo de dependencias.
- **Sugiere nombres significativos:** Las abstracciones deben nombrarse según el rol o contrato que representan.
- **Menciona el impacto de cambios:** Explica cómo la inversión de dependencias reduce el efecto de futuros cambios.
- **Evalúa el grado de acoplamiento:** Cuanto más fuerte sea el acoplamiento, más urgente será la refactorización.
- **Distingue entre DIP y DI:** DIP es el principio; DI es una técnica para aplicarlo.

## Principios relacionados

- **Dependency Inversion (DIP):** El principio central de este análisis
- **Open/Closed (OCP):** Desacoplar dependencias facilita la extensión sin modificación
- **Interface Segregation (ISP):** Interfaces pequeñas y específicas favorecen la inversión de dependencias
- **Liskov Substitution (LSP):** Abstracciones bien diseñadas permiten sustitución segura

