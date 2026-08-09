using Hacienda.Domain.Entities;
using Hacienda.Domain.Ports;

namespace Hacienda.Infrastructure.Persistence;

/// <summary>Adaptador de persistencia en archivos planos (ADR-02).</summary>
public class FilePotreroRepository : IPotreroRepository
{
    private readonly FileStoragePaths _paths;
    private readonly object _lock = new();
    private List<Potrero>? _cache;

    public FilePotreroRepository(FileStoragePaths paths) => _paths = paths;

    public async Task<IReadOnlyList<Potrero>> GetAllAsync(CancellationToken ct = default)
    {
        await EnsureLoadedAsync(ct);
        return _cache!.AsReadOnly();
    }

    public async Task<Potrero?> GetByIdAsync(string identificacion, CancellationToken ct = default)
    {
        await EnsureLoadedAsync(ct);
        return _cache!.FirstOrDefault(p => p.Identificacion.Equals(identificacion, StringComparison.OrdinalIgnoreCase));
    }

    public async Task SaveAsync(Potrero potrero, CancellationToken ct = default)
    {
        await EnsureLoadedAsync(ct);
        lock (_lock)
        {
            var idx = _cache!.FindIndex(p => p.Identificacion.Equals(potrero.Identificacion, StringComparison.OrdinalIgnoreCase));
            if (idx >= 0) _cache[idx] = potrero;
            else _cache.Add(potrero);
            PersistAll();
        }
    }

    public async Task SaveAllAsync(IEnumerable<Potrero> potreros, CancellationToken ct = default)
    {
        await Task.Yield();
        lock (_lock)
        {
            _cache = potreros.ToList();
            PersistAll();
        }
    }

    private async Task EnsureLoadedAsync(CancellationToken ct)
    {
        if (_cache != null) return;
        await Task.Yield();
        lock (_lock)
        {
            if (_cache != null) return;
            _cache = LoadFromDisk();
        }
    }

    private List<Potrero> LoadFromDisk()
    {
        var potreros = new Dictionary<string, Potrero>(StringComparer.OrdinalIgnoreCase);

        if (File.Exists(_paths.Potreros))
        {
            foreach (var line in File.ReadAllLines(_paths.Potreros))
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                var parts = line.Split('|');
                if (parts.Length < 2) continue;
                var tipo = ParseTipo(parts[1]);
                potreros[parts[0]] = new Potrero(parts[0], tipo);
            }
        }

        if (File.Exists(_paths.Reses))
        {
            foreach (var line in File.ReadAllLines(_paths.Reses))
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                var p = line.Split('|');
                if (p.Length < 5) continue;
                // potrero|nombre|peso|edad|tipo
                if (!potreros.TryGetValue(p[0], out var potrero)) continue;
                if (!uint.TryParse(p[2], out var peso)) continue;
                if (!ushort.TryParse(p[3], out var edad)) continue;
                Res? res = p[4].Trim().ToLowerInvariant() switch
                {
                    "ternero" => SafeCreate(() => new Ternero(p[1], peso, edad)),
                    "cebon" or "cebón" => SafeCreate(() => new Cebon(p[1], peso, edad)),
                    "novillo" => SafeCreate(() => new Novillo(p[1], peso, edad)),
                    _ => null
                };
                if (res != null) potrero.CargarRes(res);
            }
        }

        if (File.Exists(_paths.VacunasAplicadas))
        {
            var byRes = new Dictionary<(string pot, string res), List<Vacuna>>();
            foreach (var line in File.ReadAllLines(_paths.VacunasAplicadas))
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                var p = line.Split('|');
                if (p.Length < 7) continue;
                // potrero|res|nombreVac|lote|venc|aplic|tipo|extra
                Vacuna? v = null;
                if (!DateTime.TryParse(p[4], out var venc)) continue;
                if (!DateTime.TryParse(p[5], out var aplic)) continue;
                if (p[6].Equals("Bacteriana", StringComparison.OrdinalIgnoreCase))
                {
                    uint periodo = p.Length > 7 && uint.TryParse(p[7], out var pe) ? pe : 0;
                    v = new Bacteriana(p[2], p[3], venc, aplic, periodo);
                }
                else
                {
                    var grado = GradoAtenuacion.Baja;
                    if (p.Length > 7 && int.TryParse(p[7], out var g) && Enum.IsDefined(typeof(GradoAtenuacion), g))
                        grado = (GradoAtenuacion)g;
                    v = new Viva(p[2], p[3], venc, aplic, grado);
                }
                var key = (p[0], p[1]);
                if (!byRes.ContainsKey(key)) byRes[key] = new List<Vacuna>();
                byRes[key].Add(v);
            }
            foreach (var (key, vacs) in byRes)
            {
                if (!potreros.TryGetValue(key.pot, out var pot)) continue;
                var res = pot.BuscarRes(key.res);
                res?.CargarVacunasAplicadas(vacs);
            }
        }

        return potreros.Values.ToList();
    }

    private void PersistAll()
    {
        if (_cache == null) return;
        var potreroLines = _cache.Select(p => $"{p.Identificacion}|{p.Tipo.ToString().ToLowerInvariant()}");
        File.WriteAllLines(_paths.Potreros, potreroLines);

        var resLines = new List<string>();
        var vacAppLines = new List<string>();
        foreach (var pot in _cache)
        {
            foreach (var r in pot.Reses)
            {
                string tipo = r switch
                {
                    Ternero => "Ternero",
                    Cebon => "Cebon",
                    Novillo => "Novillo",
                    _ => "Res"
                };
                resLines.Add($"{pot.Identificacion}|{r.Nombre}|{r.Peso}|{r.Edad}|{tipo}");
                foreach (var v in r.VacunasAplicadas)
                {
                    if (v is Bacteriana b)
                        vacAppLines.Add($"{pot.Identificacion}|{r.Nombre}|{b.Nombre}|{b.Lote}|{b.FechaVencimiento:yyyy-MM-dd}|{b.FechaAplicacion:yyyy-MM-dd}|Bacteriana|{b.PeriodoAplicacion}");
                    else if (v is Viva vi)
                        vacAppLines.Add($"{pot.Identificacion}|{r.Nombre}|{vi.Nombre}|{vi.Lote}|{vi.FechaVencimiento:yyyy-MM-dd}|{vi.FechaAplicacion:yyyy-MM-dd}|Viva|{(int)vi.GradoAtenuacion}");
                }
            }
        }
        File.WriteAllLines(_paths.Reses, resLines);
        File.WriteAllLines(_paths.VacunasAplicadas, vacAppLines);
    }

    private static TipoPotrero ParseTipo(string s) => s.Trim().ToLowerInvariant() switch
    {
        "ternero" => TipoPotrero.Ternero,
        "cebon" or "cebón" => TipoPotrero.Cebon,
        "novillo" => TipoPotrero.Novillo,
        _ => TipoPotrero.Ternero
    };

    private static T? SafeCreate<T>(Func<T> factory) where T : class
    {
        try { return factory(); }
        catch { return null; }
    }
}
