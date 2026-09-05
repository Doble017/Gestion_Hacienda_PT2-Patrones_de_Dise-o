namespace Hacienda.Domain.Entities;

public sealed class ProductoLacteo : ProductoVendible
{
    /// Tipo de lácteo (ej: leche, queso, yogurt). Opcional.
    public string? Variedad { get; private set; }

    public ProductoLacteo(
        string nombre,
        decimal cantidad,
        string unidad,
        decimal precioUnitario,
        string? variedad = null)
        : base(nombre, "Lacteo", cantidad, unidad, precioUnitario)
    {
        Variedad = string.IsNullOrWhiteSpace(variedad) ? null : variedad.Trim();
    }
}