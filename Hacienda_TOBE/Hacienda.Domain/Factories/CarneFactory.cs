using Hacienda.Domain.Entities;

namespace Hacienda.Domain.Factories;

public sealed class CarneFactory : IProductoFactory
{
    public ProductoVendible Crear(
        string nombre,
        decimal cantidad,
        string unidad,
        decimal precioUnitario,
        string? atributoEspecifico = null)
    {
        return new ProductoCarne(nombre, cantidad, unidad, precioUnitario, atributoEspecifico);
    }
}