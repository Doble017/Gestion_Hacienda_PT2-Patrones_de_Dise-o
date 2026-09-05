namespace Hacienda.Domain.Entities;

public sealed class ProductoCarne : ProductoVendible
{

    /// Corte o tipo de carne (ej: lomo, pecho, etc.). Opcional.
    public string? Corte { get; private set; }

    public ProductoCarne(
        string nombre,
        decimal cantidad,
        string unidad,
        decimal precioUnitario,
        string? corte = null)
        : base(nombre, "Carne", cantidad, unidad, precioUnitario)
    {
        Corte = string.IsNullOrWhiteSpace(corte) ? null : corte.Trim();
    }
}