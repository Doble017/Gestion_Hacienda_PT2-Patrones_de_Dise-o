namespace Hacienda.Domain.Factories;

/// Implementación del provider que usa un diccionario de registro.
/// No contiene switch. Agregar un nuevo producto = registrar una nueva entrada.
public sealed class ProductoFactoryProvider : IProductoFactoryProvider
{
    private readonly IReadOnlyDictionary<string, IProductoFactory> _fabricas;

    public ProductoFactoryProvider(IReadOnlyDictionary<string, IProductoFactory> fabricas)
    {
        if (fabricas is null || fabricas.Count == 0)
            throw new ArgumentException("Debe registrarse al menos una fábrica de producto.", nameof(fabricas));

        // Normalizamos las claves a mayúsculas para hacer la búsqueda case-insensitive
        _fabricas = fabricas.ToDictionary(
            kvp => kvp.Key.Trim().ToUpperInvariant(),
            kvp => kvp.Value,
            StringComparer.Ordinal);
    }

    public IProductoFactory ObtenerFactory(string tipoProducto)
    {
        if (string.IsNullOrWhiteSpace(tipoProducto))
            throw new ArgumentException("El tipo de producto no puede estar vacío.", nameof(tipoProducto));

        var clave = tipoProducto.Trim().ToUpperInvariant();

        if (!_fabricas.TryGetValue(clave, out var factory))
            throw new NotSupportedException($"No existe fábrica registrada para el tipo de producto '{tipoProducto}'.");

        return factory;
    }
}