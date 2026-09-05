using Hacienda.Domain.Entities;

namespace Hacienda.Domain.Factories;

public sealed class CebonFactory : IResFactory
{
    public Res Create(string nombre, uint peso, ushort edad)
    {
        return new Cebon(nombre, peso, edad);
    }
}