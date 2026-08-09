using Hacienda.Domain.Entities;

namespace Hacienda.Domain.Factories;

public interface IResFactory
{
    Res CreateRes(string nombre, uint peso, ushort edad, TipoPotrero tipo);
}

public sealed class DefaultResFactory : IResFactory
{
    public Res CreateRes(string nombre, uint peso, ushort edad, TipoPotrero tipo) =>
        tipo switch
        {
            TipoPotrero.Ternero => new Ternero(nombre, peso, edad),
            TipoPotrero.Cebon => new Cebon(nombre, peso, edad),
            TipoPotrero.Novillo => new Novillo(nombre, peso, edad),
            _ => throw new InvalidOperationException("Tipo de potrero no soportado.")
        };
}
