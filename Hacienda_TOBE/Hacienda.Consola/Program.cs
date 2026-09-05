// Hacienda.Consola — Menú de consola pedido en clase (defensa oficial TO-BE).
// Composition Root: único lugar donde se resuelven concretos vía AddHaciendaInfrastructure.
// Alto nivel (menú) -> I*AppService (abstracción) -> File*Repository/LoggingEventPublisher (bajo nivel).
using Hacienda.Application.Abstractions;
using Hacienda.Domain.Entities;
using Hacienda.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

var datosPath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "Hacienda.Web", "Datos"));
var services = new ServiceCollection();
services.AddLogging(b => b.AddConsole().SetMinimumLevel(LogLevel.Warning));
services.AddHaciendaInfrastructure(datosPath);
using var provider = services.BuildServiceProvider();

var potreros = provider.GetRequiredService<IPotreroAppService>();
var reses = provider.GetRequiredService<IResAppService>();
var vacunacion = provider.GetRequiredService<IVacunacionAppService>();
var ventas = provider.GetRequiredService<IVentaAppService>();

Console.WriteLine("== Hacienda TO-BE — Consola de dominio (biblioteca, sin Web) ==");
Console.WriteLine($"Datos: {datosPath}");
while (true)
{
    Console.WriteLine();
    Console.WriteLine("1 Listar potreros/reses  2 Crear potrero  3 Agregar res  4 Alimentar");
    Console.WriteLine("5 Vacunar (aplicar lote)  6 Asignar chip GPS (SC-02)  7 Quitar chip  8 Vender  0 Salir");
    Console.Write("Opción: ");
    var op = Console.ReadLine()?.Trim();
    try
    {
        switch (op)
        {
            case "1":
            {
                var todas = await reses.ListarTodasAsync();
                if (todas.Count == 0) Console.WriteLine("(sin reses)");
                foreach (var (p, r) in todas)
                    Console.WriteLine($"- {p.Identificacion} [{p.Tipo}] | {r.Nombre} {r.GetType().Name} edad={r.Edad} peso={r.Peso} chip={(r.Chip == null ? "sin chip" : $"{r.Chip.Identificador}/{r.Chip.Estado} lat={r.Chip.Latitud} lon={r.Chip.Longitud}")} vac={r.VacunasAplicadas.Count}");
                break;
            }
            case "2":
            {
                Console.Write("Id potrero: "); var id = Console.ReadLine()!.Trim();
                Console.Write("Tipo (0 Ternero, 1 Cebon, 2 Novillo): "); var t = (TipoPotrero)int.Parse(Console.ReadLine()!);
                Console.WriteLine(await potreros.CrearPotreroAsync(id, t));
                break;
            }
            case "3":
            {
                Console.Write("Potrero: "); var pid = Console.ReadLine()!.Trim();
                Console.Write("Nombre res: "); var nom = Console.ReadLine()!.Trim();
                Console.Write("Edad meses: "); var edad = ushort.Parse(Console.ReadLine()!);
                Console.Write("Peso kg: "); var peso = uint.Parse(Console.ReadLine()!);
                Console.WriteLine(await potreros.AgregarResAsync(pid, nom, edad, peso));
                break;
            }
            case "4":
            {
                Console.Write("Potrero: "); var pid = Console.ReadLine()!.Trim();
                Console.Write("Res: "); var nom = Console.ReadLine()!.Trim();
                Console.Write("Incremento kg: "); var inc = uint.Parse(Console.ReadLine()!);
                Console.WriteLine(await reses.AlimentarAsync(pid, nom, inc));
                break;
            }
            case "5":
            {
                Console.Write("Potrero: "); var pid = Console.ReadLine()!.Trim();
                Console.Write("Res: "); var nom = Console.ReadLine()!.Trim();
                Console.Write("Lote vacuna: "); var lote = Console.ReadLine()!.Trim();
                Console.WriteLine(await vacunacion.AplicarVacunaAsync(pid, nom, lote));
                break;
            }
            case "6":
            {
                Console.Write("Potrero: "); var pid = Console.ReadLine()!.Trim();
                Console.Write("Res: "); var nom = Console.ReadLine()!.Trim();
                Console.Write("Chip Id: "); var chip = Console.ReadLine()!.Trim();
                Console.Write("Estado (0 Inactivo, 1 Activo, 2 Mantenimiento, 3 Perdido) [1]: ");
                var estTxt = Console.ReadLine(); var estado = string.IsNullOrWhiteSpace(estTxt) ? EstadoChip.Activo : (EstadoChip)int.Parse(estTxt);
                Console.Write("Lat (vacío=sin GPS): "); var latTxt = Console.ReadLine();
                Console.Write("Lon (vacío=sin GPS): "); var lonTxt = Console.ReadLine();
                double? lat = double.TryParse(latTxt, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out var la) ? la : null;
                double? lon = double.TryParse(lonTxt, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out var lo) ? lo : null;
                Console.WriteLine(await reses.AsignarChipAsync(pid, nom, chip, estado, lat, lon));
                break;
            }
            case "7":
            {
                Console.Write("Potrero: "); var pid = Console.ReadLine()!.Trim();
                Console.Write("Res: "); var nom = Console.ReadLine()!.Trim();
                Console.WriteLine(await reses.QuitarChipAsync(pid, nom));
                break;
            }
            case "8":
            {
                Console.Write("Potrero: "); var pid = Console.ReadLine()!.Trim();
                Console.Write("Res: "); var nom = Console.ReadLine()!.Trim();
                Console.Write("Monto: "); var monto = decimal.Parse(Console.ReadLine()!);
                Console.WriteLine(await ventas.VenderResAsync(pid, nom, monto));
                break;
            }
            case "0": return;
            default: Console.WriteLine("Opción inválida."); break;
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"ERR: {ex.Message}");
    }
}
