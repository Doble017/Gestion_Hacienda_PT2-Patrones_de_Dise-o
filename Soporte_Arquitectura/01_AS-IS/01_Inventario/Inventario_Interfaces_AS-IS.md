# Inventario de Interfaces AS-IS

**Proyecto:** Sistema de Gestión de Hacienda

**Versión:** AS-IS 1.0

**Fecha:** 07/08/2026

**Propósito:** Identificar y documentar las interfaces existentes en el sistema actual, sus implementaciones y el papel que desempeñan dentro de la arquitectura.

---

# 1. Objetivo

Documentar las interfaces identificadas en el sistema actual, indicando sus operaciones, implementaciones conocidas y contexto de utilización.

Este inventario constituye una fuente para la posterior construcción de la matriz de relaciones y los diagramas UML AS-IS.

---

# 2. Alcance

El inventario comprende las interfaces identificadas en los módulos:

- `Bib_Hacienda`
- `p_mvcHacienda`
- Frameworks y librerías externas cuando su participación sea arquitectónicamente relevante.

No se consideran como interfaces propias del sistema los contratos internos del framework que solamente sean utilizados de manera incidental.

---

# 3. Interfaces identificadas

## 3.1. IValidarInformacion

| Campo                        | Información                                       |
|------------------------------|---------------------------------------------------|
| Identificador                | INT-01                                            |
| Nombre                       | `IValidarInformacion`                             |
| Módulo                       | `Bib_Hacienda`                                    |
| Tipo                         | Interfaz de dominio/aplicación                    |
| Implementaciones principales | `Validacion`                                      |
| Propósito                    | Definir las operaciones de validación disponibles |

### Operaciones

La interfaz establece cuatro operaciones:

ValidarRes()
ValidarPotrero()
ValidarVacuna()
ValidarVenta()

Estas operaciones constituyen el contrato general de validación utilizado por la jerarquía Validacion.

## Relación con otras clases

<<interface>>
IValidarInformacion
          ▲
          │ realiza
          │
     <<abstract>>
      Validacion

Las clases concretas de validación heredan posteriormente de Validacion. 

---

# 4. Jerarquía asociada a IValidarInformacion

## La estructura observada es:

                 <<interface>>
                IValidarInformacion
                         ▲
                         │
                 implementación
                         │
                    <<abstract>>
                     Validacion
                         ▲
                         │
   ┌─────────────────────┼──────────────────────────┬────────────────────────┐
   │                     │                          │                        │
   ▼                     ▼                          ▼                        ▼
ValidadorRes        ValidadorPotrero            ValidadorVacuna         ValidadorVenta



La clase abstracta Validacion implementa IValidarInformacion.

Las clases concretas heredan de Validacion.

---

# 5. Interfaces utilizadas por autenticación

## 5.1  IAutenticacion

| Campo          | Información                                       |
| -------------- | ------------------------------------------------- |
| Identificador  | INT-02                                            |
| Nombre         | `IAutenticacion`                                  |
| Módulo         | `Bib_Hacienda`                                    |
| Implementación | `Autenticacion`                                   |
| Propósito      | Definir el contrato relacionado con autenticación |

## La implementación concreta identificada es:

Autenticacion
      │
      │ realiza
      ▼
IAutenticacion

La interfaz forma parte del mecanismo de autenticación utilizado por el sistema.

---

# 6. Interfaces externas relevantes

## 6.1 IInterceptor

| Campo                          | Información                                                 |
| ------------------------------ | ----------------------------------------------------------- |
| Identificador                  | EXT-01                                                      |
| Nombre                         | `IInterceptor`                                              |
| Proveedor                      | `Castle.DynamicProxy`                                       |
| Implementaciones en el sistema | `InterceptorAutenticacion`, `InterceptorValidarInformacion` |
| Propósito                      | Definir el contrato de interceptación                       |


## La relación observada es:

<<interface>>
IInterceptor
      ▲
      │
      ├─────────────────────────────────────────────┐
      │                                             │
      │                                             │
InterceptorAutenticacion                InterceptorValidarInformacion


## 6.2. IHttpContextAccessor

| Campo         | Información                                                 |
| ------------- | ----------------------------------------------------------- |
| Identificador | EXT-02                                                      |
| Nombre        | `IHttpContextAccessor`                                      |
| Proveedor     | ASP.NET Core                                                |
| Utilizado por | `InterceptorAutenticacion`, `InterceptorValidarInformacion` |
| Propósito     | Acceder al contexto HTTP actual                             |

## La relación observada es de dependencia:

InterceptorAutenticacion
          │
          ▼
IHttpContextAccessor


InterceptorValidarInformacion
          │
          ▼
IHttpContextAccessor

No se considera una interfaz propia del dominio del sistema.

## 6.3. ILogger<T>

| Campo         | Información                       |
| ------------- | --------------------------------- |
| Identificador | EXT-03                            |
| Nombre        | `ILogger<T>`                      |
| Proveedor     | Microsoft.Extensions.Logging      |
| Utilizado por | `HomeController`                  |
| Propósito     | Registro de información y errores |

HomeController recibe ILogger<HomeController> mediante inyección de dependencias.

## La relación es:

HomeController
      │
      ▼
ILogger<HomeController>

---

# 7. Resumen de interfaces

| ID     | Interfaz               | Origen              | Implementaciones/uso                                        |
| ------ | ---------------------- | ------------------- | ----------------------------------------------------------- |
| INT-01 | `IValidarInformacion`  | Sistema             | `Validacion`                                                |
| INT-02 | `IAutenticacion`       | Sistema             | `Autenticacion`                                             |
| EXT-01 | `IInterceptor`         | Castle DynamicProxy | `InterceptorAutenticacion`, `InterceptorValidarInformacion` |
| EXT-02 | `IHttpContextAccessor` | ASP.NET Core        | Dependencia de interceptores                                |
| EXT-03 | `ILogger<T>`           | .NET                | Dependencia de `HomeController`                             |


---

# 8. Consideraciones para el UML AS-IS

Las interfaces propias del sistema deberán representarse explícitamente en los diagramas UML.

Se distinguirán de las interfaces externas mediante el estereotipo o agrupación correspondiente.

La relación de implementación se representará mediante:

<<interface>>
Interfaz
    △
    ┆
Clase

Las dependencias hacia interfaces externas se representarán como dependencias y no como asociaciones de dominio.

---

# 9. Trazabilidad

| Documento                         | Relación                                 |
| --------------------------------- | ---------------------------------------- |
| `Inventario_Clases_AS-IS.md`      | Clases que implementan/heredan contratos |
| `Inventario_Componentes_AS-IS.md` | Ubicación arquitectónica                 |
| `Matriz_Relaciones_AS-IS.md`      | Relaciones de realización y dependencia  |
| `Hallazgos_SOLID.md`              | Análisis posterior de contratos          |
| `UML_AS-IS_*.drawio`              | Representación gráfica                   |



### Una precisión importante

En este archivo estamos separando deliberadamente **interfaces propias** de **interfaces externas**. `IInterceptor`, `IHttpContextAccessor` e `ILogger<T>` aparecen porque afectan las dependencias arquitectónicas, pero no debemos contaminar el modelo de dominio haciéndolas parecer parte de él.

También he mantenido `IValidarInformacion` y `Validacion` como elementos distintos: la primera es el contrato, mientras que la segunda es la clase abstracta que lo implementa. Esto será especialmente importante cuando construyamos la matriz de relaciones, porque ahí tendremos que registrar **realización** y **generalización** como relaciones diferentes.

El siguiente archivo será **`Inventario_Componentes_AS-IS.md`**, donde dejaremos de mirar solamente clases individuales y empezaremos a documentar los bloques arquitectónicos que forman el sistema.