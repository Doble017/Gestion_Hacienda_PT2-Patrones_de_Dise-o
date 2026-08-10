using Hacienda.Domain.Entities;

namespace Hacienda.Application.Abstractions;

public interface IPotreroAppService
{
    Task<string> CrearPotreroAsync(string identificacion, TipoPotrero tipo, CancellationToken ct = default);
    Task<IReadOnlyList<Potrero>> ListarAsync(CancellationToken ct = default);
    Task<Potrero?> ObtenerAsync(string identificacion, CancellationToken ct = default);
    Task<string> AgregarResAsync(string potreroId, string nombre, ushort edad, uint peso, CancellationToken ct = default);
}

public interface IResAppService
{
    Task<IReadOnlyList<(Potrero Potrero, Res Res)>> ListarTodasAsync(CancellationToken ct = default);
    Task<Res?> BuscarAsync(string potreroId, string nombreRes, CancellationToken ct = default);
    Task<string> AlimentarAsync(string potreroId, string nombreRes, uint incremento, CancellationToken ct = default);
    /// <summary>SC-2: asigna o actualiza el chip de geolocalización de una res.</summary>
    Task<string> AsignarChipAsync(string potreroId, string nombreRes, string chipId, EstadoChip estado, double? lat, double? lon, CancellationToken ct = default);
    Task<string> QuitarChipAsync(string potreroId, string nombreRes, CancellationToken ct = default);
}

public interface IVacunacionAppService
{
    Task<string> CrearVacunaBacterianaAsync(string nombre, string lote, DateTime vencimiento, DateTime aplicacion, uint periodo, CancellationToken ct = default);
    Task<string> CrearVacunaVivaAsync(string nombre, string lote, DateTime vencimiento, DateTime aplicacion, GradoAtenuacion grado, CancellationToken ct = default);
    Task<string> AplicarVacunaAsync(string potreroId, string nombreRes, string loteVacuna, CancellationToken ct = default);
    Task<IReadOnlyList<Vacuna>> ListarDisponiblesAsync(CancellationToken ct = default);
}

public interface IVentaAppService
{
    Task<string> VenderResAsync(string potreroId, string nombreRes, decimal monto, CancellationToken ct = default);
    Task<IReadOnlyList<Venta>> ListarAsync(CancellationToken ct = default);
}

public interface IUsuarioAppService
{
    Task CargarAsync(CancellationToken ct = default);
    Task<string> CrearAsync(string nombre, string contrasena, CancellationToken ct = default);
    Task<bool> AutenticarAsync(string nombre, string contrasena, CancellationToken ct = default);
    Task<IReadOnlyList<Usuario>> ListarAsync(CancellationToken ct = default);
}
