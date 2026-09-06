using Hacienda.Domain.Entities;

namespace Hacienda.Domain.Factories;

/// Contrato Factory Method para reses.
/// Cada implementación concreta crea un único tipo de Res.
public interface IResFactory
{
    
    /// Crea una res del tipo que esta fábrica sabe construir.
    Res Create(string nombre, uint peso, ushort edad);
}