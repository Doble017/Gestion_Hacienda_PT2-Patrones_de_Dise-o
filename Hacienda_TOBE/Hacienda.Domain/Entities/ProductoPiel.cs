namespace Hacienda.Domain.Entities;

public sealed class ProductoPiel : ProductoVendible
{

    /// Calidad o tratamiento de la piel (ej: cruda, curtida). Opcional.
    public string? Calidad { get; private set; }

    public ProductoPiel(
        string nombre,
        decimal cantidad,
        string unidad,
        decimal precioUnitario,
        string? calidad = null)
        : base(nombre, "Piel", cantidad, unidad, precioUnitario)
    {
        Calidad = string.IsNullOrWhiteSpace(calidad) ? null : calidad.Trim();
    }
}