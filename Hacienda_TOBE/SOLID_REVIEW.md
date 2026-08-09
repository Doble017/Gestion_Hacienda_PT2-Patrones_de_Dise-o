# Revisión SOLID del proyecto Hacienda TO-BE

## Resumen ejecutivo

El proyecto muestra una base sólida de arquitectura orientada a capas y ya incorpora varios elementos alineados con SOLID, especialmente la separación entre dominio, aplicación, infraestructura y presentación. La compilación actual es exitosa y la solución está bien organizada para un ejercicio de refactorización.

Sin embargo, hay varias áreas donde el diseño todavía presenta acoplamiento y dependencias que dificultan la extensión y el mantenimiento. Las principales debilidades se concentran en el principio OCP y en algunos puntos de SRP/DIP.

## Evidencia de verificación

- Compilación validada con: `dotnet build HaciendaTOBE.sln`
- Resultado: compilación correcta.

## Hallazgos principales

### 1) OCP: el diseño sigue dependiendo de condicionales para extender comportamiento

Se detectan múltiples puntos donde la lógica cambia según el tipo concreto de entidad. Eso obliga a modificar el código existente para agregar nuevas variantes.

#### Ejemplos
- [Hacienda.Domain/Entities/Potrero.cs](Hacienda.Domain/Entities/Potrero.cs): `AnadirRes` usa un `switch` sobre `TipoPotrero` para crear un tipo concreto de res.
- [Hacienda.Application/Services/ResAppService.cs](Hacienda.Application/Services/ResAppService.cs): se decide el umbral de peso según el tipo concreto de res mediante `switch`.
- [Hacienda.Application/Services/VacunacionAppService.cs](Hacienda.Application/Services/VacunacionAppService.cs): la lógica de límites de vacunas varía según si la res es `Ternero`, `Cebon` o `Novillo`.
- [Hacienda.Infrastructure/Persistence/FilePotreroRepository.cs](Hacienda.Infrastructure/Persistence/FilePotreroRepository.cs): al reconstruir entidades desde disco se emplean `switch` y creación de instancias concretas.

#### Impacto
- Agregar un nuevo tipo de res, vacuna o regla implica tocar varias clases.
- El código es más frágil frente a cambios de requisitos.

#### Recomendación
- Introducir abstracciones o estrategias para reglas por tipo.
- Mover la decisión de comportamiento a polimorfismo o a objetos especializados.

---

### 2) SRP: algunas clases mezclan varias responsabilidades de negocio y de estado

#### Hallazgo
- [Hacienda.Application/Services/UsuarioAppService.cs](Hacienda.Application/Services/UsuarioAppService.cs): mezcla responsabilidades de autenticación, manejo de caché en memoria, carga de datos y persistencia.

#### Por qué es un problema
- La clase tiene más de un motivo para cambiar: si cambia la política de autenticación, la estrategia de caché o la forma de persistencia, la misma clase debe modificarse.
- El estado en memoria (`_cache`) introduce un detalle de implementación que complica la evolución del servicio.

#### Recomendación
- Separar el servicio de autenticación de un componente de consulta/almacenamiento.
- Considerar un repositorio de usuarios más fino o un servicio de caché dedicado.

---

### 3) SRP: el servicio de vacunación centraliza demasiado flujo

#### Hallazgo
- [Hacienda.Application/Services/VacunacionAppService.cs](Hacienda.Application/Services/VacunacionAppService.cs): gestiona creación de vacunas, validación de vencimiento, reglas de negocio por tipo de res, persistencia y publicación de eventos.

#### Por qué es un problema
- Aunque la clase sigue siendo coherente como servicio de aplicación, está acumulando varias decisiones de negocio distintas.
- Aumenta la complejidad y dificulta probar reglas de negocio por separado.

#### Recomendación
- Extraer una política o validador de vacunación.
- Separar la creación de vacunas de la lógica de aplicación y de las reglas de negocio.

---

### 4) DIP: hay un acoplamiento implícito en la abstracción de persistencia

#### Hallazgo
- [Hacienda.Infrastructure/Persistence/FileVacunaRepository.cs](Hacienda.Infrastructure/Persistence/FileVacunaRepository.cs): `SaveAplicadasAsync` está implementado como no-op, mientras que la persistencia real de vacunas aplicadas se hace en [Hacienda.Infrastructure/Persistence/FilePotreroRepository.cs](Hacienda.Infrastructure/Persistence/FilePotreroRepository.cs).

#### Por qué es un problema
- La interfaz de repositorio no expresa claramente el contrato completo.
- Cualquier implementación alternativa tendría que conocer este detalle implícito para comportarse correctamente.
- Esto aumenta el acoplamiento y reduce la substituibilidad de los repositorios.

#### Recomendación
- Definir un contrato de persistencia más consistente para las vacunas aplicadas.
- Evitar que la lógica de escritura quede repartida entre varios repositorios.

---

### 5) DIP/SRP: el controlador de vacunas inyecta una dependencia que no usa

#### Hallazgo
- [Hacienda.Web/Controllers/VacunaController.cs](Hacienda.Web/Controllers/VacunaController.cs): recibe `IPotreroAppService` en el constructor, pero no lo usa en el código mostrado.

#### Impacto
- Aumenta el ruido del contrato del controlador.
- Puede indicar que el controlador está más acoplado de lo necesario a la capa de aplicación.

#### Recomendación
- Eliminar la dependencia innecesaria o usarla solo si realmente aporta funcionalidad.

---

## Principios que parecen cumplir mejor

### ISP

El proyecto muestra un buen uso de interfaces pequeñas y específicas en [Hacienda.Application/Abstractions/IAppServices.cs](Hacienda.Application/Abstractions/IAppServices.cs), lo que favorece la segregación de responsabilidades y reduce el acoplamiento innecesario.

### LSP

No se encontraron violaciones claras en la jerarquía de entidades como [Hacienda.Domain/Entities/Res.cs](Hacienda.Domain/Entities/Res.cs) y [Hacienda.Domain/Entities/Vacuna.cs](Hacienda.Domain/Entities/Vacuna.cs). La herencia se usa de forma razonable para modelar comportamientos compartidos.

## Resultado general

| Principio | Estado | Observación |
|---|---|---|
| SRP | Parcialmente cumplido | Hay clases con más de un motivo para cambiar y lógica mezclada. |
| OCP | Parcialmente cumplido | Se detectan varios puntos de extensión forzada mediante condicionales. |
| LSP | Cumplido | No se observan violaciones evidentes en la jerarquía actual. |
| ISP | Cumplido | Las interfaces de aplicación están bien segregadas. |
| DIP | Parcialmente cumplido | El diseño general usa puertos, pero hay acoplamientos implícitos y contratos de repositorio poco claros. |

## Recomendación prioritaria

La refactorización más valiosa sería introducir abstracciones para las reglas por tipo y separar los servicios que hoy combinan reglas de negocio, validación y persistencia. Eso mejoraría significativamente la extensibilidad del sistema sin modificar mucho código existente.
