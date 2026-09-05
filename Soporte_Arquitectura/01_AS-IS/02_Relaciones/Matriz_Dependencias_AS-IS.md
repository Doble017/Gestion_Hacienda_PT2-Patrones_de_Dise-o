# Matriz de Dependencias AS-IS

**Proyecto:** Sistema de Gestión de Hacienda  
**Versión:** AS-IS 1.0  
**Fecha:** 07/08/2026

---

# 1. Objetivo

Documentar las dependencias existentes entre los principales elementos del sistema actual, identificando el sentido de cada dependencia y el elemento concreto del que depende.

La matriz permitirá analizar posteriormente el nivel de acoplamiento y las dependencias relevantes para el diagnóstico arquitectónico basado principalmente en los principios SOLID.

---

# 2. Criterios de clasificación

Una dependencia se registra cuando un componente:

- mantiene una referencia hacia otro componente;
- utiliza una implementación concreta;
- instancia directamente otro elemento;
- requiere un servicio externo;
- utiliza infraestructura directamente;
- depende de un contrato o framework para ejecutar una operación.

La matriz describe exclusivamente el estado actual del sistema y no determina todavía si una dependencia constituye una falla de diseño.

---

# 3. Dependencias de los controladores

| ID         | Origen              | Destino                   | Naturaleza      |
|------------|---------------------|---------------------------|-----------------|
| DEP-CTR-01 | `AccountController` | `UsuarioService`          | Servicio        |
| DEP-CTR-02 | `HomeController`    | `ILogger<HomeController>` | Infraestructura |
| DEP-CTR-03 | `PotreroController` | `PotreroService`          | Servicio        |
| DEP-CTR-04 | `PotreroController` | `Hacienda`                | Dominio         |
| DEP-CTR-05 | `PotreroController` | `PersistenciaService`     | Persistencia    |
| DEP-CTR-06 | `ResController`     | `ResService`              | Servicio        |
| DEP-CTR-07 | `ResController`     | `PotreroService`          | Servicio        | 
| DEP-CTR-08 | `ResController`     | `Hacienda`                | Dominio         |
| DEP-CTR-09 | `ResController`     | `PersistenciaService`     | Persistencia    |
| DEP-CTR-10 | `UsuarioController` | `UsuarioService`          | Servicio        |
| DEP-CTR-11 | `VacunaController`  | `VacunaService`           | Servicio        |
| DEP-CTR-12 | `VacunaController`  | `ResService`              | Servicio        |
| DEP-CTR-13 | `VacunaController`  | `PotreroService`          | Servicio        |
| DEP-CTR-14 | `VentaController`   | `VentaService`            | Servicio        |

---

# 4. Dependencias de los servicios

| ID         | Origen           | Destino               | Naturaleza   |
|------------|------------------|-----------------------|--------------|
| DEP-SER-01 | `PotreroService` | `Hacienda`            | Dominio      |
| DEP-SER-02 | `PotreroService` | `PersistenciaService` | Persistencia |
| DEP-SER-03 | `ResService`     | `Hacienda`            | Dominio      |
| DEP-SER-04 | `ResService`     | `PersistenciaService` | Persistencia |
| DEP-SER-05 | `VacunaService`  | `Hacienda`            | Dominio      |
| DEP-SER-06 | `VacunaService`  | `PersistenciaService` | Persistencia |
| DEP-SER-07 | `VentaService`   | `Hacienda`            | Dominio      |
| DEP-SER-08 | `VentaService`   | `PersistenciaService` | Persistencia |
| DEP-SER-09 | `UsuarioService` | `PersistenciaService` | Persistencia |

---

# 5. Dependencias de persistencia

`PersistenciaService` presenta dependencias hacia múltiples elementos del sistema y de infraestructura.

| ID         | Origen                | Destino                | Naturaleza      |
|------------|-----------------------|------------------------|-----------------|
| DEP-PER-01 | `PersistenciaService` | `ValidadorRes`         | Validación      |
| DEP-PER-02 | `PersistenciaService` | `ValidadorPotrero`     | Validación      |
| DEP-PER-03 | `PersistenciaService` | `ValidadorVacuna`      | Validación      |
| DEP-PER-04 | `PersistenciaService` | `ValidadorVenta`       | Validación      |
| DEP-PER-05 | `PersistenciaService` | Sistema de archivos    | Infraestructura |
| DEP-PER-06 | `PersistenciaService` | `Castle.DynamicProxy`  | Infraestructura |
| DEP-PER-07 | `PersistenciaService` | `IHttpContextAccessor` | Framework       |

---

# 6. Dependencias de validación

| ID         | Origen             | Destino               | Naturaleza |
|------------|--------------------|-----------------------|------------|
| DEP-VAL-01 | `ValidadorRes`     | `IValidarInformacion` | Contrato   |
| DEP-VAL-02 | `ValidadorPotrero` | `IValidarInformacion` | Contrato   |
| DEP-VAL-03 | `ValidadorVacuna`  | `IValidarInformacion` | Contrato   |
| DEP-VAL-04 | `ValidadorVenta`   | `IValidarInformacion` | Contrato   |

Las clases concretas pertenecen a la jerarquía derivada de `Validacion`.

---

# 7. Dependencias de interceptores

| ID         | Origen                          | Destino                | Naturaleza       |
|------------|---------------------------------|------------------------|------------------|
| DEP-INT-01 | `InterceptorAutenticacion`      | `IInterceptor`         | Contrato externo |
| DEP-INT-02 | `InterceptorAutenticacion`      | `IHttpContextAccessor` | Framework        |
| DEP-INT-03 | `InterceptorValidarInformacion` | `IInterceptor`         | Contrato externo |
| DEP-INT-04 | `InterceptorValidarInformacion` | `IHttpContextAccessor` | Framework        |

---

# 8. Dependencias de autenticación

El flujo de autenticación presenta las siguientes dependencias:

| ID         | Origen              | Destino                | Naturaleza    |
|------------|---------------------|------------------------|---------------|
| DEP-AUT-01 | `AccountController` | `UsuarioService`       | Servicio      |
| DEP-AUT-02 | `UsuarioService`    | `Usuario`              | Dominio       |
| DEP-AUT-03 | `UsuarioService`    | `PersistenciaService`  | Persistencia  |
| DEP-AUT-04 | `Autenticacion`     | `IAutenticacion`       | Contrato      |

---

# 9. Dependencias de composición de la aplicación

`Program.cs` participa en la construcción del grafo de dependencias mediante el contenedor de ASP.NET Core.

Los principales elementos registrados como `Singleton` son:

| Componente            | Ciclo de vida |
|-----------------------|---------------|
| `PersistenciaService` | Singleton     |
| `Hacienda`            | Singleton     |
| `PotreroService`      | Singleton     |
| `ResService`          | Singleton     |
| `VacunaService`       | Singleton     |
| `VentaService`        | Singleton     |
| `UsuarioService`      | Singleton     |

Durante la creación de `Hacienda`, se utiliza `PersistenciaService` para cargar información inicial.

Por tanto, el proceso de composición presenta la siguiente secuencia:

`Program` → `Hacienda` → `PersistenciaService`

Esta relación corresponde al proceso de construcción de la aplicación y no debe interpretarse automáticamente como una asociación permanente entre las clases.

---

# 10. Dependencias hacia frameworks

| ID        | Origen                            | Dependencia                   |
|-----------|-----------------------------------|-------------------------------|
| DEP-FW-01 | Controllers                       | ASP.NET Core MVC              |
| DEP-FW-02 | `AccountController`               | Autenticación mediante cookies|
| DEP-FW-03 | `HomeController`                  | `ILogger<T>`                  |
| DEP-FW-04 | Interceptores                     | `Castle.DynamicProxy`         |
| DEP-FW-05 | `InterceptorAutenticacion`        | `IHttpContextAccessor`        |
| DEP-FW-06 | `InterceptorValidarInformacion`   | `IHttpContextAccessor`        |

---

# 11. Vista resumida de dependencias

## Presentación

`Controllers → Services`

`Controllers → Hacienda`

`Controllers → PersistenciaService`

## Aplicación

`Services → Hacienda`

`Services → PersistenciaService`

## Persistencia

`PersistenciaService → Validadores`

`PersistenciaService → Filesystem`

`PersistenciaService → DynamicProxy`

`PersistenciaService → HttpContext`

## Validación

`Validadores → IValidarInformacion`

## Autenticación

`AccountController → UsuarioService → Usuario`

---

# 12. Consideración para el diagnóstico SOLID

Esta matriz no establece por sí misma una violación de SOLID.

Las dependencias documentadas serán utilizadas posteriormente para identificar:

- dependencias hacia implementaciones concretas;
- acoplamiento entre capas;
- responsabilidades cruzadas;
- dificultad para extender funcionalidades;
- contratos con responsabilidades no relacionadas;
- posibles problemas de sustitución;
- dependencias directas hacia infraestructura.

En particular, las dependencias de los controladores directamente hacia `Hacienda` y `PersistenciaService` serán relevantes para el análisis posterior de separación de responsabilidades y Dependency Inversion Principle (DIP).

Las dependencias de los servicios serán evaluadas según la responsabilidad concreta que ejerzan dentro del sistema.

Corrección punto 12 (pedida en clase — Singleton): `Singleton` aquí es **ciclo de vida de DI en `Program.cs`** (una sola instancia de `Hacienda`, `PersistenciaService` y Services registrada con `AddSingleton`), NO patrón GoF Singleton con `Instance`/`lock` en `Hacienda`. Evidencia: §9 tabla de registros + secuencia `Program → Hacienda → PersistenciaService` en arranque. No se afirma Singleton GoF en el negocio. Su impacto (estado global compartido, dificulta pruebas y extiende God Class H-03/H-12) se evalúa solo con esa evidencia.

---

# 13. Trazabilidad

| Documento                         | Uso                           |
|-----------------------------------|-------------------------------|
| `Inventario_Clases_AS-IS.md`      | Identificación de elementos   |
| `Inventario_Interfaces_AS-IS.md`  | Contratos                     |
| `Matriz_Relaciones_AS-IS.md`      | Relaciones UML                |
| `Matriz_Dependencias_AS-IS.md`    | Dependencias arquitectónicas  |
| `Registro_Hallazgos_AS-IS.md`     | Diagnóstico SOLID             |
| Diagramas UML AS-IS               | Representación gráfica        |