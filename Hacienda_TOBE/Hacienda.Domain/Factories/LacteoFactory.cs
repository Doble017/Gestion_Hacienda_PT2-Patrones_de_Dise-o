using Hacienda.Domain.Entities;

namespace Hacienda.Domain.Factories;

public sealed class LacteoFactory : IProductoFactory
{
    public ProductoVendible Crear(
        string nombre,
        decimal cantidad,
        string unidad,
        decimal precioUnitario,
        string? atributoEspecifico = null)
    {
        return new ProductoLacteo(nombre, cantidad, unidad, precioUnitario, atributoEspecifico);
    }
}