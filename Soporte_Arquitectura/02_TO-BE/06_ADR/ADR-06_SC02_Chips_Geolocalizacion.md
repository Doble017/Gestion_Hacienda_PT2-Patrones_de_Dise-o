# ADR-06 — Implementar SC-02 Chips de geolocalización como extensión aditiva (OCP)

**Estado:** Aceptada · **Fecha:** 09/2026 · **Principios:** OCP, SRP, ISP, DIP · **Hallazgos:** H-03/H-05 (God Class/persistencia) · **SC:** SC-02 (SC-01/SC-03 quedan futuras)

## Contexto y evidencia

AS-IS: agregar chip obligaba a tocar `Hacienda` (~558L), cascada `Hacienda/Potrero`, `PersistenciaService` (~643L) y MVC (6-8 clases, 8-10 archivos). TO-BE ya partió el núcleo en 5 AppServices + 4 `File*` + puertos.

## Alternativas (≥2, una descartada)

| Alt | Descripción | Veredicto |
|---|---|---|
| A | `ChipGeolocalizacion` nuevo + `Res.Chip?` opcional + `Asignar/QuitarChipAsync` + columnas opcionales txt | **Elegida**: aditiva, retrocompatible, no toca ventas/vacunas/Root |
| B | Campos sueltos en `Res` (`chipId, lat, lon` string) sin tipo | Descartada: rompe SRP/encapsulamiento, sin validación coordenadas/estado |
| C | Microservicio/tabla SQL nueva de chips | Descartada: sobreingeniería, cambia comportamiento/persistencia sin necesidad |

## Decisión

Crear `Domain/Entities/ChipGeolocalizacion.cs` (`Identificador, Estado, Lat/Lon, Fecha` + validación) y extender `Res` solo con `Chip? + Asignar/Quitar/CargarChip()`; `IResAppService/ResAppService` exponen `AsignarChipAsync/QuitarChipAsync`; `FilePotreroRepository` persiste columnas opcionales; Consola menú 6/7 + Web modal `Chip GPS` como funcional (no solo clase).

## Costo aceptado

+1 tipo +2 operaciones en `IResAppService`; serialización txt con columnas opcionales (deuda: migrar a SQL exige `Sql*Repository`, ya habilitado por DIP sin tocar App).

## Verificación

OCP empírico: 4 clases / 5 archivos vs 6-8/8-10 AS-IS; C01-C08 preservados; demo Consola asignar/quitar/persistir + `Reses.txt` con chip.
