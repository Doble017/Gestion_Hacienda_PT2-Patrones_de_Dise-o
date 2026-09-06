using Hacienda.Domain.Entities;

namespace Hacienda.Application.Abstractions;

/// Fachada de aplicación (Facade).
/// Punto único de acceso desde Presentation hacia los casos de uso.
/// No contiene reglas de negocio: solo delega en los I*AppService.
public interface IHaciendaFachada
{
    // --- Potreros ---
    Task<string> CrearPotreroAsync(string identificacion, TipoPotrero tipo, CancellationToken ct = default);
    Task<IReadOnlyList<Potrero>> ListarPotrerosAsync(CancellationToken ct = default);
    Task<Potrero?> ObtenerPotreroAsync(string identificacion, CancellationToken ct = default);
    Task<string> AgregarResAsync(string potreroId, string nombre, ushort edad, uint peso, CancellationToken ct = default);

    // --- Reses ---
    Task<IReadOnlyList<(Potrero Potrero, Res Res)>> ListarResesAsync(CancellationToken ct = default);
    Task<Res?> BuscarResAsync(string potreroId, string nombreRes, CancellationToken ct = default);
    Task<string> AlimentarResAsync(string potreroId, string nombreRes, uint incremento, CancellationToken ct = default);
    Task<string> AsignarChipAsync(string potreroId, string nombreRes, string chipId, EstadoChip estado, double? lat, double? lon, CancellationToken ct = default);
    Task<string> QuitarChipAsync(string potreroId, string nombreRes, CancellationToken ct = default);

    // --- Vacunación ---
    Task<string> CrearVacunaBacterianaAsync(string nombre, string lote, DateTime vencimiento, DateTime aplicacion, uint periodo, CancellationToken ct = default);
    Task<string> CrearVacunaVivaAsync(string nombre, string lote, DateTime vencimiento, DateTime aplicacion, GradoAtenuacion grado, CancellationToken ct = default);
    Task<string> AplicarVacunaAsync(string potreroId, string nombreRes, string loteVacuna, CancellationToken ct = default);
    Task<IReadOnlyList<Vacuna>> ListarVacunasDisponiblesAsync(CancellationToken ct = default);

    
    // --- Ventas ---
    Task<string> VenderResAsync(
        string potreroId,
        string nombreRes,
        decimal monto,
        string? estrategiaCobro = null,
        CancellationToken ct = default);

    Task<string> VenderProductoAsync(
        string potreroId,
        string tipoProducto,
        string nombreProducto,
        decimal cantidad,
        string unidad,
        decimal precioUnitario,
        string? atributoEspecifico = null,
        string? estrategiaCobro = null,
        CancellationToken ct = default);

    Task<IReadOnlyList<Venta>> ListarVentasAsync(CancellationToken ct = default);
    // --- Usuarios ---
    Task CargarUsuariosAsync(CancellationToken ct = default);
    Task<string> CrearUsuarioAsync(string nombre, string contrasena, CancellationToken ct = default);
    Task<bool> AutenticarUsuarioAsync(string nombre, string contrasena, CancellationToken ct = default);
    Task<IReadOnlyList<Usuario>> ListarUsuariosAsync(CancellationToken ct = default);
}