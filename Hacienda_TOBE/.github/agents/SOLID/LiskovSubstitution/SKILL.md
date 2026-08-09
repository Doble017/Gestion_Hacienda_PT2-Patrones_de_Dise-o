---
name: LiskovSubstitution
description: Evalúa si un diseño cumple el principio de sustitución de Liskov (LSP) y sugiere mejoras cuando la herencia está siendo mal utilizada.
---

# Skill: Liskov Substitution (LSP)

## Propósito
Utiliza esta skill cuando quieras revisar si una jerarquía de herencia respeta el principio de sustitución de Liskov y si un diseño puede mejorar al reemplazar herencia forzada por interfaces, composición o agregación.

## Capacidades
- Evaluar si una subclase puede sustituir a su superclase sin romper el comportamiento esperado.
- Revisar contratos de métodos, incluyendo tipos de parámetros, tipos de retorno, excepciones y condiciones de pre y postcondición.
- Detectar violaciones de invariantes y cambios no deseados en el estado interno de la superclase.
- Sugerir refactorizaciones cuando la herencia sea inapropiada, por ejemplo:
  - reemplazar herencia por interfaces cuando varias clases compartan un comportamiento común;
  - usar composición o agregación cuando la relación “es-un” no sea natural;
  - introducir abstracciones más generales para evitar acoplamientos innecesarios.

## Protocolo de evaluación
Aplica esta revisión de forma sistemática:

1. Compatibilidad de parámetros
   - Los tipos de parámetros en un método de una subclase deben coincidir o ser más abstractos que los tipos de parámetros del método de la superclase.
   - Si una subclase exige un tipo más específico, es probable que rompa el contrato de sustitución.

2. Compatibilidad de retorno
   - El tipo de retorno en un método de una subclase debe coincidir o ser un subtipo del tipo de retorno del método de la superclase.
   - Un retorno más restrictivo o incompatible suele indicar una violación de LSP.

3. Manejo de excepciones
   - Un método en una subclase no debe generar tipos de excepciones que no se espera que genere el método base.
   - Si la subclase lanza errores adicionales que el cliente no anticipa, el contrato se ha vuelto más fuerte de lo permitido.

4. Condiciones previas
   - Una subclase no debe reforzar las condiciones previas.
   - Si exige más restricciones que la clase base, no podrá sustituirla en todos los escenarios válidos.

5. Condiciones posteriores
   - Una subclase no debe debilitar las condiciones posteriores.
   - Si produce resultados menos confiables o menos fuertes de lo esperado, el comportamiento ya no es compatible.

6. Invariantes
   - Las invariantes de una superclase deben conservarse.
   - Cualquier regla que deba mantenerse en todo estado válido debe seguir siendo válida en la subclase.

7. Estado interno
   - Una subclase no debe cambiar los valores de los campos privados de la superclase.
   - Cualquier modificación indirecta o inesperada del estado base debe considerarse una violación del principio.

## Criterios de recomendación
Cuando detectes una violación, recomienda una de estas soluciones según corresponda:
- Introducir una interfaz para modelar el comportamiento compartido.
- Mover la responsabilidad a una clase colaboradora mediante composición.
- Usar agregación cuando la relación entre clases no sea estrictamente jerárquica.
- Rediseñar la jerarquía para que las subclases respeten el contrato base sin requerir comportamientos especiales o incompatibles.

## Formato de salida esperado
Presenta tus hallazgos con esta estructura:
- Resumen del análisis.
- Violaciones detectadas por criterio.
- Impacto sobre sustituibilidad.
- Recomendación de refactorización con justificación.
- Opciones de diseño: interfaz, composición, agregación o nueva abstracción.
