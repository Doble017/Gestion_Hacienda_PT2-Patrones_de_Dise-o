# Bitácora de Uso de Inteligencia Artificial

**Proyecto:** Sistema de Gestión de Hacienda
**Fase:** Modernización arquitectónica y aplicación de SOLID
**Versión:** 1.0
**Fecha:** 09/08/2026

---

# 1. Objetivo

Documentar de manera trazable el uso de herramientas de Inteligencia Artificial durante el proceso de análisis, diagnóstico, diseño y refactorización del Sistema de Gestión de Hacienda.

La bitácora tiene como propósito evidenciar que las herramientas de IA fueron utilizadas como apoyo técnico para explorar el código, detectar posibles problemas, contrastar hipótesis y evaluar alternativas, pero que las decisiones finales fueron tomadas por el equipo a partir de la revisión del código fuente, la documentación del sistema, las restricciones del reto y los criterios arquitectónicos definidos.

La IA no se considera fuente de verdad sobre el sistema. Toda propuesta relevante fue contrastada con el código fuente y, cuando correspondió, aceptada, modificada o rechazada.

---

# 2. Criterio de uso de IA

El equipo adoptó la siguiente regla de trabajo:

> **La IA puede proponer; el código y el criterio del equipo deben demostrar.**

Por esta razón, las respuestas de las herramientas no fueron incorporadas directamente al diseño. Las propuestas se utilizaron como hipótesis que debían ser verificadas contra:

* código fuente;
* relaciones estructurales;
* dependencias;
* comportamiento observable;
* documentación AS-IS;
* diagnóstico SOLID;
* arquitectura TO-BE;
* restricciones del reto;
* impacto esperado de las solicitudes de cambio.

Esta decisión es especialmente importante debido a que el reto establece que el comportamiento observable del sistema no debe modificarse, salvo en las solicitudes de cambio autorizadas.

---

# 3. Herramientas utilizadas

| Herramienta                                            | Uso principal                                                                                                                             |
| ------------------------------------------------------ | ----------------------------------------------------------------------------------------------------------------------------------------- |
| ChatGPT                                                | Análisis de código, revisión arquitectónica, contraste de propuestas, generación y revisión de documentación, apoyo en la refactorización |
| Asistente de IA utilizado para generación de diagramas | Generación preliminar de diagramas UML a partir de la documentación AS-IS                                                                 |
| Grok                                                   | Generación de versiones preliminares de diagramas de comportamiento y apoyo en su estructuración                                          |
| Analizador/agente de SOLID                             | Identificación inicial de posibles violaciones de SRP, OCP, LSP, ISP y DIP                                                                |

Las salidas de estas herramientas no fueron consideradas evidencia suficiente por sí mismas. Las afirmaciones relevantes fueron contrastadas con el código y con los demás artefactos del proyecto.

---

# 4. Primera etapa: lectura y análisis del sistema

## 4.1. Uso de IA

Se utilizó IA posteriormente a la lectura inicial del código para apoyar la identificación de:

* entidades principales;
* servicios;
* controladores;
* interfaces;
* mecanismos de persistencia;
* validadores;
* publishers;
* dependencias entre componentes;
* posibles puntos de acoplamiento.

La herramienta permitió acelerar la exploración de una base de código que inicialmente carecía de documentación arquitectónica suficiente.

## 4.2. Decisión del equipo

Las propuestas de la IA no se utilizaron directamente para construir el diagnóstico.

El equipo decidió reconstruir progresivamente:

1. inventario de clases;
2. inventario de interfaces;
3. matriz de relaciones;
4. matriz de dependencias;
5. matriz de multiplicidades;
6. inventario de flujos de comportamiento;
7. diagnóstico de hallazgos.

Esto permitió contrastar posteriormente las observaciones de la herramienta con evidencia estructural y comportamental.

---

# 5. Análisis SOLID asistido por IA

## 5.1. Resultado inicial

El análisis asistido identificó posibles problemas relacionados con:

* **SRP:** concentración de responsabilidades en `Hacienda`, `Potrero` y `PersistenciaService`;
* **OCP:** decisiones basadas en tipos concretos;
* **LSP:** riesgos asociados a determinadas jerarquías;
* **ISP:** interfaces con responsabilidades múltiples;
* **DIP:** dependencias directas hacia implementaciones concretas.

Entre los hallazgos principales se identificó que `Hacienda` concentra múltiples responsabilidades, incluyendo creación de potreros, gestión de reses, ventas, vacunación, inventario y eventos.

También se identificó que `PersistenciaService` mezcla persistencia, validación y conocimiento de mecanismos de infraestructura.

Estos hallazgos fueron posteriormente contrastados con el código fuente y registrados en el diagnóstico AS-IS.

---

# 6. Primera decisión relevante: no aceptar automáticamente todas las violaciones LSP propuestas

## Propuesta asistida

Durante el análisis de la jerarquía:

`Res → Ternero / Cebon / Novillo`

se consideró la posibilidad de que la existencia de restricciones diferentes en las subclases representara una violación de LSP.

También apareció como alternativa la eliminación de la herencia y su sustitución por composición y Strategy.

## Verificación del equipo

El equipo revisó directamente:

* comportamiento de `Res`;
* constructores de las subclases;
* sobrescritura de `Edad`;
* reglas asociadas a cada tipo;
* invariantes de la entidad;
* comportamiento esperado de las operaciones.

La revisión permitió concluir que la simple existencia de una jerarquía no constituye por sí misma una violación de LSP.

## Decisión

**Se rechazó la propuesta de eliminar automáticamente la jerarquía `Res`.**

La arquitectura TO-BE conserva:

```text
Res
├── Ternero
├── Cebon
└── Novillo
```

La decisión quedó respaldada además en el ADR correspondiente de conservación de las jerarquías.

La razón principal fue evitar una refactorización innecesaria que introdujera composición, Strategy u otros mecanismos sin demostrar que aportaran una mejora necesaria para las solicitudes de cambio.

## Evidencia

La decisión fue registrada como un hallazgo asistido posteriormente refutado:

**H-06 — Propuesta de eliminar la herencia Res → Ternero/Cebon/Novillo.**

La documentación del diagnóstico registra que esta propuesta fue rechazada y que la jerarquía se consideró válida bajo el análisis realizado.

---

# 7. Identificación de la concentración de responsabilidades en Hacienda

## Observación asistida

El análisis identificó `Hacienda` como uno de los principales puntos de concentración de responsabilidades.

Se señaló que la clase participa en:

* creación de potreros;
* incorporación de reses;
* venta de reses;
* alimentación;
* creación de vacunas;
* aplicación de vacunas;
* gestión de eventos;
* administración de inventario.

## Verificación

El equipo revisó la clase y comprobó que estas responsabilidades se encontraban efectivamente concentradas en ella.

También se contrastó la dependencia de los servicios hacia la instancia concreta de `Hacienda`.

## Decisión

El hallazgo fue **aceptado y priorizado**.

La solución propuesta posteriormente no consiste simplemente en dividir la clase en helpers, sino en separar responsabilidades mediante servicios de aplicación y repositorios por agregado.

La decisión quedó formalizada en el diseño TO-BE y en el ADR correspondiente a la partición de `Hacienda`.

---

# 8. Identificación de PersistenciaService como punto de dolor

## Observación asistida

La IA señaló que `PersistenciaService` concentra responsabilidades relacionadas con:

* lectura de archivos;
* escritura de archivos;
* serialización;
* validación;
* construcción de proxies;
* conocimiento de infraestructura.

## Verificación

El equipo revisó directamente la implementación y confirmó que la clase mezcla persistencia con mecanismos de validación y detalles de infraestructura.

Además, el análisis permitió identificar el alto costo que tendría cambiar posteriormente el mecanismo de persistencia.

## Decisión

El hallazgo fue aceptado.

Sin embargo, se rechazó una migración inmediata a una base de datos relacional.

La solución adoptada fue introducir puertos de persistencia y mantener inicialmente implementaciones basadas en archivos.

La arquitectura propuesta utiliza interfaces como:

```text
IPotreroRepository
IResRepository
IVacunaRepository
IVentaRepository
```

con implementaciones concretas en Infrastructure.

Esta decisión permite aplicar DIP sin modificar innecesariamente el medio de persistencia actual.

---

# 9. Decisión sobre la biblioteca de dominio

## Propuesta considerada

Durante la discusión arquitectónica se evaluó si `Bib_Hacienda` debía eliminarse y trasladarse completamente al proyecto MVC.

## Evaluación del equipo

Se identificó que eliminar la separación de la biblioteca y trasladar las responsabilidades directamente a Controllers y Services produciría una simplificación aparente, pero aumentaría el riesgo de volver a mezclar:

* presentación;
* aplicación;
* dominio;
* infraestructura.

Esto contradice el objetivo principal de la modernización.

## Decisión

Se decidió **no eliminar conceptualmente la separación del dominio**.

La nueva arquitectura mantiene fronteras claras entre:

```text
Domain
Application
Infrastructure
Presentation
```

La biblioteca monolítica original puede desaparecer como estructura física, pero sus responsabilidades no se mezclan nuevamente.

Esta decisión se documentó en el diseño TO-BE.

---

# 10. Decisión sobre la persistencia basada en archivos

## Propuesta posible

Una alternativa natural sugerida durante el rediseño era migrar inmediatamente de archivos de texto a una base de datos.

## Evaluación

El equipo identificó que esta modificación no era necesaria para demostrar la aplicación de SOLID y que podía modificar innecesariamente el comportamiento observable y el alcance del reto.

Los archivos de texto existentes constituyen actualmente el mecanismo utilizado para cargar y almacenar información necesaria para ejecutar el sistema.

## Decisión

Se decidió conservar inicialmente el mecanismo de archivos.

La diferencia arquitectónica estará en que el dominio y la aplicación no conocerán directamente los detalles del almacenamiento.

La implementación concreta quedará detrás de interfaces de repositorio.

Por tanto:

```text
AS-IS

Servicio
   ↓
PersistenciaService
   ↓
Archivo
```

se transforma en:

```text
TO-BE

Application / Domain
        ↓
 IRepository
        ↑
FileRepository
        ↓
Archivo
```

La decisión permite aplicar DIP sin introducir una migración de infraestructura que no es necesaria para el objetivo del reto.

---

# 11. Identificación del problema de eventos

## Observación

El análisis identificó que algunas clases del dominio crean y gestionan directamente publishers concretos.

Entre ellos se encuentran publishers asociados a:

* peso mínimo;
* peso de venta;
* capacidad de potrero;
* vacunación;
* vencimiento de vacunas.

## Verificación

El equipo revisó las referencias estructurales y observó que el dominio conoce mecanismos concretos de publicación.

Esto genera acoplamiento entre la lógica de dominio y el mecanismo utilizado para comunicar eventos.

## Decisión

El hallazgo fue aceptado.

Se decidió extraer la publicación concreta hacia Infrastructure mediante una abstracción:

```text
IEventPublisher
```

El dominio podrá producir hechos o eventos de dominio sin conocer el mecanismo concreto utilizado para publicarlos.

La implementación concreta se registra en el Composition Root.

---

# 12. Identificación de decisiones por tipo concreto

## Observación

El análisis identificó varios patrones como:

```csharp
if (res is Ternero)
else if (res is Cebon)
else if (res is Novillo)
```

y decisiones equivalentes relacionadas con vacunas.

## Evaluación

El equipo verificó que estas condiciones aparecen en partes relevantes de la lógica y que introducir nuevos tipos puede obligar a modificar clases existentes.

Este comportamiento constituye evidencia relevante para OCP y, dependiendo del contexto, puede generar problemas de acoplamiento y extensibilidad.

## Decisión

El hallazgo fue aceptado donde existe impacto demostrable.

Sin embargo, **no se decidió eliminar indiscriminadamente todos los chequeos por tipo**.

Cuando el costo de reemplazarlos por Strategy, polimorfismo u otra abstracción no se justifique por las solicitudes de cambio actuales, se conserva el comportamiento y se documenta como deuda técnica consciente.

---

# 13. Refactorización de las reglas de Res

Durante la implementación de la nueva arquitectura se revisaron las reglas existentes en:

```text
ReglaRes
```

La clase contiene reglas de edad y peso utilizadas por diferentes componentes.

## Observación

El código original permite que:

* `Ternero`;
* `Cebon`;
* `Novillo`;
* publishers;

conozcan directamente la clase de reglas.

Esto produce dependencias concretas y distribuye el conocimiento de las políticas de negocio.

## Decisión

La nueva arquitectura busca separar:

```text
Entidad
```

de:

```text
Reglas / políticas de negocio
```

manteniendo la coherencia con la arquitectura TO-BE.

Las reglas no se trasladan indiscriminadamente a las entidades ni se conserva una dependencia global hacia una clase estática equivalente a la original.

La refactorización será validada posteriormente mediante los casos de caracterización para asegurar que los límites originales de edad y peso no cambien accidentalmente.

---

# 14. Propuestas de IA rechazadas por sobreingeniería

No todas las recomendaciones recibidas fueron implementadas.

El equipo estableció como criterio que una mejora arquitectónica debe justificar:

1. reducción de acoplamiento;
2. separación real de responsabilidades;
3. mejora de extensibilidad;
4. impacto sobre las solicitudes de cambio;
5. compatibilidad con el comportamiento observable;
6. costo razonable de implementación.

Por este motivo se rechazaron o limitaron propuestas que introducían complejidad sin beneficio proporcional.

Entre las decisiones de límite se encuentran:

* no migrar inmediatamente a una base de datos;
* no introducir microservicios;
* no implementar un bus de eventos completo;
* no eliminar automáticamente las jerarquías de dominio;
* no convertir cada regla en una abstracción independiente sin necesidad;
* no aplicar Strategy a todos los chequeos por tipo únicamente por razones teóricas.

Estas decisiones forman parte de la deuda técnica consciente del diseño.

---

# 15. Uso de IA para la documentación AS-IS

La IA también fue utilizada para estructurar y revisar los documentos:

* `Inventario_Clases_AS-IS.md`
* `Matriz_Relaciones_AS-IS.md`
* `Matriz_Dependencias_AS-IS.md`
* `Matriz_Multiplicidades_AS-IS.md`
* `Inventario_Flujos_Comportamiento_AS-IS.md`

El equipo utilizó la IA para detectar inconsistencias, proponer estructuras y señalar relaciones que requerían verificación.

Sin embargo, las multiplicidades, relaciones y flujos que no contaban con evidencia suficiente fueron revisados directamente antes de ser incorporados como definitivos.

Esto permitió evitar inferencias basadas únicamente en reglas conceptuales del negocio.

---

# 16. Uso de IA para diagramas

Se utilizaron asistentes de IA para generar versiones preliminares de diagramas de comportamiento.

Las primeras versiones fueron tratadas como borradores.

Se revisaron posteriormente aspectos como:

* participantes;
* orden de mensajes;
* dependencias;
* llamadas reales;
* interacción con persistencia;
* validaciones;
* publicación de eventos;
* coherencia con el código.

Las correcciones realizadas muestran que los diagramas generados automáticamente no fueron considerados evidencia definitiva.

El criterio adoptado fue:

> **El diagrama debe representar el comportamiento comprobado en el código, no el comportamiento que la IA considere razonable.**

---

# 17. Uso de IA durante el diseño TO-BE

La IA se utilizó como apoyo para explorar alternativas de:

* separación por capas;
* interfaces;
* repositorios;
* servicios de aplicación;
* inversión de dependencias;
* publishers;
* composición del sistema.

Las propuestas fueron contrastadas con el diseño TO-BE definido por el equipo.

La arquitectura final mantiene:

```text
Presentation
      ↓
Application
      ↓
Domain
```

y coloca las implementaciones concretas de infraestructura detrás de abstracciones.

El Composition Root se mantiene en `Program.cs`, donde se registran las implementaciones concretas y se configura el grafo de dependencias.

---

# 18. Decisiones aceptadas, modificadas y rechazadas

| Propuesta / hallazgo                           | Origen                     | Decisión               | Justificación                                                       |
| ---------------------------------------------- | -------------------------- | ---------------------- | ------------------------------------------------------------------- |
| `Hacienda` concentra responsabilidades         | IA + revisión propia       | Aceptada               | Evidencia directa en código; alto impacto en cambios                |
| `PersistenciaService` mezcla responsabilidades | IA + revisión propia       | Aceptada               | Mezcla persistencia, validación e infraestructura                   |
| Dependencias concretas hacia `Hacienda`        | IA + revisión propia       | Aceptada               | Afecta DIP y dificulta pruebas/extensión                            |
| Publishers concretos dentro del dominio        | IA + revisión propia       | Aceptada               | Acoplamiento innecesario con infraestructura                        |
| Eliminar jerarquía `Res`                       | IA                         | Rechazada              | No se justificó una violación LSP suficiente; introduce complejidad |
| Migrar archivos a BD                           | Alternativa arquitectónica | Rechazada              | Fuera del objetivo; no necesaria para aplicar SOLID                 |
| Introducir microservicios                      | Alternativa arquitectónica | Rechazada              | Sobreingeniería para el tamaño y alcance del sistema                |
| Implementar bus de eventos completo            | Alternativa arquitectónica | Rechazada              | Complejidad superior al beneficio esperado                          |
| Extraer repositorios detrás de interfaces      | IA + análisis propio       | Aceptada               | Mejora DIP manteniendo archivos                                     |
| Segregar servicios de aplicación               | IA + análisis propio       | Aceptada               | Reduce superficie de interfaces y responsabilidades                 |
| Extraer publicación de eventos mediante puerto | IA + análisis propio       | Aceptada               | Reduce dependencia del dominio hacia infraestructura                |
| Eliminar todos los chequeos por tipo           | IA                         | Parcialmente rechazada | Algunos pueden mantenerse como deuda técnica consciente             |

---

# 19. Hallazgos propios derivados del proceso

Además de las propuestas generadas por IA, el análisis del equipo permitió identificar y verificar situaciones que debían tratarse con mayor precisión.

Entre ellas:

* la necesidad de diferenciar relaciones estructurales de dependencias temporales;
* la necesidad de no inferir multiplicidades únicamente desde reglas del negocio;
* la existencia de referencias directas de controladores hacia elementos concretos del dominio;
* la concentración simultánea de persistencia y validación en `PersistenciaService`;
* la necesidad de mantener los archivos de soporte para preservar el mecanismo actual de ejecución;
* la necesidad de comprobar los flujos de comportamiento directamente contra el código antes de construir los diagramas definitivos;
* la diferencia entre identificar una posible violación SOLID y justificar que dicha violación tenga impacto arquitectónico real.

Estos hallazgos fueron incorporados progresivamente al diagnóstico y al diseño.

---

# 20. Límite de rediseño adoptado

El equipo decidió que la modernización no tiene como objetivo producir la arquitectura más sofisticada posible.

El objetivo es reducir los principales costos de cambio identificados sin alterar innecesariamente el comportamiento existente.

Por ello, el rediseño se limita a:

* separar responsabilidades relevantes;
* invertir dependencias;
* introducir abstracciones donde existe una dependencia concreta problemática;
* segregar interfaces cuando los clientes tienen necesidades diferentes;
* extraer infraestructura del dominio;
* mejorar la extensibilidad frente a las solicitudes de cambio;
* mantener el mecanismo actual de persistencia;
* conservar las jerarquías justificadas;
* evitar introducir tecnologías o patrones que no aporten valor demostrable.

Este límite busca evitar sobreingeniería y mantener la refactorización proporcional al problema real.

---

# 21. Principio general utilizado para las decisiones

Durante el proyecto se adoptó el siguiente criterio:

```text
Propuesta de IA
      ↓
¿Existe evidencia en el código?
      ↓
¿Tiene impacto arquitectónico real?
      ↓
¿Afecta alguno de los principios SOLID?
      ↓
¿Mejora la capacidad de evolución?
      ↓
¿Preserva el comportamiento?
      ↓
Decisión del equipo
```

Por tanto, una propuesta no fue implementada simplemente por haber sido generada por una herramienta de IA.

---

# 22. Evidencia y trazabilidad

Las decisiones registradas en esta bitácora deben poder contrastarse con los siguientes artefactos:

| Evidencia                                         | Propósito                                    |
| ------------------------------------------------- | -------------------------------------------- |
| `01_Diagnostico_Hallazgos_y_Puntos_de_Dolor.docx` | Hallazgos y puntos de dolor del AS-IS        |
| `Matriz_Relaciones_AS-IS.md`                      | Relaciones estructurales verificadas         |
| `Matriz_Dependencias_AS-IS.md`                    | Dependencias y acoplamientos                 |
| `Matriz_Multiplicidades_AS-IS.md`                 | Cardinalidades                               |
| `Inventario_Flujos_Comportamiento_AS-IS.md`       | Flujos de comportamiento                     |
| `03_Diseno_TOBE_Arquitectura_Completo.docx`       | Decisiones de arquitectura TO-BE             |
| ADR-01                                            | Partición de `Hacienda`                      |
| ADR-02                                            | Puertos de persistencia                      |
| ADR-03                                            | Segregación de interfaces                    |
| ADR-04                                            | Extracción de publishers                     |
| ADR-05                                            | Conservación de jerarquías                   |
| Código AS-IS                                      | Evidencia original                           |
| Código TO-BE                                      | Implementación de las decisiones             |
| Casos de caracterización                          | Evidencia de preservación del comportamiento |

---

# 23. Conclusión

El uso de IA en el proyecto fue principalmente asistivo.

Las herramientas permitieron acelerar la exploración del código, identificar posibles problemas, generar hipótesis y comparar alternativas. Sin embargo, las decisiones arquitectónicas finales se tomaron mediante contraste con el código fuente, el comportamiento observado, las restricciones del reto y el impacto de cada cambio.

Una parte importante del proceso consistió precisamente en **no aceptar algunas recomendaciones de la IA**.

La decisión de conservar la jerarquía `Res`, mantener los archivos como mecanismo de persistencia, evitar microservicios y limitar la introducción de patrones adicionales demuestra que el objetivo no fue aplicar SOLID de manera mecánica, sino utilizar los principios para resolver problemas concretos del sistema.

La arquitectura propuesta busca, por tanto, mejorar la capacidad de evolución del sistema reduciendo acoplamiento y responsabilidades excesivamente concentradas, sin convertir la refactorización en una reescritura completa ni modificar la conducta observable no autorizada.

---

# 24. Registro resumido de criterio técnico

| Aspecto                                     | Resultado |
| ------------------------------------------- | --------- |
| IA utilizada como apoyo                     | Sí        |
| Propuestas verificadas contra código        | Sí        |
| Propuestas rechazadas                       | Sí        |
| Hallazgos propios documentados              | Sí        |
| Decisiones arquitectónicas justificadas     | Sí        |
| Alternativas consideradas                   | Sí        |
| Límite contra sobreingeniería               | Sí        |
| Preservación del comportamiento considerada | Sí        |
| Trazabilidad hacia documentos y código      | Sí        |

**Criterio rector:** la herramienta proporciona hipótesis y alternativas; la decisión arquitectónica pertenece al equipo y debe poder defenderse mediante evidencia verificable.
