using Hacienda.Domain.Entities;

namespace Hacienda.Domain.Factories;

public sealed class PielFactory : IProductoFactory
{
    public ProductoVendible Crear(
        string nombre,
        decimal cantidad,
        string unidad,
        decimal precioUnitario,
        string? atributoEspecifico = null)
    {
        return new ProductoPiel(nombre, cantidad, unidad, precioUnitario, atributoEspecifico);
    }
}