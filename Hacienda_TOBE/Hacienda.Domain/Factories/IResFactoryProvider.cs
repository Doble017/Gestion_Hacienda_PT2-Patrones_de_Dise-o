using Hacienda.Domain.Entities;

namespace Hacienda.Domain.Factories;

/// Resuelve la fábrica concreta de res según el tipo de potrero.
/// Agregar un nuevo tipo de res = registrar su fábrica, sin modificar este contrato.
public interface IResFactoryProvider
{
    IResFactory ObtenerFactory(TipoPotrero tipo);
}