namespace Hacienda.Domain.Strategies;

/// Strategy: sin descuento ni impuestos. Total = Subtotal.
public sealed class SinDescuento : ICalculadora
{
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
            nombreEstrategia: nameof(SinDescuento));
    }
}