using Hacienda.Domain.Entities;

namespace Hacienda.Domain.Factories;

/// Contrato del Factory Method para productos derivados.
/// Cada implementación concreta sabe crear un tipo específico de ProductoVendible.
public interface IProductoFactory
{
  
    /// Crea una instancia del producto que esta fábrica sabe construir.
    ProductoVendible Crear(
        string nombre,
        decimal cantidad,
        string unidad,
        decimal precioUnitario,
        string? atributoEspecifico = null);
}