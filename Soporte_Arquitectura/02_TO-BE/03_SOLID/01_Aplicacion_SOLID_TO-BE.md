# Aplicación de SOLID en el TO-BE (argumentado)

Este documento responde **literalmente** a lo que pide el enunciado: no basta con nombrar el principio; hay que decir qué se partió, en cuántas piezas, por qué esa frontera y qué se gana.

Convención de color del UML de notación extendida (`00_UML_TO-BE_Notacion_Extendida.drawio`):

| Color | Principio / significado |
|-------|-------------------------|
| Negro/gris | Conservado del AS-IS |
| Verde | SRP (partición) |
| Morado | ISP + DIP (abstracciones nuevas) |
| Naranja | DIP (adaptadores de bajo nivel) |
| Azul | Presentation desacoplada + Composition Root (OCP/DIP) |

---

## 1. SRP — Single Responsibility Principle

### Qué clase se partió
**`Hacienda`** (AS-IS, ~558 líneas) y, de forma complementaria, **`PersistenciaService`** (~643 líneas).

### En cuántas piezas quedó

| Origen AS-IS | Piezas TO-BE | Responsabilidad única de cada pieza |
|--------------|--------------|-------------------------------------|
| `Hacienda` | `PotreroAppService` | Casos de uso de potreros (crear, listar, agregar res) |
| `Hacienda` | `ResAppService` | Listar / buscar / alimentar reses |
| `Hacienda` | `VacunacionAppService` | Crear vacunas y aplicarlas a reses |
| `Hacienda` | `VentaAppService` | Vender res y listar ventas |
| `Hacienda` | `UsuarioAppService` | Autenticar y administrar usuarios |
| `PersistenciaService` | `FilePotreroRepository` | Leer/escribir potreros (+ reses) en archivo |
| `PersistenciaService` | `FileVacunaRepository` | Leer/escribir inventario de vacunas |
| `PersistenciaService` | `FileVentaRepository` | Leer/escribir ventas |
| `PersistenciaService` | `FileUsuarioRepository` | Leer/escribir usuarios |
| Publishers embebidos | `LoggingEventPublisher` | Publicar eventos de dominio |

**Total:** 1 God Class + 1 Persistencia monolítica → **5 App Services + 4 repositorios + 1 publisher** (10 tipos con responsabilidad acotada).

### Por qué esa frontera y no otra
- **Por capacidad de negocio** (potrero / res / vacuna / venta / usuario), no por capa técnica artificial.
- Coincide con los módulos de la UI original (Controllers) y con las Solicitudes de Cambio (SC) del reto: una SC de vacunación no debería tocar ventas.
- Frontera alternativa descartada: “un servicio por método HTTP” → demasiada fragmentación; “un solo `IHaciendaService`” → repite ISP/SRP rotos.

### Qué se gana
1. Un cambio en reglas de vacunación se localiza en `VacunacionAppService` + `ReglaVacuna`.
2. Se puede sustituir solo la persistencia de ventas sin recompilar vacunación.
3. Tests unitarios por capacidad sin arrastrar todo el grafo de `Hacienda`.
4. Lectura del código: el nombre del tipo declara su responsabilidad.

**Evidencia:** `Hacienda.Application/Services/*`, `Hacienda.Infrastructure/Persistence/*`. Diagrama: bloques **verdes**.

---

## 2. OCP — Open/Closed Principle

### Qué quedó abierto a extensión
| Punto de extensión | Cómo se extiende sin modificar el núcleo |
|--------------------|------------------------------------------|
| `IEventPublisher` | Nueva implementación (email, bus) + registro en Composition Root |
| `I*Repository` | `Sql*Repository` u otro medio sin tocar App Services |
| `IResPolicy` / `IResFactory` | Políticas alternativas de peso/creación |
| Jerarquía `Vacuna` / `Res` | Nuevos subtipos (con verificación LSP) |

### Qué quedó cerrado a modificación
- Invariantes de edad en subclases de `Res`.
- Límites de vacunas en `ReglaVacuna`.
- Lógica de orquestación de cada App Service (no necesita editarse para cambiar el canal de eventos).

### Qué se gana
SC futuras de notificación o de medio de almacenamiento no obligan a reeditar dominio ni controllers.

**Color en UML:** azul (presentation/Composition Root) y morado (puertos de extensión).

---

## 3. LSP — Liskov Substitution Principle

### Herencia conservada (negro en el UML)
```
Res «abstract»
 ├── Ternero
 ├── Cebon
 └── Novillo

Vacuna «abstract»
 ├── Bacteriana
 └── Viva
```

### Por qué herencia y no composición
| Criterio | Herencia (elegida) | Composición + Strategy |
|----------|--------------------|------------------------|
| Modelo mental del dominio | Res **es-un** Ternero/Cebon/Novillo | Requiere objeto “etapa” separado |
| Código AS-IS ya estable | Conserva comportamiento observable | Reescritura amplia |
| LSP | Pasa (ver `04_LSP`) | N/A |
| SC actuales | Suficiente | Sobre-ingeniería |

### Verificación explícita de sustituibilidad (Res)

| Dimensión | Tipo base `Res` | Subtipos |
|-----------|-----------------|----------|
| **Precondiciones** | Nombre no vacío | Añaden restricción de edad **solo en construcción** (no en operaciones posteriores) |
| **Postcondiciones** | `Alimentar` aumenta peso; `RegistrarVacuna` agrega a la lista | Se preservan sin debilitar |
| **Invariantes** | Colección de vacunas solo lectura hacia afuera | Se preservan |
| **Excepciones** | `ArgumentException` / `ArgumentNullException` en entradas inválidas | No introducen excepciones nuevas en `Alimentar` / `RegistrarVacuna` |

**Conclusión:** un cliente que espera `Res` puede recibir `Ternero`/`Cebon`/`Novillo` sin sorpresas.  
**Documento completo:** `04_LSP/01_Verificacion_LSP_Res_Vacuna.md`.  
**ADR:** ADR-05 (refutación del hallazgo H-06).

---

## 4. ISP — Interface Segregation Principle

### Qué se partió a nivel de contrato
AS-IS: una sola clase implementaba `IVacunacion`, `IVentaRes`, `ICreacionVacuna` → el cliente de venta veía métodos de vacunación.

TO-BE (interfaces segregadas):

| Interfaz | Consumidores típicos |
|----------|----------------------|
| `IPotreroAppService` | `PotreroController` |
| `IResAppService` | `ResController`, `VacunaController` |
| `IVacunacionAppService` | `VacunaController` |
| `IVentaAppService` | `VentaController`, `ResController` |
| `IUsuarioAppService` | `AccountController`, `UsuarioController` |

### Por qué esa frontera
Alineada a **roles de cliente** (controllers / casos de uso), no a “una interfaz por método”.

### Qué se gana
- Un controller no se ve obligado a depender de métodos que no usa.
- Cambios en la firma de vacunación no recompilan el módulo de usuarios.
- Facilita mocks en pruebas por capacidad.

**Color en UML:** morado.

---

## 5. DIP — Dependency Inversion Principle

### Plantilla exigida por el enunciado (por cada inversión)

| # | Módulo de **alto nivel** | **Abstracción** que desacopla | Módulo de **bajo nivel** | **Composition Root** |
|---|--------------------------|-------------------------------|--------------------------|----------------------|
| 1 | `PotreroAppService` | `IPotreroRepository` | `FilePotreroRepository` | `Program.cs` → `AddHaciendaInfrastructure` |
| 2 | `PotreroAppService` / `ResAppService` / `VacunacionAppService` | `IEventPublisher` | `LoggingEventPublisher` | idem |
| 3 | `VacunacionAppService` | `IVacunaRepository` | `FileVacunaRepository` | idem |
| 4 | `VentaAppService` | `IVentaRepository` | `FileVentaRepository` | idem |
| 5 | `UsuarioAppService` | `IUsuarioRepository` | `FileUsuarioRepository` | idem |
| 6 | `PotreroController` (y resto) | `I*AppService` | `*AppService` | registro Scoped en el mismo Composition Root |

### Evidencia estructural de proyectos
```
Web → Application, Infrastructure
Application → Domain
Infrastructure → Domain, Application
Domain → (nada)
```
El dominio **no** referencia archivos ni ASP.NET: solo define puertos.

### Qué se gana
Sustituir archivos planos por SQL implica **solo** un nuevo adaptador + un cambio de registro en el Composition Root; App Services y Controllers no se modifican.

**Documentos:** `05_DIP/01_Inversion_Dependencias_TO-BE.md`, ADR-02, ADR-04.  
**Color en UML:** morado (abstracciones) + naranja (bajo nivel) + azul (Composition Root).

---

## 6. Mapa hallazgo → principio → remedio

| Hallazgo | Principio | Remedios en código |
|----------|-----------|--------------------|
| H-01 `new Publisher*` en dominio | DIP | `IEventPublisher` + `LoggingEventPublisher` |
| H-02 3 interfaces en una God Class | ISP | `I*AppService` segregados |
| H-03 God Class + Singleton concreto | SRP + DIP | 5 App Services + repositorios |
| H-04 Eventos duplicados en Potrero | DIP | Publisher fuera del dominio |
| H-05 PersistenciaService monolítico | SRP + DIP | 4 `File*Repository` |
| H-06 “Eliminar herencia Res” | LSP | **Refutado** — herencia conservada (negro) |

---

## 7. Artefactos a presentar (corrección: un solo diagrama negocio + consola)

| Artefacto | Ruta |
|-----------|------|
| UML único oficial (evaluar solo este) | `02_Diagramas/UML_TO-BE_Dominio_Unico.drawio` + `UML_TO-BE_Dominio_Unico.md` |
| Anexos (no evaluar) | `00_Notacion_Extendida, 01_Capas, 02_Dominio, 03_DIP_y_Application` |
| LSP | `04_LSP/01_Verificacion_LSP_Res_Vacuna.md` |
| DIP | `05_DIP/01_Inversion_Dependencias_TO-BE.md` (Root: Consola/Program.cs y Web/Program.cs) |
| ADRs | `06_ADR/ADR-01` … `ADR-06` (ADR-06 = SC-02 chips) |
| Demo funcional | `Hacienda.Consola` menú 1-8 (ver `Hacienda_TOBE/README.md`), `Hacienda.Web` como anexo |

## 8. Trazabilidad propia SC-02 chips (lo aplicado de SOLID al cambio elegido)

| Principio | Aplicación al chip | Evidencia |
|---|---|---|
| SRP | Chip vive en `ChipGeolocalizacion`; `Res` solo lo referencia opcional; `ResAppService` orquesta; `FilePotreroRepository` persiste | `Res.cs:12,39-47`, `ResAppService.cs:59-81` |
| OCP | Tipo nuevo + columnas opcionales retrocompatibles; sin tocar vacunación/ventas/Root | Métrica: AS-IS 6-8/8-10 vs TO-BE 4/5, aditivo |
| LSP | Chip no altera jerarquía `Res/Vacuna`; sustitución intacta | `04_LSP` |
| ISP | `AsignarChipAsync/QuitarChipAsync` solo en `IResAppService` | `IAppServices.cs:19-20` |
| DIP | App depende de `IPotreroRepository/IEventPublisher`; File detrás del puerto | `DependencyInjection.cs`, Root Consola/Web |
