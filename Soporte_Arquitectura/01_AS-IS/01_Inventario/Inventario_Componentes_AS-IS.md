# Inventario de Componentes AS-IS — ANEXO (no parte de la defensa; alcance oficial = biblioteca, ver Inventario_Clases + UML único)

**Proyecto:** Sistema de Gestión de Hacienda
**Versión:** AS-IS 1.1 (corrección: este archivo queda como anexo histórico generado con IA; la evaluación usa el diagrama único de biblioteca)
**Fecha:** 07/08/2026 (rev. 09/2026: aporte propio — Services pertenecen a biblioteca §5, front §4 fuera de alcance)

---

# 1. Objetivo

Identificar los principales componentes que conforman la arquitectura actual del sistema y establecer los elementos que pertenecen a cada uno.

Este inventario servirá como base para representar la estructura arquitectónica AS-IS y sus dependencias.

---

# 2. Componentes identificados

| ID     | Componente              | Tipo                         | Responsabilidad observada                                                 |
|--------|-------------------------|------------------------------|---------------------------------------------------------------------------|
| CMP-01 | `Bib_Hacienda`          | Biblioteca de dominio        | Contener entidades, reglas, eventos, validaciones y contratos del dominio.|
| CMP-02 | `p_mvcHacienda`         | Aplicación MVC               | Proporcionar la interfaz web y coordinar las operaciones del sistema.     |
| CMP-03 | Servicios de aplicación | Capa de servicios            | Coordinar operaciones entre controladores, dominio y persistencia.        |
| CMP-04 | Persistencia            | Infraestructura              | Cargar y almacenar información mediante archivos.                         |
| CMP-05 | Validación              | Componente transversal       | Ejecutar validaciones mediante validadores e interceptores.               |
| CMP-06 | Eventos                 | Componente de comportamiento | Publicar notificaciones derivadas de operaciones del    dominio.          | 
| CMP-07 | Autenticación           | Componente transversal       | Gestionar autenticación y autorización de usuarios.                       |

---

# 3. Componente `Bib_Hacienda`

## Responsabilidad

Contiene los elementos principales del dominio de la aplicación.

## Elementos identificados

Bib_Hacienda
├── Entidades
├── Reglas
├── Eventos
├── Validaciones
├── Interceptores
└── Contratos

## Elementos principales:

- Hacienda
- Potrero
- Res
- Ternero
- Cebon
- Novillo
- Vacuna
- Bacteriana
- Viva
- Venta
- Usuario
- Autenticacion
- Validacion
- ValidadorRes
- ValidadorPotrero
- ValidadorVacuna
- ValidadorVenta
- Publishers de eventos
- Interceptores

---

# 4. Componente p_mvcHacienda

## Responsabilidad

Implementar la aplicación web basada en ASP.NET Core MVC.

## Elementos principales:

p_mvcHacienda
├── Controllers
├── Models
├── Views
├── Servicios
├── Datos
└── Configuración

## Controladores
- AccountController
- HomeController
- PotreroController
- ResController
- UsuarioController
- VacunaController
- VentaController

## ViewModels
- LoginViewModel
- ErrorViewModel

---

# 5. Componente de servicios

Contiene los servicios utilizados por los controladores para ejecutar las operaciones del sistema.

Servicios
├── PersistenciaService
├── PotreroService
├── ResService
├── VacunaService
├── VentaService
└── UsuarioService

Los servicios mantienen dependencias hacia elementos del dominio y hacia PersistenciaService.

---

# 6. Componente de persistencia

Elemento principal

- PersistenciaService

## Responsabilidades observadas

- Cargar información almacenada.
- Guardar información.
- Serializar información.
- Escribir archivos.
- Leer archivos.
- Participar en procesos de validación mediante proxies.

## Medio de almacenamiento
Archivos de texto

La persistencia no se encuentra encapsulada mediante un repositorio o abstracción equivalente identificada en el código analizado.

---

# 7. Componente de validación

Está compuesto principalmente por:

IValidarInformacion
        ▲
        │
   Validacion
        ▲
        │
 ┌──────┼───────────────┐
 ▼      ▼       ▼       ▼
Res  Potrero  Vacuna  Venta

Los procesos de validación son utilizados junto con Castle.DynamicProxy mediante InterceptorValidarInformacion.

---

# 8. Componente de eventos

El sistema utiliza publishers especializados para comunicar determinados acontecimientos del dominio.

Eventos
├── PublisherPesoMin
├── PublisherPesoVenta
├── PublisherPotreroMitad
├── PublisherPotreroLleno
├── PublisherVacunacionCompletada
└── PublisherVacunaVencida

Estos componentes participan en mecanismos de publicación y suscripción mediante eventos.

---

# 9. Componente de autenticación

El flujo de autenticación está compuesto por:

AccountController
        │
        ▼
UsuarioService
        │
        ▼
Usuario
        │
        ▼
Autenticación
        │
        ▼
Cookie Authentication

El sistema utiliza autenticación basada en cookies proporcionada por ASP.NET Core.

---

# 10. Dependencias principales entre componentes


p_mvcHacienda
      │
      ├──────────► Servicios
      │
      ├──────────► Bib_Hacienda
      │
      └──────────► ASP.NET Core
                       │
                       ▼
                  Autenticación


Servicios
      │
      ├──────────► Bib_Hacienda
      │
      └──────────► Persistencia


Bib_Hacienda
      │
      ├──────────► Eventos
      │
      ├──────────► Validaciones
      │
      └──────────► Interceptores

---

# 11. Configuración de dependencias

El contenedor de dependencias de ASP.NET Core registra como Singleton los principales servicios y el objeto Hacienda.

## Elementos registrados:

- PersistenciaService
- Hacienda
- PotreroService
- ResService
- VacunaService
- VentaService
- UsuarioService

Esta configuración forma parte del comportamiento de composición de la aplicación.

---

# 12. Observaciones

El inventario representa la organización existente en el código fuente y no establece todavía una arquitectura objetivo.

**Las relaciones y dependencias concretas serán documentadas posteriormente en:*

02_Relaciones/Matriz_Relaciones_AS-IS.md

**Los posibles problemas derivados de estas dependencias se documentarán separadamente en:*

04_Hallazgos/
