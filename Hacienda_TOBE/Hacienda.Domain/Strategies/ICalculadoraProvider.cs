namespace Hacienda.Domain.Strategies;

public interface ICalculadoraProvider
{
    ICalculadora Obtener(string nombreEstrategia);
    IReadOnlyList<string> NombresDisponibles { get; }
}