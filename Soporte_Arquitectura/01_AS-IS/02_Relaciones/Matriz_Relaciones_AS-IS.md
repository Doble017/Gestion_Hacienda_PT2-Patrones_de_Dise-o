# Matriz de Relaciones AS-IS

**Proyecto:** Sistema de Gestión de Hacienda  
**Versión:** AS-IS 1.0  
**Fecha:** 07/08/2026

---

# 1. Objetivo

Documentar las relaciones existentes entre los elementos identificados en el sistema actual, estableciendo el tipo de relación UML correspondiente y su evidencia en el código fuente.

Esta matriz constituye la fuente de referencia para la construcción de los diagramas UML AS-IS.

---

# 2. Criterios de clasificación

Las relaciones se clasifican utilizando la siguiente correspondencia:

| Tipo                           | Uso                                                       |
|--------------------------------|-----------------------------------------------------------|
| Generalización                 | Una clase hereda de otra clase                            |
| Realización                    | Una clase implementa una interfaz                         |
| Asociación                     | Una clase mantiene una referencia estructural a otra      |    
| Agregación                     | Relación todo-parte con independencia del ciclo de vida   |
| Composición                    | Relación todo-parte con dependencia del ciclo de vida     |
| Dependencia                    | Un elemento utiliza temporalmente otro                    |
| Creación                       | Un elemento crea instancias de otro                       |
| Dependencia de infraestructura | Uso directo de un componente externo o de infraestructura |

> Las relaciones de agregación y composición únicamente se utilizarán cuando el código permita justificar la existencia de una relación todo-parte y su correspondiente semántica de ciclo de vida.

---

# 3. Relaciones de generalización

## 3.1. Jerarquía de reses

| ID     | Superclase | Subclase  | Relación       | Evidencia                 |
|--------|------------|-----------|----------------|---------------------------|
| GEN-01 | `Res`      | `Ternero` | Generalización | `Ternero` hereda de `Res` |
| GEN-02 | `Res`      | `Cebon`   | Generalización | `Cebon` hereda de `Res`   |
| GEN-03 | `Res`      | `Novillo` | Generalización | `Novillo` hereda de `Res` |

Representación:


              <<abstract>>
                  Res
                   △
          ┌────────┼────────┐
          │        │        │
       Ternero   Cebon    Novillo


## 3.2. Jerarquía de vacunas

| ID     | Superclase | Subclase     | Relación       | Evidencia                       |
| ------ | ---------- | ------------ | -------------- | ------------------------------- |
| GEN-04 | `Vacuna`   | `Bacteriana` | Generalización | `Bacteriana` hereda de `Vacuna` |
| GEN-05 | `Vacuna`   | `Viva`       | Generalización | `Viva` hereda de `Vacuna`       |


## Representación:

              <<abstract>>
                 Vacuna
                   △
                ┌──┴──┐
                │     │
           Bacteriana  Viva

## 3.3. Jerarquía de validaciones

| ID     | Superclase   | Subclase           | Relación       |
| ------ | ------------ | ------------------ | -------------- |
| GEN-06 | `Validacion` | `ValidadorRes`     | Generalización |
| GEN-07 | `Validacion` | `ValidadorPotrero` | Generalización |
| GEN-08 | `Validacion` | `ValidadorVacuna`  | Generalización |
| GEN-09 | `Validacion` | `ValidadorVenta`   | Generalización |


---

# 4. Relaciones de realización

## 4.1. Contrato de validación

| ID      | Interfaz              | Implementación | Relación    |
| ------- | --------------------- | -------------- | ----------- |
| REAL-01 | `IValidarInformacion` | `Validacion`   | Realización |

## Representación:

<<interface>>
IValidarInformacion
          △
          ┆
          ┆
     <<abstract>>
      Validacion

Las clases concretas de validación heredan posteriormente de Validacion.

# 4.2. Contrato de autenticación

| ID      | Interfaz         | Implementación  | Relación    |
| ------- | ---------------- | --------------- | ----------- |
| REAL-02 | `IAutenticacion` | `Autenticacion` | Realización |


## 4.3. Contratos implementados por Hacienda

| ID      | Interfaz          | Implementación | Relación    |
| ------- | ----------------- | -------------- | ----------- |
| REAL-03 | `IVacunacion`     | `Hacienda`     | Realización |
| REAL-04 | `IVentaRes`       | `Hacienda`     | Realización |
| REAL-05 | `ICreacionVacuna` | `Hacienda`     | Realización |


## 4.4. Interceptores

Las clases interceptoras implementan el contrato proporcionado por Castle.DynamicProxy.

| ID      | Interfaz       | Implementación                  | Relación    |
| ------- | -------------- | ------------------------------- | ----------- |
| REAL-06 | `IInterceptor` | `InterceptorAutenticacion`      | Realización |
| REAL-07 | `IInterceptor` | `InterceptorValidarInformacion` | Realización |

---

# 5. Relaciones de asociación

Las siguientes relaciones se clasifican como asociaciones cuando el código mantiene una referencia estructural entre los elementos involucrados. La clasificación se basa en referencias almacenadas como atributos o dependencias conservadas durante el ciclo de vida del componente.

## 5.1. Controladores y servicios

| ID     | Origen              | Destino               | Relación   |
| ------ | ------------------- | --------------------- | ---------- |
| ASO-01 | `AccountController` | `UsuarioService`      | Asociación |
| ASO-02 | `PotreroController` | `PotreroService`      | Asociación |
| ASO-03 | `PotreroController` | `Hacienda`            | Asociación |
| ASO-04 | `PotreroController` | `PersistenciaService` | Asociación |
| ASO-05 | `ResController`     | `ResService`          | Asociación |
| ASO-06 | `ResController`     | `PotreroService`      | Asociación |
| ASO-07 | `ResController`     | `Hacienda`            | Asociación |
| ASO-08 | `ResController`     | `PersistenciaService` | Asociación |
| ASO-09 | `UsuarioController` | `UsuarioService`      | Asociación |
| ASO-10 | `VacunaController`  | `VacunaService`       | Asociación |
| ASO-11 | `VacunaController`  | `ResService`          | Asociación |
| ASO-12 | `VacunaController`  | `PotreroService`      | Asociación |
| ASO-13 | `VentaController`   | `VentaService`        | Asociación |

## 5.2. Servicios y dominio

| ID     | Origen           | Destino    | Relación   |
| ------ | ---------------- | ---------- | ---------- |
| ASO-14 | `PotreroService` | `Hacienda` | Asociación |
| ASO-15 | `ResService`     | `Hacienda` | Asociación |
| ASO-16 | `VacunaService`  | `Hacienda` | Asociación |
| ASO-17 | `VentaService`   | `Hacienda` | Asociación |
| ASO-18 | `UsuarioService` |  `Usuario` | Asociación |

## 5.3. Servicios y persistencia

| ID     | Origen           | Destino               | Relación   |
| ------ | ---------------- | --------------------- | ---------- |
| ASO-19 | `PotreroService` | `PersistenciaService` | Asociación |
| ASO-20 | `ResService`     | `PersistenciaService` | Asociación |
| ASO-21 | `VacunaService`  | `PersistenciaService` | Asociación |
| ASO-22 | `VentaService`   | `PersistenciaService` | Asociación |
| ASO-23 | `UsuarioService` | `PersistenciaService` | Asociación |


---

# 6. Relaciones de creación

Se registran como relaciones de creación aquellas donde el código crea explícitamente una instancia.

| ID     | Creador               | Elemento creado                  | Relación  |
| ------ | --------------------- | -------------------------------- | ----------|
| CRE-01 | `HomeController`      | `ErrorViewModel`                 | Creación  |
| CRE-02 | `PersistenciaService` | objetos de dominio durante carga | Creación  |
| CRE-03 | `Program`             | `Hacienda`                       | Creación  |

Estas relaciones deben distinguirse de las asociaciones permanentes.

---

# 7. Relaciones entre dominio y eventos

El sistema utiliza mecanismos de eventos para comunicar determinadas situaciones del dominio.

Las clases Hacienda y Potrero contienen publishers asociados a eventos del dominio. Debido a que estas relaciones se materializan mediante mecanismos de suscripción y ejecución de eventos, su representación detallada se documentará en los diagramas de comportamiento.

En esta matriz estructural no se establecerán asociaciones adicionales entre entidades, publishers y reglas mientras no exista una referencia estructural permanente que justifique dicha clasificación.

La identificación de quién publica, quién se suscribe y qué operación se ejecuta será desarrollada posteriormente en los diagramas de secuencia y actividad.


---

# 8. Dependencias de persistencia y validación

PersistenciaService utiliza los validadores durante operaciones específicas de persistencia. Estas referencias corresponden a usos puntuales y no constituyen asociaciones estructurales permanentes.

| ID     | Origen                | Destino              | Relación    |
| ------ | --------------------- | -------------------- | ----------- |
| DEP-01 | `PersistenciaService` | `ValidadorRes`       | Dependencia |
| DEP-02 | `PersistenciaService` | `ValidadorPotrero`   | Dependencia |
| DEP-03 | `PersistenciaService` | `ValidadorVacuna`    | Dependencia |
| DEP-04 | `PersistenciaService` | `ValidadorVenta`     | Dependencia |

---

# 9. Relaciones con infraestructura

| ID     | Origen                          | Destino                   | Relación    |
| ------ | ------------------------------- | ------------------------- | ----------- |
| INF-01 | `PersistenciaService`           | Sistema de archivos       | Dependencia |
| INF-02 | `InterceptorAutenticacion`      | `IHttpContextAccessor`    | Asociación  |
| INF-03 | `InterceptorValidarInformacion` | `IHttpContextAccessor`    | Asociación  |
| INF-04 | `HomeController`                | `ILogger<HomeController>` | Dependencia |


---

# 10. Relaciones de composición y agregación

No se registran relaciones de composición o agregación únicamente por existir colecciones dentro de las clases.

La existencia de una colección no implica por sí misma composición UML.

Antes de establecer una relación de este tipo deberá verificarse:

- Propiedad de las partes.
- Dependencia de ciclo de vida.
- Forma de creación de los objetos.
- Forma de eliminación de los objetos.
- Evidencia suficiente en el código.

Resultado de la auditoría: no se identificaron relaciones que justifiquen formalmente el uso de agregación o composición en el modelo AS-IS.

Aunque existen colecciones y creación directa de objetos en algunas clases, la evidencia disponible no permite afirmar de manera suficiente una semántica UML de propiedad de las partes y dependencia estricta de ciclo de vida.

Por esta razón, las relaciones Hacienda → Potrero, Potrero → Res, Hacienda → Venta, Hacienda → Vacuna y Res → Vacuna se representan como asociaciones.

No se utilizarán rombos de agregación o composición para estas relaciones.

---

# 11. Decisiones metodológicas verificadas


Durante la auditoría del código fuente se revisaron las relaciones que inicialmente habían quedado pendientes de clasificación.

Las relaciones entre entidades del dominio que se sustentan mediante referencias estructurales o colecciones se clasifican como asociaciones. La existencia de una colección o la creación directa de un objeto no se considera evidencia suficiente para establecer agregación o composición.

Las multiplicidades se determinan a partir de los atributos y colecciones realmente presentes en las clases. Cuando no existe una referencia inversa en el código, no se infiere una relación navegable en sentido contrario, Las multiplicidades se documentan únicamente en la Matriz de Multiplicidades AS-IS; esta matriz establece el tipo de relación y su navegabilidad, pero no sustituye la evidencia de cardinalidad.

Las relaciones de realización corresponden exclusivamente a implementaciones explícitas de interfaces.

Las dependencias representan usos puntuales de elementos que no son conservados como referencias estructurales.

El uso de Singleton mediante inyección de dependencias se documenta como una característica arquitectónica del sistema y no como una relación de composición UML.

---

# 12. Criterio para el diagnóstico SOLID

Esta matriz representa exclusivamente el estado actual.

Las relaciones identificadas serán utilizadas posteriormente para detectar:

1. responsabilidades distribuidas incorrectamente;
2. dependencias concretas;
3. acoplamiento excesivo;
4. contratos demasiado amplios;
5. jerarquías que puedan presentar problemas de sustitución;
6. componentes que requieran modificación ante nuevas funcionalidades.

Los diagnósticos SOLID se documentarán posteriormente y no forman parte de esta matriz.

---

# 13. Trazabilidad

| Documento                          | Uso                                                                                          |
| ---------------------------------- | -------------------------------------------------------------------------------------------- |
| `Inventario_Clases_AS-IS.md`       | Identificación de elementos                                                                  |
| `Inventario_Interfaces_AS-IS.md`   | Contratos e implementaciones                                                                 |
| `Matriz_Relaciones_AS-IS.md`       | Relaciones estructurales                                                                     |
| `Matriz_Dependencias_AS-IS.md`     | Dependencias arquitectónicas                                                                 |
| `Registro_Hallazgos_AS-IS.md`      | Diagnóstico                                                                                  |
| `UML_AS-IS_*.drawio`               | Representación gráfica                                                                       |
| 00_Trazabilidad_Auditoria_AS-IS.md | Correspondencia entre código fuente, relaciones identificadas y decisiones de clasificación  |
