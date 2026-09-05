# Guion video defensa (reemplazo link inaccesible — publicar como No listado y pegar link aquí)

**Duración:** ≤8 min · **Visibilidad exigida:** YouTube/Vimeo **No listado con link público** (probar en incógnito antes de entregar; el 0 anterior fue por link sin acceso).

## Pegar aquí el link público verificado

* Link video: `<PEGAR_URL_PUBLICA_AQUI>` (verificado en incógnito el: ___/___/___)
* Respaldo PDF de este guion + capturas: `Soporte_Arquitectura/03_Implementacion/Evidencias/Evidencia_Consola_C01-C08_SC02.md`

## Guion (tiempos)

| T | Qué mostrar | Qué decir (aporte propio, no leer IA) |
|---|---|---|
| 0:00-1:00 | `UML_AS-IS_Dominio_Unico.md` (1 diagrama, solo biblioteca) | Alcance solo `Bib_Hacienda`; Services a este lado; todo asociación; punto #1 propio: Hacienda 558L + Persistencia 643L concentran todo |
| 1:00-2:30 | `Registro_Hallazgos` H-01..H-12 + 3 dolores | #1 God Class explicado con métodos reales; #2 interfaces anchas; #3 acople a concretos; SC elegida chips por menor impacto |
| 2:30-5:30 | **Consola en vivo**: `dotnet run` → 1 listar → 6 asignar chip a Rayo → 1 verificar → `Reses.txt` línea chip → 7 quitar | SC-02 funcional, no solo clase; aditiva; no toca ventas/vacunas |
| 5:30-7:00 | `UML_TO-BE_Dominio_Unico.md` (1 diagrama) | SOLID visible: verde SRP/SC-02, morado puertos, negro herencias LSP; Venta/Usuario conectadas |
| 7:00-8:00 | `05_DIP` tabla + `DependencyInjection.cs` + `dotnet test 5/5` | Alto/abstracción/bajo/Root + inyección ctor; File justificado (preservar txt, cambiable a SQL con 1 registro); ADR-06; cierre |

## Checklist antes de enviar

* [ ] Link abre en incógnito sin login
* [ ] Se ve consola asignando/quitando chip + persistencia
* [ ] Se ven los 2 diagramas únicos (no los 8 viejos)
* [ ] `dotnet test` 5/5 en pantalla
