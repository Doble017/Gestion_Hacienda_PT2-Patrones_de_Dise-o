namespace Hacienda.Domain.Strategies;

public sealed class CalculadoraProvider : ICalculadoraProvider
{
    private readonly IReadOnlyDictionary<string, ICalculadora> _calculadoras;

    public CalculadoraProvider(IReadOnlyDictionary<string, ICalculadora> calculadoras)
    {
        if (calculadoras is null || calculadoras.Count == 0)
            throw new ArgumentException("Debe registrarse al menos una calculadora.", nameof(calculadoras));

        _calculadoras = calculadoras.ToDictionary(
            kvp => kvp.Key.Trim().ToUpperInvariant(),
            kvp => kvp.Value,
            StringComparer.Ordinal);
    }

    public IReadOnlyList<string> NombresDisponibles =>
        _calculadoras.Keys.OrderBy(k => k).ToList();

    public ICalculadora Obtener(string nombreEstrategia)
    {
        if (string.IsNullOrWhiteSpace(nombreEstrategia))
            throw new ArgumentException("Debe indicar una forma de cobro.", nameof(nombreEstrategia));

        var clave = nombreEstrategia.Trim().ToUpperInvariant();
        if (!_calculadoras.TryGetValue(clave, out var calc))
            throw new NotSupportedException($"Forma de cobro no disponible: '{nombreEstrategia}'.");

        return calc;
    }
}