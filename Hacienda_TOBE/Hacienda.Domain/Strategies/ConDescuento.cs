namespace Hacienda.Domain.Strategies;

/// Strategy: aplica un porcentaje de descuento (rango lógico 0–50).
/// Sin impuestos. El % se inyecta por constructor (OCP / configurable).
public sealed class ConDescuento : ICalculadora
{
    public const decimal PorcentajeMinimo = 0m;
    public const decimal PorcentajeMaximo = 50m;

    /// Porcentaje de descuento en [0, 50].
    public decimal PorcentajeDescuento { get; }

    public ConDescuento(decimal porcentajeDescuento)
    {
        if (porcentajeDescuento < PorcentajeMinimo || porcentajeDescuento > PorcentajeMaximo)
            throw new ArgumentOutOfRangeException(
                nameof(porcentajeDescuento),
                $"El descuento debe estar entre {PorcentajeMinimo} y {PorcentajeMaximo} %.");

        PorcentajeDescuento = porcentajeDescuento;
    }

    public ResultadoCobro Calcular(decimal subtotal)
    {
        if (subtotal < 0)
            throw new ArgumentOutOfRangeException(nameof(subtotal), "El subtotal no puede ser negativo.");

        var descuento = decimal.Round(subtotal * (PorcentajeDescuento / 100m), 2);
        var baseImponible = subtotal - descuento;

        return new ResultadoCobro(
            subtotal: subtotal,
            descuento: descuento,
            baseImponible: baseImponible,
            impuesto: 0m,
            total: baseImponible,
            nombreEstrategia: $"{nameof(ConDescuento)}({PorcentajeDescuento}%)");
    }
}