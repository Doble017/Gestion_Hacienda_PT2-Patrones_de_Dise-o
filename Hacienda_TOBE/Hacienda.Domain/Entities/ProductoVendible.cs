namespace Hacienda.Domain.Entities;

/// Abstracción de cualquier ítem que se puede vender (productos derivados).
/// No representa un animal (Res), sino una mercancía comercializable.
public abstract class ProductoVendible
{
    public string Nombre { get; protected set; }
    public string Tipo { get; protected set; }
    public decimal Cantidad { get; protected set; }
    public string Unidad { get; protected set; }
    public decimal PrecioUnitario { get; protected set; }

    protected ProductoVendible(
        string nombre,
        string tipo,
        decimal cantidad,
        string unidad,
        decimal precioUnitario)
    {
        if (string.IsNullOrWhiteSpace(nombre))
            throw new ArgumentException("El nombre del producto no puede estar vacío.", nameof(nombre));
        if (cantidad <= 0)
            throw new ArgumentException("La cantidad debe ser mayor que cero.", nameof(cantidad));
        if (precioUnitario < 0)
            throw new ArgumentException("El precio unitario no puede ser negativo.", nameof(precioUnitario));

        Nombre = nombre.Trim();
        Tipo = tipo.Trim();
        Cantidad = cantidad;
        Unidad = unidad.Trim();
        PrecioUnitario = precioUnitario;
    }

    public decimal CalcularMonto() => Cantidad * PrecioUnitario;
}