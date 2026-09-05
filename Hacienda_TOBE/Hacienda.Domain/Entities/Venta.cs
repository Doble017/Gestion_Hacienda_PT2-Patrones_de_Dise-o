namespace Hacienda.Domain.Entities;

public class Venta
{
    // --- Datos comunes ---
    public string PotreroId { get; private set; }
    public DateTime Fecha { get; private set; }
    public decimal Monto { get; private set; }

    // --- Discriminador ---
    /// true = venta de producto derivado; false = venta de res completa.
    public bool EsVentaDeProducto { get; private set; }

    // --- Datos de venta de res (se mantienen para compatibilidad) ---
    public string NombreRes { get; private set; }
    public uint PesoRes { get; private set; }
    public ushort EdadRes { get; private set; }
    public string TipoRes { get; private set; }

    // --- Datos de venta de producto derivado (nuevos) ---
    public string? NombreProducto { get; private set; }
    public string? TipoProducto { get; private set; }
    public decimal? Cantidad { get; private set; }
    public string? Unidad { get; private set; }


    /// Constructor original: venta de una res completa.
    /// Se mantiene intacto para no romper el comportamiento existente.
    public Venta(
        string potreroId,
        DateTime fecha,
        string nombreRes,
        uint pesoRes,
        ushort edadRes,
        string tipoRes,
        decimal monto)
    {
        PotreroId = potreroId;
        Fecha = fecha;
        NombreRes = nombreRes;
        PesoRes = pesoRes;
        EdadRes = edadRes;
        TipoRes = tipoRes;
        Monto = monto;

        EsVentaDeProducto = false;
        NombreProducto = null;
        TipoProducto = null;
        Cantidad = null;
        Unidad = null;
    }

    /// Constructor nuevo: venta de un producto derivado.
    public Venta(
        string potreroId,
        DateTime fecha,
        ProductoVendible producto,
        decimal monto)
    {
        if (producto is null)
            throw new ArgumentNullException(nameof(producto));

        PotreroId = potreroId;
        Fecha = fecha;
        Monto = monto;

        EsVentaDeProducto = true;
        NombreProducto = producto.Nombre;
        TipoProducto = producto.Tipo;
        Cantidad = producto.Cantidad;
        Unidad = producto.Unidad;

        // Campos de res quedan con valores neutros
        NombreRes = string.Empty;
        PesoRes = 0;
        EdadRes = 0;
        TipoRes = string.Empty;
    }
}