namespace Hacienda.Domain.Factories;

/// Resuelve la fábrica concreta de producto según el tipo solicitado.
/// Es el punto central de extensión: para agregar un nuevo producto
/// solo hay que registrar su fábrica, sin modificar la lógica de resolución.
public interface IProductoFactoryProvider
{

    /// Devuelve la fábrica correspondiente al tipo de producto.
    
    IProductoFactory ObtenerFactory(string tipoProducto);
}