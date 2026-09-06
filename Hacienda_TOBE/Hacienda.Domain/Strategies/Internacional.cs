namespace Hacienda.Domain.Strategies;


/// Strategy: régimen internacional / exportación.
/// Sin IVA local (0 %). Extensible si más adelante se agregan aranceles.
public sealed class Internacional : ICalculadora
{
    public const decimal PorcentajeImpuesto = 0m;

    public ResultadoCobro Calcular(decimal subtotal)
    {
        if (subtotal < 0)
            throw new ArgumentOutOfRangeException(nameof(subtotal), "El subtotal no puede ser negativo.");

        return new ResultadoCobro(
            subtotal: subtotal,
            descuento: 0m,
            baseImponible: subtotal,
            impuesto: 0m,
            total: subtotal,
            nombreEstrategia: nameof(Internacional));
    }
}