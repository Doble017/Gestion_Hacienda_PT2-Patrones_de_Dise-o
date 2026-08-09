# Inventario de Clases AS-IS

**Proyecto:** Sistema de Gestión de Hacienda

**Versión:** AS-IS 1.0

**Fecha:** 07/08/2026

**Responsables:**
- Arquitectura de dominio
- Ingeniería de comportamiento

---

# 1. Objetivo

Documentar las clases identificadas en el sistema actual, registrando su ubicación, tipo, responsabilidad observada y relaciones principales, con el fin de construir la línea base arquitectónica (AS-IS).

---

# 2. Alcance

Este documento abarca los siguientes módulos:

- Bib_Hacienda
- p_mvcHacienda

---

# 3. Criterios de clasificación

Se utilizarán las siguientes categorías:

| Categoría     | Descripción                               |
|---------------|-------------------------------------------|
| Entidad       | Elemento principal del dominio            |
| Interfaz      | Contrato de comportamiento                |
| Servicio      | Implementación de lógica de aplicación    |
| Controlador   | Componente de la capa MVC                 |
| Publicador    | Componente del sistema de eventos         |
| Validador     | Componente de validación                  |    
| Interceptor   | Componente transversal                    |
| Regla         | Contenedor de reglas de negocio           |
| ViewModel     | Modelo orientado a la presentación        |
| Auxiliar      | Clase de soporte                          |

---

# 4. Inventario de clases

## 4.1. Dominio

| ID     | Clase         | Tipo                | Responsabilidad                          |
|--------|---------------|---------------------|------------------------------------------|
| DOM-01 | Hacienda      | Entidad             | Gestionar el estado general del sistema. |
| DOM-02 | Potrero       | Entidad             | Administrar conjuntos de reses.          |
| DOM-03 | Res           | Entidad abstracta   | Representar una res genérica.            |
| DOM-04 | Ternero       | Entidad             | Especialización de Res.                  |
| DOM-05 | Cebon         | Entidad             | Especialización de Res.                  |
| DOM-06 | Novillo       | Entidad             | Especialización de Res.                  |
| DOM-07 | Vacuna        | Entidad abstracta   | Representar vacunas del sistema.         |
| DOM-08 | Bacteriana    | Entidad             | Especialización de Vacuna.               |
| DOM-09 | Viva          | Entidad             | Especialización de Vacuna.               |
| DOM-10 | Venta         | Entidad             | Gestionar procesos de venta.             |
| DOM-11 | Usuario       | Entidad             | Gestionar usuarios del sistema.          |
| DOM-12 | Autenticacion | Servicio de dominio | Gestionar autenticación.                 |

---

## 4.2. Reglas

| ID     | Clase        | Tipo            | Responsabilidad                                 |
|--------|--------------|-----------------|-------------------------------------------------|
| REG-01 | ReglaRes     | Clase abstracta | Centralizar constantes asociadas a las reses.   |
| REG-02 | ReglaPotrero | Clase abstracta | Centralizar reglas asociadas a potreros.        |
| REG-03 | ReglaVacuna  | Clase abstracta | Centralizar reglas de vacunación.               |

---

## 4.3. Eventos

| ID     | Clase                         | Tipo       | Responsabilidad                   |
|--------|-------------------------------|------------|-----------------------------------|
| EVT-01 | PublisherPesoMin              | Publicador | Notificar pesos mínimos.          |
| EVT-02 | PublisherPesoVenta            | Publicador | Notificar pesos de venta.         |
| EVT-03 | PublisherPotreroMitad         | Publicador | Notificar ocupación intermedia.   |
| EVT-04 | PublisherPotreroLleno         | Publicador | Notificar ocupación máxima.       |
| EVT-05 | PublisherVacunacionCompletada | Publicador | Notificar vacunación completa.    |
| EVT-06 | PublisherVacunaVencida        | Publicador | Notificar vencimiento de vacunas. |

---

## 4.4. Validaciones

| ID     | Clase            | Tipo            | Responsabilidad                    |
|--------|------------------|-----------------|------------------------------------|
| VAL-01 | Validacion       | Clase abstracta | Definir el contrato de validación. |
| VAL-02 | ValidadorRes     | Validador       | Validar información de reses.      |
| VAL-03 | ValidadorPotrero | Validador       | Validar información de potreros.   |
| VAL-04 | ValidadorVacuna  | Validador       | Validar información de vacunas.    |
| VAL-05 | ValidadorVenta   | Validador       | Validar información de ventas.     |

---

## 4.5. Interceptores

| ID     | Clase                         | Tipo        |   Responsabilidad                      |
|--------|-------------------------------|-------------|----------------------------------------|
| INT-01 | InterceptorAutenticacion      | Interceptor | Interceptar procesos de autenticación. |
| INT-02 | InterceptorValidarInformacion | Interceptor | Interceptar procesos de validación.    |

---

## 4.6. Servicios

| ID     | Clase               | Tipo     | Responsabilidad                                |
|--------|---------------------|----------|------------------------------------------------|
| SER-01 | PersistenciaService | Servicio | Gestionar persistencia y carga de información. |
| SER-02 | PotreroService      | Servicio | Gestionar operaciones sobre potreros.          |
| SER-03 | ResService          | Servicio | Gestionar operaciones sobre reses.             |
| SER-04 | VacunaService       | Servicio | Gestionar operaciones sobre vacunas.           |
| SER-05 | VentaService        | Servicio | Gestionar operaciones sobre ventas.            |
| SER-06 | UsuarioService      | Servicio | Gestionar autenticación y usuarios.            |

---

## 4.7. Controladores

| ID     | Clase             | Tipo        |
|--------|-------------------|-------------|
| CTR-01 | HomeController    | Controlador |
| CTR-02 | AccountController | Controlador |
| CTR-03 | PotreroController | Controlador |
| CTR-04 | ResController     | Controlador |
| CTR-05 | UsuarioController | Controlador |
| CTR-06 | VacunaController  | Controlador |
| CTR-07 | VentaController   | Controlador |

---

## 4.8. ViewModels

| ID    | Clase          | Tipo      |
|-------|----------------|-----------|
| VM-01 | LoginViewModel | ViewModel |
| VM-02 | ErrorViewModel | ViewModel |

---

# 5. Observaciones

- El sistema está dividido en una biblioteca de dominio (`Bib_Hacienda`) y una aplicación MVC (`p_mvcHacienda`).

- Los componentes de validación utilizan interceptores implementados mediante `Castle.DynamicProxy`.

- El sistema emplea un mecanismo de publicación y suscripción basado en eventos de C#.

- El estado principal del sistema es administrado por la clase `Hacienda`.

- El almacenamiento de la información se realiza mediante archivos de texto.

---

# 6. Trazabilidad

| Documento                       | Relación    |
|---------------------------------|-------------|
| Inventario_Interfaces_AS-IS.md  | Contratos   |
| Inventario_Componentes_AS-IS.md | Componentes |
| Matriz_Relaciones_AS-IS.md      | Relaciones  |
| Registro_Hallazgos_AS-IS.md     | Hallazgos   |
| UML_AS-IS_*.drawio              | Diagramas   |