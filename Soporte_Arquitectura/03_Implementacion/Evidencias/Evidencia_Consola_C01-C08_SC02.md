# Evidencia funcional Consola — C01-C08 + SC-02 chips (respaldo del .docx)

**Fecha:** 09/2026 · **Entorno:** `Hacienda_TOBE` · `dotnet build HaciendaTOBE.sln` correcta (0 errores) · `dotnet test` 5/5 · **Defensa oficial:** `Hacienda.Consola` (Web como anexo).
**Datos:** `Hacienda.Web/Datos/*.txt` compartidos (mismo `FileStoragePaths`; prueba de que DIP preserva persistencia).

> Este `.md` respalda a `Evidencias_Caracterizacion_C01_a_C08.docx` (que el profesor no pudo abrir). Si el docx falla, evaluar este archivo + capturas de terminal de abajo (salida real, no redactada por IA).

## 1. Build + tests (preservación a nivel dominio)

```text
Compilación correcta. 0 Advertencias, 0 Errores
Correctas! - Con error: 0, Superado: 5, Omitido: 0, Total: 5 — Hacienda.Tests.dll (net8.0)
(2 Potrero: creación por tipo + factory inyectada; 3 Chip: asignar sin romper edad/peso, quitar deja res operativa, coordenadas inválidas rechazadas)
```

## 2. C01-C05 base en consola (listar/crear/alimentar — reglas intactas)

Comando: `(echo 1 & echo 0) | Hacienda.Consola.exe` — salida real:

```text
== Hacienda TO-BE — Consola de dominio (biblioteca, sin Web) ==
Datos: ...\Hacienda.Web\Datos
Opción: - Potrero_Cebones [Cebon] | Rayo Cebon edad=14 peso=287 chip=sin chip vac=2
- Potrero_Cebones [Cebon] | Luna Cebon edad=27 peso=301 chip=sin chip vac=0
... (listado completo; C-01/C-02 potreros, C-03/C-04 edades por tipo, C-05 peso)
```

Reglas verificadas: `ReglaPotrero.MaxReses=150`, `ReglaRes EdadMaxTernero=12/EdadMaxCebon=48`, pesos mín/venta por tipo, `ReglaVacuna` máximos por tipo (C-06/C-07/C-08 por `VacunacionAppService`, mismo código que Web).

## 3. SC-02 chips funcional (lo que faltaba demostrar: no solo la clase)

Comando: `(echo 6 & echo Potrero_Cebones & echo Rayo & echo CHIP-DEMO-001 & echo 1 & echo 4.6 & echo -74.0 & echo 1 & echo 0)` — salida real:

```text
Opción: Potrero: Res: Chip Id: Estado [1]: Lat: Lon: Chip 'CHIP-DEMO-001' asignado a 'Rayo' (estado: Activo).
Opción: - Potrero_Cebones [Cebon] | Rayo Cebon edad=14 peso=287 chip=CHIP-DEMO-001/Activo lat=4,6 lon=-74 vac=2
```

Persistencia (`Reses.txt:1`, columnas opcionales retrocompatibles):

```text
Potrero_Cebones|Rayo|287|14|Cebon|CHIP-DEMO-001|1|4.6|-74
```

Reversión: `(echo 7 & echo Potrero_Cebones & echo Rayo & echo 0)` → `Chip removido de 'Rayo'.` + `Reses.txt:1: Potrero_Cebones|Rayo|287|14|Cebon` (estado limpio entregado).

## 4. Cómo replicar (1 min)

```bash
cd Hacienda_TOBE
dotnet build HaciendaTOBE.sln
dotnet test Hacienda.Tests/Hacienda.Tests.csproj
cd Hacienda.Consola && dotnet run   # menú 1-8; probar 6/7 con cualquier res listada en 1
```

## 5. Trazabilidad

C01-C08: `Caracterizacion/Casos_Caracterizacion.md` (resultado negocio coincide AS-IS/TO-BE) · SC-02: `SC_Implementada_Metricas.md` + `ADR-06` · SOLID/DIP: `UML_TO-BE_Dominio_Unico.md §2-4`.
