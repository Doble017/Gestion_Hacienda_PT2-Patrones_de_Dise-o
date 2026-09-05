using Hacienda.Domain.Entities;

namespace Hacienda.Domain.Factories;

public sealed class NovilloFactory : IResFactory
{
    public Res Create(string nombre, uint peso, ushort edad)
    {
        return new Novillo(nombre, peso, edad);
    }
}