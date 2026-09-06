namespace Hacienda.Domain.Strategies;


/// Strategy: régimen nacional. IVA 19 % sobre el subtotal (sin descuento por defecto).
public sealed class Nacional : ICalculadora
{
    public const decimal PorcentajeIva = 19m;

    public ResultadoCobro Calcular(decimal subtotal)
    {
        if (subtotal < 0)
            throw new ArgumentOutOfRangeException(nameof(subtotal), "El subtotal no puede ser negativo.");

        var impuesto = decimal.Round(subtotal * (PorcentajeIva / 100m), 2);
        var total = subtotal + impuesto;

        return new ResultadoCobro(
            subtotal: subtotal,
            descuento: 0m,
            baseImponible: subtotal,
            impuesto: impuesto,
            total: total,
            nombreEstrategia: nameof(Nacional));
    }
}