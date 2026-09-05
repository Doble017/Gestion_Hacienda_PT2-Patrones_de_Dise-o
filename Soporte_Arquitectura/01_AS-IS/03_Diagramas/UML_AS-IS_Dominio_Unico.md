# UML AS-IS Dominio — Diagrama Único Oficial

> **Este es el único diagrama AS-IS a evaluar.** El resto de `.drawio` en esta carpeta quedan como anexos históricos (generados con IA) y no hacen parte de la defensa.
> Archivo drawio canónico: `UML_AS-IS_Dominio_Unico.drawio` (copia fiel de `02_UML_AS-IS_Dominio.drawio`, única base válida según el profesor).
> Alcance estricto: **solo biblioteca `Bib_Hacienda` (negocio)**. No front, no MVC, no Castle, no infraestructura.

## 1. Alcance (corrección pedida en clase)

* Solo clases de negocio de `Bib_Hacienda`: `Hacienda, Potrero, Res/Ternero/Cebon/Novillo, Vacuna/Bacteriana/Viva, Venta, Usuario, Autenticacion, ReglaRes/ReglaPotrero/ReglaVacuna`.
* Las clases `*Service` (`PersistenciaService, PotreroService, ResService, VacunaService, VentaService, UsuarioService`) **pertenecen a la biblioteca** a este lado (no a MVC). Se dibujan dentro del paquete `Bib_Hacienda`.
* Fuera de alcance (anexo, no evaluar): `Home/Account/Potrero/Res/Vacuna/Venta/Usuario Controllers`, `LoginViewModel/ErrorViewModel`, `Castle.DynamicProxy`, `IHttpContextAccessor`, `ILogger`, archivos `Datos/*.txt`.
* Decisión UML: todo `Hacienda->Potrero, Potrero->Res, Hacienda->Venta/Vacuna, Res->Vacuna` es **asociación** (sin rombos de composición/agregación por falta de evidencia de ciclo de vida en código). Corrige contradicción del README anterior que decía "composición".

## 2. Diagrama único en texto (todo visible en una sola vista)

```text
+------------------------------------------------ Bib_Hacienda ------------------------------------------------+
|                                                                                                              |
|  Hacienda (God Class AS-IS ~558L, Singleton en Program.cs)                                                   |
|   - L_potreros: List<Potrero>  - L_ventas: List<Venta>  - L_vacunas: List<Vacuna>                              |
|   + crear_potrero / buscar_potrero / anadir_res_potrero / vender_res / alimentar_res                          |
|   + crear_vacuna / aplicar_vacuna    Realiza: IVacunacion, IVentaRes, ICreacionVacuna (ver §3)                 |
|        | 1                     | 1                      | 1                                             |
|        | 0..*                  | 0..*                   | 0..*                                          |
|   Potrero (Tipo: TipoPotrero)   Venta (snapshot: potreroId,resNombre,monto,fecha)  Vacuna «abstract»           |
|   - _reses: List<Res> (Max 150 ReglaPotrero)  1--1 Potrero / 1--1 Res (snapshot, no ref viva)  + Bacteriana     |
|        | 1                                                                                          + Viva           |
|        | 0..*                                                                                                         |
|   Res «abstract» (Nombre,Peso,Edad abstracta)                                                                 |
|   + Ternero (Edad<=12)  + Cebon (13-48)  + Novillo (>48)  [ReglaRes]                                          |
|        | 1                                                                                                         |
|        | 0..*                                                                                                      |
|   Vacuna aplicada (List<Vacuna> por Res)                                                                      |
|                                                                                                              |
|  Reglas: ReglaRes (EdadMaxTernero=12, EdadMaxCebon=48) | ReglaPotrero (MaxReses=150) | ReglaVacuna (max bac/viva x tipo)|
|  Servicios (en biblioteca): PersistenciaService 643L (archivos+validación+proxy) | Potrero/Res/Vacuna/Venta/UsuarioService -> Hacienda concreta + PersistenciaService concreta (dolor DIP) |
|  Validación/Eventos (resumen 1 línea, no desglosado): Validacion<-ValidadorRes/Potrero/Vacuna/Venta : IValidarInformacion; 6 Publisher* instanciados con new en Hacienda/Potrero; 2 Interceptores Castle |
+-------------------------------------------------------------------------------------------------------------------------------+
```

Generalizaciones: `Res <- Ternero/Cebon/Novillo`, `Vacuna <- Bacteriana/Viva`, `Validacion <- 4 validadores`.
Realizaciones: `Hacienda -> IVacunacion/IVentaRes/ICreacionVacuna`, `Validacion -> IValidarInformacion`, `Autenticacion -> IAutenticacion`.
Multiplicidades núcleo: `Hacienda 1-0..* Potrero | Potrero 1-0..* Res (<=150) | Res 1-0..* Vacuna | Hacienda 1-0..* Venta, 1-0..* Vacuna`.

## 3. Nota de interfaces (cierra brecha detectada)

`IVacunacion, IVentaRes, ICreacionVacuna` sí existen en código AS-IS (realizadas por `Hacienda`, ver `Matriz_Relaciones REAL-03..05`) pero faltaban en `Inventario_Interfaces_AS-IS.md`. Se agregan en la corrección del inventario como `INT-03..05` con operaciones `aplicar_vacuna / vender_res / crear_vacuna`. Si el equipo no encuentra el archivo fuente exacto, se marca `REAL-03..05 por confirmar en código, no inferir`.

## 4. Aporte propio del equipo (no IA)

* Verificación manual: `Hacienda` concentra potreros+reses+ventas+vacunas+eventos (punto de dolor #1 explicado en lenguaje propio, no "God Class" genérico de IA).
* Decisión propia: no ir al front; Services a biblioteca; solo asociación; un solo diagrama.
* Archivos anexos (no evaluar): `01_General, 03_Validacion_Eventos_AOP, 04_MVC_Servicios, Diagrama_Dependencias_AS-IS, carpeta Diagramas_UML_AS-IS duplicada`.
