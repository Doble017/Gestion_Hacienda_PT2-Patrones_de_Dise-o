# UML TO-BE Dominio — Diagrama Único Oficial (negocio + SC-02 chips + SOLID/DIP visibles)

> **Este es el único diagrama TO-BE a evaluar.** Archivo drawio canónico: `UML_TO-BE_Dominio_Unico.drawio`.
> Alcance estricto: **solo negocio (`Hacienda.Domain` + puertos + `Hacienda.Application` como casos de uso)**. Nada de Web/Infraestructura/Presentación en el dibujo principal (van en anexo `01_Capas` y en texto DIP §4).
> Convención color (notación extendida, 1 sola leyenda): Negro/gris=conservado | Verde=SRP/nuevo SC-02 | Morado=puertos ISP+DIP | Naranja=adaptadores File/Logging (fuera del dibujo, solo tabla) | Azul=Composition Root + Consola (fuera del dibujo, solo tabla).

## 1. Diagrama único en texto (todo en una vista, con Venta/Usuario conectadas y chips explícito)

```text
+-------------------------------------------------- DOMINIO (Hacienda.Domain, no conoce archivos ni Web) --------------------------------------------------+
| NEGRO conservado: Potrero --1-0..*--> Res «abstract» --1-0..*--> Vacuna «abstract»                                                                       |
|   Potrero(Id,Tipo:TipoPotrero,_reses<=150 ReglaPotrero)  Res(Nombre,Peso,Edad) -> Ternero(<=12) / Cebon(13-48) / Novillo(>48) [ReglaRes]                   |
|   Vacuna(Nombre,Lote,Vencimiento)+EstaVencida() -> Bacteriana(Periodo) / Viva(Grado) [ReglaVacuna]                                                        |
|   Venta(snapshot: potreroId,resNombre,monto,fecha; NO ref viva) <-- asociada a Potrero 1-1 y Res 1-1 por snapshot  |  Usuario(Nombre,Clave)         |
| VERDE SRP+OCP (nuevo SC-02): Res --1-0..1--> ChipGeolocalizacion {Identificador,Estado(Inactivo/Activo/Mantenimiento/Perdido),Lat/Lon,Fecha}               |
|   + Res.AsignarChip()/QuitarChip()/CargarChip()  (única modificación mínima a Res; resto del núcleo cerrado)                                             |
| MORADO puertos (ISP+DIP, en Domain.Ports / Application.Abstractions):                                                                                    |
|   IPotreroRepository | IVacunaRepository | IVentaRepository | IUsuarioRepository | IEventPublisher                                                   |
|   IResFactory/DefaultResFactory | IResPolicy/DefaultResPolicy | IPotreroAppService | IResAppService(+AsignarChipAsync/QuitarChipAsync) | IVacunacionAppService | IVentaAppService | IUsuarioAppService |
| Eventos dominio (records): PesoMinimoAlcanzado, PesoIdealVenta, PotreroMitadCapacidad, PotreroLleno, VacunacionCompletada, VacunaVencidaDetectada          |
+-------------------------------------------------------------------------------------------------------------------------------------------------------------+
| Alto nivel (Application: Potrero/Res/Vacunacion/Venta/UsuarioAppService) --inyecta por ctor--> MORADO --resuelve en Root--> NARANJA (File*Repository, LoggingEventPublisher) |
| Presentación (fuera del dibujo): Consola Menu o Controllers --inyectan--> I*AppService (Scoped). Composition Root AZUL: Consola/Program.cs o Web/Program.cs -> AddHaciendaInfrastructure(dataPath) |
```

Herencias conservadas y justificadas (LSP verificado): `Res->3 subtipos`, `Vacuna->2 subtipos`. `Venta` y `Usuario` ya no aisladas: `Venta` ligada por snapshot a `Potrero/Res`, `Usuario` como entidad de autenticación usada por `IUsuarioAppService`.

## 2. SC-02 chips visible (lo que el profe no encontró)

| Elemento | Tipo | Evidencia código |
|---|---|---|
| `ChipGeolocalizacion` + `EstadoChip` | Nuevo (verde, OCP) | `Domain/Entities/ChipGeolocalizacion.cs` |
| `Res.Chip? + Asignar/Quitar/CargarChip()` | Modificación mínima | `Domain/Entities/Res.cs:12,39-47` |
| `IResAppService.AsignarChipAsync/QuitarChipAsync` | Extensión ISP | `Application/Abstractions/IAppServices.cs:19-20`, `Services/ResAppService.cs:59-81` |
| `FilePotreroRepository` columnas opcionales `chipId\|estado\|lat\|lon` | Adaptador, retrocompatible | `Infrastructure/Persistence/FilePotreroRepository.cs` |
| UI/Consola | Funcional, no solo clase | `Web/Controllers/ResController.AsignarChip/QuitarChip` + `Views/Res/Index.cshtml` col `Chip GPS` + `Consola` menú 6/7 (nuevo) |
| No toca | Vacunación, ventas, Root, Hacienda (eliminada) | Métrica OCP: AS-IS 6-8 clases/8-10 arch vs TO-BE 4/5, aditivo |

## 3. SOLID en el diagrama único (qué se partió, frontera, ganancia)

* **S:** `Hacienda 558L + PersistenciaService 643L -> 5 AppServices + 4 File* + LoggingEventPublisher` (frontera por capacidad negocio potrero/res/vacuna/venta/usuario).
* **O:** abierto `IEventPublisher, I*Repository, IResPolicy/Factory, subtipos`; cerrado invariantes edad/límites/orquestación; prueba empírica SC-02 aditiva.
* **L:** herencias conservadas, precondición edad solo en construcción, sin nuevas excepciones en `Alimentar/RegistrarVacuna/EstaVencida` (doc `04_LSP`).
* **I:** 5 `I*AppService` por rol cliente (Consola/Controller); venta no ve vacunación.
* **D:** ver §4.

## 4. DIP explícito (quién inyecta qué, dónde)

| # | Alto nivel | Abstracción | Bajo nivel | Composition Root | Inyección |
|---|---|---|---|---|---|
| 1 | `PotreroAppService, ResAppService` | `IPotreroRepository` | `FilePotreroRepository` | `Infrastructure/DependencyInjection.cs:AddHaciendaInfrastructure` llamado desde `Consola/Program.cs` o `Web/Program.cs` | ctor, Singleton |
| 2 | `Potrero/Res/VacunacionAppService` | `IEventPublisher` | `LoggingEventPublisher` | idem | ctor, Singleton |
| 3 | `VacunacionAppService` | `IVacunaRepository` | `FileVacunaRepository` | idem | ctor, Singleton |
| 4 | `VentaAppService` | `IVentaRepository` (+`IPotreroRepository`) | `FileVentaRepository` | idem | ctor, Singleton |
| 5 | `UsuarioAppService` | `IUsuarioRepository` | `FileUsuarioRepository` | idem | ctor, Singleton |
| 6 | `Menu Consola` / `*Controller` | `I*AppService` | `*AppService` | mismo Root | ctor, Scoped (consola: scope por operación) |
| 7 | `DefaultResFactory/Policy` | `IResFactory/IResPolicy` | `Default*` | idem | ctor, Singleton |

Por qué `File*`: decisión consciente para **preservar comportamiento/txt** sin migrar a SQL; DIP queda probado porque cambiar a `Sql*Repository` solo exige nuevo adaptador + cambio de registro, sin tocar Application/Domain. Prueba: `Domain` no referencia `Infrastructure/Web` (dirección `Consola->Application->Domain<-Infrastructure`).

## 5. Anexos (no evaluar como diagrama principal)

`00_Notacion_Extendida, 01_Capas, 03_DIP_y_Application` quedan como anexos de detalle. La defensa se hace sobre este único archivo + su drawio gemelo.
