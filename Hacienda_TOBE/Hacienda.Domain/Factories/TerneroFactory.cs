using Hacienda.Domain.Entities;

namespace Hacienda.Domain.Factories;

public sealed class TerneroFactory : IResFactory
{
    public Res Create(string nombre, uint peso, ushort edad)
    {
        return new Ternero(nombre, peso, edad);
    }
}