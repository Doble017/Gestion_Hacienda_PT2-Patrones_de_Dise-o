using Hacienda.Domain.Entities;
using Hacienda.Domain.Ports;

namespace Hacienda.Infrastructure.Persistence;

public class FileVacunaRepository : IVacunaRepository
{
    private readonly FileStoragePaths _paths;

    public FileVacunaRepository(FileStoragePaths paths) => _paths = paths;

    public Task<IReadOnlyList<Vacuna>> GetDisponiblesAsync(CancellationToken ct = default)
    {
        var list = new List<Vacuna>();
        if (!File.Exists(_paths.Vacunas)) return Task.FromResult<IReadOnlyList<Vacuna>>(list);
        foreach (var line in File.ReadAllLines(_paths.Vacunas))
        {
            if (string.IsNullOrWhiteSpace(line)) continue;
            var p = line.Split('|');
            if (p.Length < 5) continue;
            if (!DateTime.TryParse(p[2], out var venc)) continue;
            if (!DateTime.TryParse(p[3], out var aplic)) continue;
            if (p[4].Equals("Bacteriana", StringComparison.OrdinalIgnoreCase))
            {
                uint periodo = p.Length > 5 && uint.TryParse(p[5], out var pe) ? pe : 0;
                list.Add(new Bacteriana(p[0], p[1], venc, aplic, periodo));
            }
            else
            {
                var grado = GradoAtenuacion.Baja;
                if (p.Length > 5 && int.TryParse(p[5], out var g) && Enum.IsDefined(typeof(GradoAtenuacion), g))
                    grado = (GradoAtenuacion)g;
                list.Add(new Viva(p[0], p[1], venc, aplic, grado));
            }
        }
        return Task.FromResult<IReadOnlyList<Vacuna>>(list);
    }

    public Task SaveDisponiblesAsync(IEnumerable<Vacuna> vacunas, CancellationToken ct = default)
    {
        var lines = new List<string>();
        foreach (var v in vacunas)
        {
            if (v is Bacteriana b)
                lines.Add($"{b.Nombre}|{b.Lote}|{b.FechaVencimiento:yyyy-MM-dd}|{b.FechaAplicacion:yyyy-MM-dd}|Bacteriana|{b.PeriodoAplicacion}");
            else if (v is Viva vi)
                lines.Add($"{vi.Nombre}|{vi.Lote}|{vi.FechaVencimiento:yyyy-MM-dd}|{vi.FechaAplicacion:yyyy-MM-dd}|Viva|{(int)vi.GradoAtenuacion}");
        }
        File.WriteAllLines(_paths.Vacunas, lines);
        return Task.CompletedTask;
    }

    public Task SaveAplicadasAsync(string potreroId, string nombreRes, IEnumerable<Vacuna> aplicadas, CancellationToken ct = default)
    {
        var lines = new List<string>();
        foreach (var vacuna in aplicadas)
        {
            if (vacuna is Bacteriana b)
                lines.Add($"{potreroId}|{nombreRes}|{b.Nombre}|{b.Lote}|{b.FechaVencimiento:yyyy-MM-dd}|{b.FechaAplicacion:yyyy-MM-dd}|Bacteriana|{b.PeriodoAplicacion}");
            else if (vacuna is Viva vi)
                lines.Add($"{potreroId}|{nombreRes}|{vi.Nombre}|{vi.Lote}|{vi.FechaVencimiento:yyyy-MM-dd}|{vi.FechaAplicacion:yyyy-MM-dd}|Viva|{(int)vi.GradoAtenuacion}");
        }

        File.WriteAllLines(_paths.VacunasAplicadas, lines);
        return Task.CompletedTask;
    }
}
