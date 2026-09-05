using Hacienda.Domain.Entities;

namespace Hacienda.Domain.Factories;


/// Provider con registro por diccionario.
/// No contiene switch de creación: solo resuelve la fábrica ya registrada.
public sealed class ResFactoryProvider : IResFactoryProvider
{
    private readonly IReadOnlyDictionary<TipoPotrero, IResFactory> _fabricas;

    public ResFactoryProvider(IReadOnlyDictionary<TipoPotrero, IResFactory> fabricas)
    {
        if (fabricas is null || fabricas.Count == 0)
            throw new ArgumentException("Debe registrarse al menos una fábrica de res.", nameof(fabricas));

        _fabricas = fabricas;
    }

    public IResFactory ObtenerFactory(TipoPotrero tipo)
    {
        if (!_fabricas.TryGetValue(tipo, out var factory))
            throw new NotSupportedException($"No existe fábrica registrada para el tipo de potrero '{tipo}'.");

        return factory;
    }
}