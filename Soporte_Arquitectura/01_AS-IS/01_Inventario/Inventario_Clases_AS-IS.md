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

# 3. Criterios de clasificación (corrección: sin términos dominio/entidad DDD; alcance = biblioteca Bib_Hacienda)

Se utilizarán las siguientes categorías:

| Categoría     | Descripción                               |
|---------------|-------------------------------------------|
| Clase negocio | Clase principal de la biblioteca          |
| Clase negocio abstracta | Clase base de la biblioteca        |
| Interfaz      | Contrato de comportamiento                |
| Servicio biblioteca | Lógica de la propia biblioteca      |
| Publicador    | Componente del sistema de eventos         |
| Validador     | Componente de validación                  |
| Interceptor   | Componente transversal                    |
| Regla         | Contenedor de reglas de negocio           |

> Nota corrección: los términos "dominio/entidad" de la v1 se reemplazan por "clase de la biblioteca".
> Los Controladores MVC y ViewModels están fuera de alcance (pedido en clase: solo biblioteca) y se listan en §4.7-4.8 como anexo informativo, no como inventario principal.

---

# 4. Inventario de clases

## 4.1. Biblioteca Bib_Hacienda — clases de negocio (alcance oficial)

| ID     | Clase         | Tipo                      | Ubicación | Responsabilidad verificada por el equipo |
|--------|---------------|---------------------------|-----------|------------------------------------------|
| BIB-01 | Hacienda      | Clase negocio (~558L)     | Biblioteca | Estado general: potreros, reses, ventas, vacunas, eventos. Concentración verificada línea por línea (punto de dolor #1 propio). |
| BIB-02 | Potrero       | Clase negocio             | Biblioteca | Conjunto de reses (máx 150 ReglaPotrero). |
| BIB-03 | Res           | Clase negocio abstracta   | Biblioteca | Res genérica (Nombre, Peso, Edad abstracta). |
| BIB-04 | Ternero       | Clase negocio             | Biblioteca | Res edad ≤12 (ReglaRes). |
| BIB-05 | Cebon         | Clase negocio             | Biblioteca | Res edad 13–48. |
| BIB-06 | Novillo       | Clase negocio             | Biblioteca | Res edad >48. |
| BIB-07 | Vacuna        | Clase negocio abstracta   | Biblioteca | Vacuna (nombre, lote, fechas). |
| BIB-08 | Bacteriana    | Clase negocio             | Biblioteca | Vacuna con periodo aplicación. |
| BIB-09 | Viva          | Clase negocio             | Biblioteca | Vacuna con grado atenuación. |
| BIB-10 | Venta         | Clase negocio (registro)  | Biblioteca | Snapshot venta (no ref viva a Res/Potrero). |
| BIB-11 | Usuario       | Clase negocio             | Biblioteca | Usuarios/credenciales. |
| BIB-12 | Autenticacion | Servicio biblioteca       | Biblioteca | Autenticación (realiza IAutenticacion). |

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

## 4.6. Servicios de la biblioteca (corrección: están a este lado, no en MVC)

| ID     | Clase               | Tipo                | Ubicación | Responsabilidad |
|--------|---------------------|---------------------|-----------|-----------------|
| SER-01 | PersistenciaService | Servicio biblioteca (643L) | Biblioteca | Archivos txt + validación + proxy (mezcla verificada por el equipo). |
| SER-02 | PotreroService      | Servicio biblioteca | Biblioteca | Operaciones potreros (depende de Hacienda y PersistenciaService concretos). |
| SER-03 | ResService          | Servicio biblioteca | Biblioteca | Operaciones reses (ídem). |
| SER-04 | VacunaService       | Servicio biblioteca | Biblioteca | Operaciones vacunas (ídem). |
| SER-05 | VentaService        | Servicio biblioteca | Biblioteca | Operaciones ventas (ídem). |
| SER-06 | UsuarioService      | Servicio biblioteca | Biblioteca | Usuarios/autenticación (ídem). |

---

## 4.7. Anexo fuera de alcance — Controladores MVC (informativo, no evaluar)

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

## 4.8. Anexo fuera de alcance — ViewModels (informativo, no evaluar)

| ID    | Clase          | Estado |
|-------|----------------|--------|
| VM-01 | LoginViewModel | Fuera de alcance (front) |
| VM-02 | ErrorViewModel | Fuera de alcance (front) |

## 4.9. Aporte propio del equipo (trazabilidad anti-IA)

Cada fila BIB/SER fue contrastada con código fuente (no aceptada de la herramienta). Punto de dolor #1 redactado propio: `Hacienda` concentra crear potrero/res, vender, alimentar, crear/aplicar vacuna, eventos e inventario (~558L); `PersistenciaService` mezcla archivos+validación+proxy (~643L). Impacto: cualquier SC (incluida chips) atraviesa ambas clases.

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