# Casos de caracterización — Preservación del comportamiento

**Sistema:** Hacienda (AS-IS y TO-BE)  
**Fase:** 4 — Implementación y evidencia  
**Objetivo:** Demostrar que el rediseño arquitectónico preserva el comportamiento observable del sistema.

**Entorno TO-BE:** solución `Hacienda_TOBE` · defensa oficial `Hacienda.Consola` (Web como anexo)
**Credenciales Web (anexo):** usuario `santi` · contraseña `santi11`
**Respaldo legible (el .docx no abrió al profesor):** `Evidencias/Evidencia_Consola_C01-C08_SC02.md` (salidas reales terminal + `Reses.txt` + `dotnet test 5/5`)

---

## 1. Criterio de comparación

Se considera que el comportamiento se preserva cuando el **resultado de negocio** coincide entre AS-IS y TO-BE (éxito o rechazo, y efecto sobre los datos). No se exige identidad literal de cada mensaje de interfaz.

---

## 2. Resumen de ejecución

| ID | Escenario | Resultado de negocio | ¿Coincide AS-IS / TO-BE? | Evidencia |
|----|-----------|----------------------|---------------------------|-----------|
| C-01 | Crear potrero nuevo | Potrero creado y listado | Sí | `Evidencias/C-01/` |
| C-02 | Crear potrero duplicado | Operación rechazada | Sí | `Evidencias/C-02/` |
| C-03 | Añadir res con edad válida | Res añadida al potrero | Sí | `Evidencias/C-03/` |
| C-04 | Añadir res con edad inválida | Operación rechazada | Sí | `Evidencias/C-04/` |
| C-05 | Alimentar res | Peso incrementado | Sí | `Evidencias/C-05/` |
| C-06 | Crear vacuna | Vacuna en inventario | Sí | `Evidencias/C-06/` |
| C-07 | Aplicar vacuna dentro de límite | Vacuna aplicada y descontada del inventario | Sí | `Evidencias/C-07/` |
| C-08 | Aplicar vacuna excediendo límite | Operación rechazada | Sí | `Evidencias/C-08/` |

---

## 3. Detalle de casos

### C-01 — Crear potrero nuevo

| Campo | Contenido |
|-------|-----------|
| Precondición | No existe potrero con el nombre utilizado |
| Entrada | Nombre único; tipo de potrero válido (Ternero, Cebón o Novillo) |
| Resultado esperado | Creación exitosa; el potrero aparece en el listado |
| Salida AS-IS | Potrero creado correctamente / visible en índice de potreros |
| Salida TO-BE | Potrero creado correctamente / visible en índice de potreros |
| ¿Coincide? | Sí |

---

### C-02 — Crear potrero duplicado

| Campo | Contenido |
|-------|-----------|
| Precondición | Ya existe un potrero con el mismo nombre (resultado de C-01) |
| Entrada | Mismo nombre de potrero |
| Resultado esperado | Rechazo de la operación; no se duplica el registro |
| Salida AS-IS | Error o mensaje de potrero ya existente |
| Salida TO-BE | Error o mensaje de potrero ya existente |
| ¿Coincide? | Sí |

---

### C-03 — Añadir res con edad válida

| Campo | Contenido |
|-------|-----------|
| Precondición | Potrero existente del tipo adecuado |
| Entrada | Nombre de res; edad dentro del rango del tipo (ternero 0–12, cebón 13–48, novillo 49+); peso válido |
| Resultado esperado | Res incorporada al potrero |
| Salida AS-IS | Res añadida; visible en el detalle o listado del potrero |
| Salida TO-BE | Res añadida; visible en el detalle o listado del potrero |
| ¿Coincide? | Sí |

---

### C-04 — Añadir res con edad inválida

| Campo | Contenido |
|-------|-----------|
| Precondición | Potrero existente |
| Entrada | Edad fuera del rango permitido para el tipo de potrero |
| Resultado esperado | Rechazo; la res no se agrega |
| Salida AS-IS | Error de validación de edad / tipo |
| Salida TO-BE | Error de validación de edad / tipo |
| ¿Coincide? | Sí |

---

### C-05 — Alimentar res

| Campo | Contenido |
|-------|-----------|
| Precondición | Res existente (alta en C-03) |
| Entrada | Acción de alimentar (incremento por defecto o cantidad indicada en UI) |
| Resultado esperado | Aumento del peso de la res; posible aviso de umbral según reglas del sistema |
| Salida AS-IS | Peso actualizado tras la operación |
| Salida TO-BE | Peso actualizado tras la operación |
| ¿Coincide? | Sí |

---

### C-06 — Crear vacuna

| Campo | Contenido |
|-------|-----------|
| Precondición | Lote no presente en inventario |
| Entrada | Vacuna bacteriana o viva con datos válidos (nombre, lote, fechas según formulario) |
| Resultado esperado | Vacuna disponible en el inventario |
| Salida AS-IS | Vacuna listada en inventario |
| Salida TO-BE | Vacuna listada en inventario |
| ¿Coincide? | Sí |

---

### C-07 — Aplicar vacuna dentro de límite

| Campo | Contenido |
|-------|-----------|
| Precondición | Res por debajo del máximo de vacunas de ese tipo; vacuna en inventario y vigente |
| Entrada | Aplicación de la vacuna a la res en su potrero |
| Resultado esperado | Vacuna registrada en la res; salida del inventario |
| Salida AS-IS | Aplicación exitosa; historial de vacunas de la res actualizado |
| Salida TO-BE | Aplicación exitosa; historial de vacunas de la res actualizado |
| ¿Coincide? | Sí |

---

### C-08 — Aplicar vacuna excediendo límite

| Campo | Contenido |
|-------|-----------|
| Precondición | Res en el máximo permitido de vacunas bacterianas o vivas para su tipo |
| Entrada | Nuevo intento de aplicar vacuna del tipo que excede el límite |
| Resultado esperado | Rechazo; no se aplica la vacuna adicional |
| Salida AS-IS | Error por límite de vacunación |
| Salida TO-BE | Error por límite de vacunación |
| ¿Coincide? | Sí |

---

## 4. Conclusión

Los ocho casos de caracterización fueron ejecutados sobre el sistema rediseñado (TO-BE) y contrastados con el comportamiento del sistema original (AS-IS). En todos los escenarios el **resultado de negocio coincide**: altas válidas se aceptan, reglas de unicidad, rangos de edad y límites de vacunación se rechazan de forma equivalente, y las operaciones de alimentación y registro de vacunas preservan el efecto observable sobre los datos.

Se concluye que el rediseño arquitectónico **preserva el comportamiento observable** del sistema en el alcance de los escenarios evaluados.

Las capturas de pantalla asociadas se encuentran en `03_Implementacion/Evidencias/`, organizadas por identificador de caso.
