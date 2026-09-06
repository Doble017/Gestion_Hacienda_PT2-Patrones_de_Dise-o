using Hacienda.Application.Abstractions;
using Hacienda.Domain.Entities;

namespace Hacienda.Application.Services;

/// <summary>
/// Implementación de la Fachada. Solo delega; SRP de orquestación.
/// </summary>
public sealed class HaciendaFachada : IHaciendaFachada
{
    private readonly IPotreroAppService _potreros;
    private readonly IResAppService _reses;
    private readonly IVacunacionAppService _vacunacion;
    private readonly IVentaAppService _ventas;
    private readonly IUsuarioAppService _usuarios;

    public HaciendaFachada(
        IPotreroAppService potreros,
        IResAppService reses,
        IVacunacionAppService vacunacion,
        IVentaAppService ventas,
        IUsuarioAppService usuarios)
    {
        _potreros = potreros;
        _reses = reses;
        _vacunacion = vacunacion;
        _ventas = ventas;
        _usuarios = usuarios;
    }

    // --- Potreros ---
    public Task<string> CrearPotreroAsync(string identificacion, TipoPotrero tipo, CancellationToken ct = default) =>
        _potreros.CrearPotreroAsync(identificacion, tipo, ct);

    public Task<IReadOnlyList<Potrero>> ListarPotrerosAsync(CancellationToken ct = default) =>
        _potreros.ListarAsync(ct);

    public Task<Potrero?> ObtenerPotreroAsync(string identificacion, CancellationToken ct = default) =>
        _potreros.ObtenerAsync(identificacion, ct);

    public Task<string> AgregarResAsync(string potreroId, string nombre, ushort edad, uint peso, CancellationToken ct = default) =>
        _potreros.AgregarResAsync(potreroId, nombre, edad, peso, ct);

    // --- Reses ---
    public Task<IReadOnlyList<(Potrero Potrero, Res Res)>> ListarResesAsync(CancellationToken ct = default) =>
        _reses.ListarTodasAsync(ct);

    public Task<Res?> BuscarResAsync(string potreroId, string nombreRes, CancellationToken ct = default) =>
        _reses.BuscarAsync(potreroId, nombreRes, ct);

    public Task<string> AlimentarResAsync(string potreroId, string nombreRes, uint incremento, CancellationToken ct = default) =>
        _reses.AlimentarAsync(potreroId, nombreRes, incremento, ct);

    public Task<string> AsignarChipAsync(string potreroId, string nombreRes, string chipId, EstadoChip estado, double? lat, double? lon, CancellationToken ct = default) =>
        _reses.AsignarChipAsync(potreroId, nombreRes, chipId, estado, lat, lon, ct);

    public Task<string> QuitarChipAsync(string potreroId, string nombreRes, CancellationToken ct = default) =>
        _reses.QuitarChipAsync(potreroId, nombreRes, ct);

    // --- Vacunación ---
    public Task<string> CrearVacunaBacterianaAsync(string nombre, string lote, DateTime vencimiento, DateTime aplicacion, uint periodo, CancellationToken ct = default) =>
        _vacunacion.CrearVacunaBacterianaAsync(nombre, lote, vencimiento, aplicacion, periodo, ct);

    public Task<string> CrearVacunaVivaAsync(string nombre, string lote, DateTime vencimiento, DateTime aplicacion, GradoAtenuacion grado, CancellationToken ct = default) =>
        _vacunacion.CrearVacunaVivaAsync(nombre, lote, vencimiento, aplicacion, grado, ct);

    public Task<string> AplicarVacunaAsync(string potreroId, string nombreRes, string loteVacuna, CancellationToken ct = default) =>
        _vacunacion.AplicarVacunaAsync(potreroId, nombreRes, loteVacuna, ct);

    public Task<IReadOnlyList<Vacuna>> ListarVacunasDisponiblesAsync(CancellationToken ct = default) =>
        _vacunacion.ListarDisponiblesAsync(ct);

    // --- Ventas ---
        // --- Ventas ---
    public Task<string> VenderResAsync(
        string potreroId,
        string nombreRes,
        decimal monto,
        string? estrategiaCobro = null,
        CancellationToken ct = default) =>
        _ventas.VenderResAsync(potreroId, nombreRes, monto, estrategiaCobro, ct);

    public Task<string> VenderProductoAsync(
        string potreroId,
        string tipoProducto,
        string nombreProducto,
        decimal cantidad,
        string unidad,
        decimal precioUnitario,
        string? atributoEspecifico = null,
        string? estrategiaCobro = null,
        CancellationToken ct = default) =>
        _ventas.VenderProductoAsync(
            potreroId, tipoProducto, nombreProducto, cantidad, unidad, precioUnitario,
            atributoEspecifico, estrategiaCobro, ct);

    public Task<IReadOnlyList<Venta>> ListarVentasAsync(CancellationToken ct = default) =>
        _ventas.ListarAsync(ct);    

    // --- Usuarios ---
    public Task CargarUsuariosAsync(CancellationToken ct = default) =>
        _usuarios.CargarAsync(ct);

    public Task<string> CrearUsuarioAsync(string nombre, string contrasena, CancellationToken ct = default) =>
        _usuarios.CrearAsync(nombre, contrasena, ct);

    public Task<bool> AutenticarUsuarioAsync(string nombre, string contrasena, CancellationToken ct = default) =>
        _usuarios.AutenticarAsync(nombre, contrasena, ct);

    public Task<IReadOnlyList<Usuario>> ListarUsuariosAsync(CancellationToken ct = default) =>
        _usuarios.ListarAsync(ct);
}