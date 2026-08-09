# Inventario de Flujos de Comportamiento AS-IS

**Proyecto:** Sistema de Gestión de Hacienda
**Versión:** AS-IS 1.0
**Fecha:** 07/08/2026

---

# 1. Objetivo

Identificar y documentar los principales flujos de comportamiento existentes en el sistema actual.

Estos flujos servirán como base para la construcción de diagramas de secuencia y actividad AS-IS y permitirán relacionar posteriormente el comportamiento observado con los hallazgos de diseño arquitectónico.

---

# 2. Alcance

Se documentarán los procesos relevantes para comprender:

* interacción entre usuario y aplicación;
* interacción entre controladores y servicios;
* ejecución de reglas y validaciones;
* acceso a persistencia;
* autenticación;
* publicación de eventos;
* manejo de resultados y errores.

El comportamiento será representado tal como existe actualmente en el código.

---

# 3. Criterios de documentación

Cada flujo deberá identificar, cuando corresponda:

1. Actor o elemento que inicia la operación.
2. Componente receptor.
3. Servicios involucrados.
4. Entidades involucradas.
5. Validaciones ejecutadas.
6. Acceso a persistencia.
7. Eventos generados.
8. Resultado de la operación.
9. Caminos alternativos o errores.

No se incorporarán decisiones correspondientes a la arquitectura propuesta.

---

# 4. Flujos principales identificados

| ID     | Flujo                              | Actor iniciador | Componentes principales                                                  | Diagrama previsto     |
| ------ | ---------------------------------- | --------------- | ------------------------------------------------------------------------ | --------------------- |
| FLU-01 | Autenticación de usuario           | Usuario         | AccountController, UsuarioService, Autenticacion                         | Secuencia             |
| FLU-02 | Registro de potrero                | Usuario         | PotreroController, PotreroService, Hacienda, PersistenciaService         | Secuencia             |
| FLU-03 | Registro de res                    | Usuario         | ResController, ResService, PotreroService, Hacienda, PersistenciaService | Secuencia             |
| FLU-04 | Registro de vacuna                 | Usuario         | VacunaController, VacunaService, ResService, PotreroService              | Secuencia             |
| FLU-05 | Registro de venta                  | Usuario         | VentaController, VentaService, Hacienda, PersistenciaService             | Secuencia             |
| FLU-06 | Validación de información          | Sistema         | InterceptorValidarInformacion, IValidarInformacion, Validadores          | Secuencia / Actividad |
| FLU-07 | Persistencia de información        | Sistema         | PersistenciaService, validadores, sistema de archivos                    | Secuencia             |
| FLU-08 | Publicación de eventos             | Sistema         | Hacienda, Potrero, Publishers, eventos                                   | Secuencia             |
| FLU-09 | Autenticación mediante interceptor | Sistema         | InterceptorAutenticacion, IHttpContextAccessor                           | Secuencia             |
| FLU-10 | Carga inicial del sistema          | Aplicación      | Program, Hacienda, PersistenciaService                                   | Secuencia             |

---

# 5. Flujo FLU-01: Autenticación de usuario

## Descripción

Representa el proceso mediante el cual un usuario intenta autenticarse en la aplicación.

## Elementos involucrados

Usuario
↓
AccountController
↓
UsuarioService
↓
Usuario
↓
PersistenciaService
↓
Autenticacion

## Comportamiento identificado

El flujo parte de una operación de autenticación iniciada desde el controlador de cuenta.

`UsuarioService` participa en la gestión de la información correspondiente al usuario y mantiene una referencia hacia `PersistenciaService`.

La autenticación se encuentra representada mediante la implementación concreta `Autenticacion`, que implementa el contrato `IAutenticacion`.

## Resultado

* Credenciales válidas: la operación de autenticación continúa con el resultado correspondiente.
* Credenciales inválidas: la operación devuelve el resultado correspondiente al mecanismo de autenticación.

## Diagrama asociado

`SEQ-01_Autenticacion_AS-IS`

---

# 6. Flujo FLU-02: Registro de potrero

## Descripción

Representa el proceso mediante el cual se registra o modifica información relacionada con un potrero.

## Elementos involucrados

Usuario
↓
PotreroController
↓
PotreroService
↓
Hacienda
↓
PersistenciaService

## Comportamiento identificado

El controlador recibe la operación y delega parte de su ejecución en `PotreroService`.

`PotreroService` mantiene una referencia hacia `Hacienda` y hacia `PersistenciaService`.

La lógica de dominio asociada a `Potrero` incluye reglas relacionadas con la creación de reses, validación de edad y capacidad, generación de eventos y construcción de mensajes.

## Diagrama asociado

`SEQ-02_RegistroPotrero_AS-IS`

---

# 7. Flujo FLU-03: Registro de res

## Descripción

Representa el proceso mediante el cual se registra una res dentro del sistema.

## Elementos involucrados

Usuario
↓
ResController
↓
ResService
↓
PotreroService
↓
Hacienda
↓
Validación
↓
PersistenciaService

## Comportamiento identificado

El proceso involucra al controlador de res y a los servicios relacionados con reses y potreros.

`Potrero` participa directamente en la creación de reses. La implementación actual determina el subtipo concreto de `Res` mediante una selección basada en el tipo de potrero. El diagnóstico del código identifica específicamente el uso de un `switch` basado en `tipo_potrero` dentro de `Potrero.anadir_res(...)`.

La lógica de `Potrero` también ejecuta reglas relacionadas con edad y capacidad y puede disparar eventos durante la operación.

La información posteriormente participa en los mecanismos de persistencia.

## Efectos identificados

* creación de una instancia concreta de `Res`;
* incorporación de la res al estado del potrero;
* ejecución de reglas del dominio;
* posible generación de eventos;
* persistencia del estado.

## Diagrama asociado

`SEQ-03_RegistroRes_AS-IS`

---

# 8. Flujo FLU-04: Registro de vacuna

## Descripción

Representa el proceso mediante el cual se registra una vacuna asociada a una res.

## Elementos involucrados

Usuario
↓
VacunaController
↓
VacunaService
↓
ResService
↓
PotreroService
↓
Validación
↓
PersistenciaService

## Comportamiento identificado

La lógica de vacunas se encuentra centralizada parcialmente en `Hacienda`.

`Hacienda` contiene operaciones para crear vacunas bacterianas y vivas y para aplicar vacunas a las reses.

La aplicación de vacunas utiliza decisiones basadas en el tipo concreto de la res:

* `Ternero`;
* `Novillo`;
* `Cebon`.

También utiliza decisiones basadas en el tipo concreto de vacuna:

* `Bacteriana`;
* `Viva`.

Estas decisiones forman parte del comportamiento actual y deberán representarse en el diagrama de secuencia cuando intervengan en la operación.

## Efectos identificados

* creación de la vacuna;
* asociación con el dominio correspondiente;
* ejecución de reglas de aplicación;
* posible generación de eventos;
* persistencia del estado resultante.

## Diagrama asociado

`SEQ-04_RegistroVacuna_AS-IS`

---

# 9. Flujo FLU-05: Registro de venta

## Descripción

Representa el proceso de gestión de una venta.

## Elementos involucrados

Usuario
↓
VentaController
↓
VentaService
↓
Hacienda
↓
Validación
↓
PersistenciaService
↓
Eventos

## Comportamiento identificado

`Hacienda` contiene lógica relacionada con la venta de reses y con la administración del inventario.

La operación de venta forma parte de las responsabilidades actualmente centralizadas en `Hacienda`.

El flujo también puede involucrar mecanismos de validación y persistencia.

La gestión de eventos forma parte de las responsabilidades de `Hacienda`, por lo que una operación de venta puede encontrarse relacionada con publishers del dominio.

## Diagrama asociado

`SEQ-05_RegistroVenta_AS-IS`

---

# 10. Flujo FLU-06: Validación de información

## Descripción

Representa el mecanismo utilizado actualmente para validar información.

## Elementos involucrados

Operación
↓
InterceptorValidarInformacion
↓
IValidarInformacion
↓
Validacion
↓
Validador específico

## Validadores identificados

* `ValidadorRes`
* `ValidadorPotrero`
* `ValidadorVacuna`
* `ValidadorVenta`

## Comportamiento identificado

El sistema utiliza `IValidarInformacion` como contrato común para las validaciones.

`Validacion` implementa dicho contrato y las clases concretas de validación heredan de ella.

La interfaz concentra métodos correspondientes a las cuatro categorías de validación:

* validación de res;
* validación de potrero;
* validación de vacuna;
* validación de venta.

El diagnóstico del código confirma además que existen implementaciones específicas que no utilizan todos los métodos del contrato y que algunas presentan `NotImplementedException`.

`PersistenciaService` crea proxies concretos de:

* `ValidadorVacuna`;
* `ValidadorPotrero`;
* `ValidadorRes`;
* `ValidadorVenta`.

Además, `PersistenciaService` utiliza `IHttpContextAccessor` para transportar resultados o mensajes relacionados con la validación mediante `HttpContext`.

## Diagrama asociado

`SEQ-06_Validacion_AS-IS`

---

# 11. Flujo FLU-07: Persistencia

## Descripción

Representa el proceso mediante el cual el sistema almacena o recupera información utilizando archivos.

## Elementos involucrados

Servicio
↓
PersistenciaService
↓
Validadores
↓
Sistema de archivos

## Comportamiento identificado

`PersistenciaService` concentra las operaciones de lectura y escritura de archivos.

Además de las responsabilidades de persistencia, el servicio inicializa proxies de validación y utiliza información del contexto HTTP para transportar resultados de validación.

Durante la reconstrucción del estado:

`PersistenciaService.CargarReses(...)`

utiliza:

`Potrero.anadir_res(...)`

Esto significa que la carga de reses no se limita a reconstruir datos pasivamente, sino que puede ejecutar nuevamente lógica de negocio, validaciones y eventos.

Asimismo:

`CargarVentas(...)`

genera nuevas instancias de `Res` durante la reconstrucción de las ventas.

## Efectos identificados

* lectura de información almacenada;
* escritura de información;
* creación de objetos durante la carga;
* ejecución de lógica de dominio durante `CargarReses`;
* ejecución de validaciones y eventos durante la reconstrucción de reses;
* interacción con componentes de infraestructura.

## Diagrama asociado

`SEQ-07_Persistencia_AS-IS`

---

# 12. Flujo FLU-08: Publicación de eventos

## Descripción

Representa la comunicación basada en eventos utilizada por el sistema.

## Publishers identificados

* `PublisherPesoMin`
* `PublisherPesoVenta`
* `PublisherPotreroMitad`
* `PublisherPotreroLleno`
* `PublisherVacunacionCompletada`
* `PublisherVacunaVencida`

## Elementos del comportamiento

`Hacienda` y `Potrero` contienen lógica relacionada con la gestión de eventos.

El patrón observado es:

Cambio en el dominio
↓
Condición de negocio
↓
Publisher
↓
Evento
↓
Suscripción / ejecución del handler

El diagnóstico del código confirma que existen publishers que son suscritos dentro de métodos de operación.

La suscripción y desuscripción se encuentra acoplada al método que ejecuta la operación, generando suscripciones temporales anidadas y el riesgo de duplicación de handlers cuando un mismo objeto es reutilizado.

## Comportamiento confirmado

Los eventos no constituyen únicamente una relación estructural permanente entre las entidades y los publishers. Se generan como consecuencia de determinadas operaciones del dominio.

Por ello, la relación deberá representarse principalmente como comportamiento en el diagrama de secuencia.

## Diagrama asociado

`SEQ-08_Eventos_AS-IS`

---

# 13. Flujo FLU-09: Autenticación mediante interceptor

## Descripción

Representa el comportamiento transversal asociado al interceptor de autenticación.

## Elementos involucrados

Solicitud HTTP
↓
InterceptorAutenticacion
↓
IHttpContextAccessor
↓
Contexto HTTP
↓
Validación de autenticación

## Comportamiento identificado

`InterceptorAutenticacion` implementa `IInterceptor` y utiliza `IHttpContextAccessor`.

La dependencia hacia `IHttpContextAccessor` permite al interceptor acceder al contexto HTTP durante la ejecución de la solicitud.

El interceptor forma parte del mecanismo transversal de autenticación y no constituye una operación de negocio independiente.

## Diagrama asociado

`SEQ-09_InterceptorAutenticacion_AS-IS`

---

# 14. Flujo FLU-10: Carga inicial

## Descripción

Representa el proceso de inicialización de la aplicación y construcción de las instancias registradas mediante el contenedor de dependencias.

## Estructura identificada

Program
↓
Contenedor DI
↓
Hacienda / servicios registrados
↓
PersistenciaService
↓
Carga de información

## Comportamiento identificado

`Program.cs` registra y consume implementaciones concretas mediante el contenedor de dependencias.

`Hacienda` se registra como `Singleton`, por lo que su estado permanece compartido en memoria durante la vida de la aplicación.

La carga inicial utiliza `PersistenciaService` para reconstruir información.

Durante esta reconstrucción:

* `CargarReses` utiliza `Potrero.anadir_res`;
* dicha operación puede ejecutar nuevamente validaciones y eventos;
* `CargarVentas` genera nuevas instancias de `Res`.

Por tanto, la carga inicial no consiste únicamente en una deserialización pasiva de información, sino que puede activar comportamiento del dominio.

## Diagrama asociado

`SEQ-10_CargaInicial_AS-IS`

---

# 15. Priorización de flujos

| Prioridad | Flujo                        | Justificación                                                                                   |
| --------- | ---------------------------- | ----------------------------------------------------------------------------------------------- |
| Alta      | Autenticación                | Involucra controlador, servicio, autenticación e infraestructura                                |
| Alta      | Registro de res              | Involucra creación de objetos, reglas de dominio, servicios, validación, persistencia y eventos |
| Alta      | Registro de vacuna           | Involucra creación, aplicación de reglas y decisiones según tipos concretos                     |
| Alta      | Registro de venta            | Involucra reglas de dominio, persistencia y eventos                                             |
| Alta      | Validación                   | Permite estudiar interceptores, contratos y responsabilidades                                   |
| Media     | Persistencia                 | Permite analizar persistencia, reconstrucción de estado y dependencia hacia infraestructura     |
| Media     | Eventos                      | Permite analizar el comportamiento de publishers y suscripciones                                |
| Media     | Carga inicial                | Permite documentar inicialización, Singleton y reconstrucción del estado                        |
| Media     | Registro de potrero          | Representa una operación relevante sobre el dominio                                             |
| Media     | Interceptor de autenticación | Representa comportamiento transversal                                                           |

---

# 16. Relación con el diagnóstico arquitectónico

Los flujos de comportamiento serán utilizados posteriormente como evidencia para evaluar el impacto de los problemas arquitectónicos identificados.

Un hallazgo no deberá basarse únicamente en la existencia de una relación estructural.

Cuando sea posible, deberá demostrarse:

Elemento arquitectónico
↓
Dependencia / responsabilidad
↓
Comportamiento observado
↓
Consecuencia
↓
Principio SOLID afectado

Esto permitirá relacionar los diagramas estructurales con los diagramas de comportamiento.

---

# 17. Trazabilidad

| Documento                         | Relación                   |
| --------------------------------- | -------------------------- |
| `Inventario_Clases_AS-IS.md`      | Elementos participantes    |
| `Inventario_Componentes_AS-IS.md` | Componentes                |
| `Matriz_Relaciones_AS-IS.md`      | Relaciones estructurales   |
| `Matriz_Dependencias_AS-IS.md`    | Dependencias               |
| `Matriz_Multiplicidades_AS-IS.md` | Cardinalidades             |
| `Registro_Hallazgos_AS-IS.md`     | Diagnóstico arquitectónico |
| `SEQ-*.drawio`                    | Diagramas de secuencia     |
| `ACT-*.drawio`                    | Diagramas de actividad     |

---

# 18. Criterio para los diagramas de comportamiento

Este documento constituye el inventario de flujos y no reemplaza los diagramas de secuencia o actividad.

Los diagramas deberán representar únicamente las llamadas, decisiones, validaciones, eventos y resultados que puedan trazarse al comportamiento existente.

En particular:

* las decisiones por tipo concreto de `Res` y `Vacuna` deberán aparecer donde realmente intervengan;
* las validaciones deberán representarse en el punto real de ejecución;
* la carga de información deberá mostrar cuando la reconstrucción activa nuevamente lógica de dominio;
* los publishers deberán representarse en el punto donde se ejecuta la operación que los activa;
* las suscripciones deberán representarse de acuerdo con su ubicación real en el flujo;
* no deberán introducirse componentes, abstracciones o pasos correspondientes a una arquitectura propuesta.

El objetivo del modelo AS-IS es representar el sistema actual, incluso cuando dicho comportamiento evidencie problemas de diseño.
