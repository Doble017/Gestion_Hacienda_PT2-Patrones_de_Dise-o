using Hacienda.Domain.Entities;
using Hacienda.Domain.Ports;

namespace Hacienda.Infrastructure.Persistence;

public class FileVentaRepository : IVentaRepository
{
    private readonly FileStoragePaths _paths;

    public FileVentaRepository(FileStoragePaths paths) => _paths = paths;

    public Task<IReadOnlyList<Venta>> GetAllAsync(CancellationToken ct = default)
    {
        var list = new List<Venta>();
        if (!File.Exists(_paths.Ventas)) return Task.FromResult<IReadOnlyList<Venta>>(list);
        foreach (var line in File.ReadAllLines(_paths.Ventas))
        {
            if (string.IsNullOrWhiteSpace(line)) continue;
            var p = line.Split('|');
            // potrero|fecha|nombre|peso|edad|tipo|monto
            if (p.Length < 7) continue;
            if (!DateTime.TryParse(p[1], out var fecha)) continue;
            if (!uint.TryParse(p[3], out var peso)) continue;
            if (!ushort.TryParse(p[4], out var edad)) continue;
            if (!decimal.TryParse(p[6], out var monto)) continue;
            list.Add(new Venta(p[0], fecha, p[2], peso, edad, p[5], monto));
        }
        return Task.FromResult<IReadOnlyList<Venta>>(list);
    }

    public async Task AddAsync(Venta venta, CancellationToken ct = default)
    {
        var all = (await GetAllAsync(ct)).ToList();
        all.Add(venta);
        var lines = all.Select(v =>
            $"{v.PotreroId}|{v.Fecha:yyyy-MM-dd}|{v.NombreRes}|{v.PesoRes}|{v.EdadRes}|{v.TipoRes}|{v.Monto}");
        File.WriteAllLines(_paths.Ventas, lines);
    }
}
