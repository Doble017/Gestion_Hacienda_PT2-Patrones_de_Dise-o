# Matriz de Multiplicidades AS-IS — DOCUMENTO DE APOYO (no solicitado; solo se evalúa su resumen en el UML único)

**Proyecto:** Sistema de Gestión de Hacienda
**Versión:** AS-IS 1.1 (rev. 09/2026: se conserva como apoyo; núcleo evaluable: Hacienda 1-0..* Potrero, Potrero 1-0..* Res ≤150, Res 1-0..* Vacuna, Hacienda 1-0..* Venta/Vacuna)
**Fecha:** 07/08/2026

---

# 1. Objetivo

Documentar las multiplicidades identificadas en las relaciones estructurales del modelo actual, utilizando como fuente principal las propiedades, colecciones y referencias presentes en el código.

---

# 2. Criterios

Las multiplicidades se determinarán a partir de:

- propiedades individuales;
- colecciones;
- referencias obligatorias;
- referencias opcionales;
- inicialización de objetos;
- constructores;
- creación y gestión de elementos relacionados.

No se inferirá una multiplicidad únicamente a partir de las reglas conceptuales del negocio.

Cuando el código no proporcione evidencia suficiente, la multiplicidad será registrada como `No determinada`.

---

# 3. Relaciones del dominio

## 3.1. Hacienda y Potrero

| ID     | Elemento A | Elemento B | Multiplicidad A | Multiplicidad B | Evidencia                                     |
|--------|------------|------------|----------------:|----------------:|-----------------------------------------------|
| MUL-01 | `Hacienda` | `Potrero`  | 1               | 0..*            | `Hacienda` mantiene una colección de potreros |

Representación conceptual actual:

`Hacienda 1 ─── 0..* Potrero`

La multiplicidad indica que una instancia de `Hacienda` puede mantener cero o múltiples potreros.

---

## 3.2. Potrero y Res

| ID     | Elemento A | Elemento B | Multiplicidad A | Multiplicidad B | Evidencia                                 |
|--------|------------|------------|----------------:|----------------:|-------------------------------------------|
| MUL-02 | `Potrero`  | `Res`      | 1               | 0..*            | `Potrero` mantiene una colección de reses |

Representación:

`Potrero 1 ─── 0..* Res`

---

## 3.3. Res y Vacuna

| ID     | Elemento A | Elemento B | Multiplicidad A | Multiplicidad B | Evidencia                               |
|--------|------------|------------|----------------:|----------------:|-----------------------------------------|
| MUL-03 | `Res`      | `Vacuna`   | 1               | 0..*            | La res puede mantener múltiples vacunas |

Representación:

`Res 1 ─── 0..* Vacuna`

---

## 3.4. Hacienda y Venta

| ID     | Elemento A | Elemento B | Multiplicidad A | Multiplicidad B | Evidencia                                              |
|--------|------------|------------|----------------:|----------------:|--------------------------------------------------------|
| MUL-04 | `Hacienda` | `Venta`    | 1               | 0..*            | La hacienda mantiene información asociada a las ventas |

Representación:

`Hacienda 1 ─── 0..* Venta`

La multiplicidad deberá mantenerse sujeta a la implementación concreta de la colección utilizada por `Hacienda`.

---

## 3.5. Hacienda y Usuario

| ID     | Elemento A | Elemento B | Multiplicidad A | Multiplicidad B | Evidencia                                     |
|--------|------------|------------|----------------:|----------------:|-----------------------------------------------|
| MUL-05 | `Hacienda` | `Usuario` | 1                | 0..*            | El sistema puede gestionar múltiples usuarios |

La relación deberá representarse únicamente si la referencia se encuentra estructuralmente establecida en el código.

## 3.6. Venta y Potrero

| ID     | Elemento A | Elemento B | Multiplicidad A | Multiplicidad B | Evidencia                                  |
| ------ | ---------- | ---------- | --------------: | --------------: | ------------------------------------------ |
| MUL-06 | `Venta`    | `Potrero`  |               1 |               1 | `Venta` mantiene una referencia a `Potrero` |

Representación:

`Venta 1 ─── 1 Potrero`

La relación se representa como una asociación navegable desde `Venta` hacia `Potrero`, debido a que `Venta` mantiene una referencia directa al objeto `Potrero`.

## 3.7. Venta y Res

| ID     | Elemento A | Elemento B | Multiplicidad A | Multiplicidad B | Evidencia                           |
| ------ | ---------- | ---------- | --------------: | --------------: | ----------------------------------- |
| MUL-07 | `Venta`    | `Res`      |               1 |               1 | `Venta` mantiene una referencia a `Res` |

Representación:

`Venta 1 ─── 1 Res`

La relación se representa como una asociación navegable desde `Venta` hacia `Res`, debido a que `Venta` mantiene una referencia directa al objeto `Res`.


---

# 4. Relaciones de herencia

Las relaciones de generalización no requieren multiplicidad.

| Superclase   | Subclase           | Relación       |
|--------------|--------------------|----------------|
| `Res`        | `Ternero`          | Generalización |
| `Res`        | `Cebon`            | Generalización |
| `Res`        | `Novillo`          | Generalización |
| `Vacuna`     | `Bacteriana`       | Generalización |
| `Vacuna`     | `Viva`             | Generalización |
| `Validacion` | `ValidadorRes`     | Generalización |
| `Validacion` | `ValidadorPotrero` | Generalización |
| `Validacion` | `ValidadorVacuna`  | Generalización |
| `Validacion` | `ValidadorVenta`   | Generalización |

---

# 5. Relaciones entre servicios y dominio

Las referencias mantenidas por los servicios hacia `Hacienda` representan dependencias estructurales.

| ID     | Origen           | Destino    | Multiplicidad observada |
|--------|------------------|------------|-------------------------|
| MUL-06 | `PotreroService` | `Hacienda` | 1                       |
| MUL-07 | `ResService`     | `Hacienda` | 1                       |
| MUL-08 | `VacunaService`  | `Hacienda` | 1                       |
| MUL-09 | `VentaService`   | `Hacienda` | 1                       |

Estas relaciones no representan relaciones de composición del dominio.

---

# 6. Relaciones entre servicios y persistencia

Los servicios mantienen una dependencia hacia `PersistenciaService`.

| ID     | Origen           | Destino               | Multiplicidad |
|--------|------------------|-----------------------|--------------:|
| MUL-10 | `PotreroService` | `PersistenciaService` | 1             |
| MUL-11 | `ResService`     | `PersistenciaService` | 1             |  
| MUL-12 | `VacunaService`  | `PersistenciaService` | 1             |
| MUL-13 | `VentaService`   | `PersistenciaService` | 1             |
| MUL-14 | `UsuarioService` | `PersistenciaService` | 1             |

La multiplicidad corresponde a la referencia mantenida por cada servicio dentro de su ciclo de vida actual.

---

# 7. Relaciones entre controladores y servicios

| ID     | Controlador         | Servicio         | Multiplicidad   |
|--------|---------------------|------------------|----------------:|
| MUL-15 | `AccountController` | `UsuarioService` | 1               |
| MUL-16 | `PotreroController` | `PotreroService` | 1               |
| MUL-17 | `ResController`     | `ResService`     | 1               |
| MUL-18 | `ResController`     | `PotreroService` | 1               |
| MUL-19 | `UsuarioController` | `UsuarioService` | 1               |
| MUL-20 | `VacunaController`  | `VacunaService`  | 1               |
| MUL-21 | `VacunaController`  | `ResService`     | 1               |
| MUL-22 | `VacunaController`  | `PotreroService` | 1               |
| MUL-23 | `VentaController`   | `VentaService`   | 1               |

---

# 8. Relaciones pendientes de verificación

Las siguientes relaciones requieren revisar directamente las declaraciones de propiedades y colecciones antes de establecer una multiplicidad definitiva:

| ID          | Relación                | Estado                                |
|-------------|-------------------------|---------------------------------------|
| PEND-MUL-01 | `Hacienda` → `Potrero`  | Verificar colección                   |
| PEND-MUL-02 | `Potrero` → `Res`       | Verificar colección                   |
| PEND-MUL-03 | `Res` → `Vacuna`        | Verificar colección                   |
| PEND-MUL-04 | `Hacienda` → `Venta`    | Verificar implementación              |
| PEND-MUL-05 | `Hacienda` → `Usuario`  | Verificar referencia                  |
| PEND-MUL-06 | `Hacienda` → `Res`      | Verificar si existe referencia directa|

---

# 9. Composición y agregación

La multiplicidad no determina por sí misma si una relación es composición o agregación.

Por ejemplo:

`Hacienda 1 ─── 0..* Potrero`

únicamente establece la cardinalidad.

Para decidir posteriormente entre:

- asociación;
- agregación;
- composición;

se deberá analizar adicionalmente el ciclo de vida de los objetos.

Por esta razón, el tipo de relación se mantiene separado de la multiplicidad.

---

# 10. Criterio para el UML AS-IS

Las multiplicidades confirmadas deberán incorporarse al diagrama de clases AS-IS.

Las relaciones cuya multiplicidad no pueda determinarse con evidencia suficiente serán representadas con la multiplicidad que corresponda al código únicamente después de completar la revisión.

No se utilizarán multiplicidades basadas exclusivamente en reglas deseables del dominio.

---

# 11. Trazabilidad

| Documento                         | Relación                  |
|-----------------------------------|---------------------------|
| `Inventario_Clases_AS-IS.md`      | Elementos del modelo      |
| `Matriz_Relaciones_AS-IS.md`      | Tipo de relación          |
| `Matriz_Dependencias_AS-IS.md`    | Dependencias              |
| `Matriz_Multiplicidades_AS-IS.md` | Cardinalidades            |
| `UML_AS-IS_Dominio.drawio`        | Representación gráfica    |


**Observación importante*: en este documento he dejado explícitamente algunas relaciones como pendientes de verificación. Esto es intencional. Para el UML académico sería peor afirmar Hacienda 1 ── 0..* Res si el código realmente no tiene una referencia directa, que dejarla pendiente y comprobarla después.

Con este archivo cerramos la parte principal de relaciones estructurales. El siguiente paso ya puede ser bastante más interesante para tus responsabilidades: 05_Comportamiento, empezando por los flujos actuales que nos permitirán construir los diagramas de secuencia y actividad y, posteriormente, conectar esos comportamientos con los hallazgos SOLID.