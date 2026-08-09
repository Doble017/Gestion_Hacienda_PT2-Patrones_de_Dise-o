using Hacienda.Domain.Entities;

namespace Hacienda.Domain.Ports;

public interface IPotreroRepository
{
    Task<IReadOnlyList<Potrero>> GetAllAsync(CancellationToken ct = default);
    Task<Potrero?> GetByIdAsync(string identificacion, CancellationToken ct = default);
    Task SaveAsync(Potrero potrero, CancellationToken ct = default);
    Task SaveAllAsync(IEnumerable<Potrero> potreros, CancellationToken ct = default);
}

public interface IVacunaRepository
{
    Task<IReadOnlyList<Vacuna>> GetDisponiblesAsync(CancellationToken ct = default);
    Task SaveDisponiblesAsync(IEnumerable<Vacuna> vacunas, CancellationToken ct = default);
    Task SaveAplicadasAsync(string potreroId, string nombreRes, IEnumerable<Vacuna> aplicadas, CancellationToken ct = default);
}

public interface IVentaRepository
{
    Task<IReadOnlyList<Venta>> GetAllAsync(CancellationToken ct = default);
    Task AddAsync(Venta venta, CancellationToken ct = default);
}

public interface IUsuarioRepository
{
    Task<IReadOnlyList<Usuario>> GetAllAsync(CancellationToken ct = default);
    Task SaveAllAsync(IEnumerable<Usuario> usuarios, CancellationToken ct = default);
}
