using Hacienda.Domain.Entities;
using Hacienda.Domain.Ports;

namespace Hacienda.Infrastructure.Persistence;

public class FileUsuarioRepository : IUsuarioRepository
{
    private readonly FileStoragePaths _paths;

    public FileUsuarioRepository(FileStoragePaths paths) => _paths = paths;

    public Task<IReadOnlyList<Usuario>> GetAllAsync(CancellationToken ct = default)
    {
        var list = new List<Usuario>();
        if (!File.Exists(_paths.Usuarios)) return Task.FromResult<IReadOnlyList<Usuario>>(list);
        foreach (var line in File.ReadAllLines(_paths.Usuarios))
        {
            if (string.IsNullOrWhiteSpace(line)) continue;
            var p = line.Split('|');
            if (p.Length < 2) continue;
            try { list.Add(new Usuario(p[0], p[1])); } catch { /* skip bad lines */ }
        }
        return Task.FromResult<IReadOnlyList<Usuario>>(list);
    }

    public Task SaveAllAsync(IEnumerable<Usuario> usuarios, CancellationToken ct = default)
    {
        var lines = usuarios.Select(u => $"{u.Nombre}|{u.Contrasena}");
        File.WriteAllLines(_paths.Usuarios, lines);
        return Task.CompletedTask;
    }
}
