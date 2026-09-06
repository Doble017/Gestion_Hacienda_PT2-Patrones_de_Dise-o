namespace Hacienda.Domain.Strategies;

/// Strategy de cobro (DIP / OCP).
/// Cada implementación aplica sus propias reglas de descuento e impuestos.
/// Cambiar de régimen = registrar otra estrategia en el composition root.
public interface ICalculadora
{
 
    /// Calcula el desglose de cobro a partir del subtotal (monto base).
    ResultadoCobro Calcular(decimal subtotal);
}


/// Resultado inmutable del cálculo de cobro.
public sealed class ResultadoCobro
{
    public decimal Subtotal { get; }
    public decimal Descuento { get; }
    public decimal BaseImponible { get; }
    public decimal Impuesto { get; }
    public decimal Total { get; }
    public string NombreEstrategia { get; }

    public ResultadoCobro(
        decimal subtotal,
        decimal descuento,
        decimal baseImponible,
        decimal impuesto,
        decimal total,
        string nombreEstrategia)
    {
        if (subtotal < 0)
            throw new ArgumentOutOfRangeException(nameof(subtotal), "El subtotal no puede ser negativo.");
        if (descuento < 0)
            throw new ArgumentOutOfRangeException(nameof(descuento), "El descuento no puede ser negativo.");
        if (impuesto < 0)
            throw new ArgumentOutOfRangeException(nameof(impuesto), "El impuesto no puede ser negativo.");

        Subtotal = subtotal;
        Descuento = descuento;
        BaseImponible = baseImponible;
        Impuesto = impuesto;
        Total = total;
        NombreEstrategia = nombreEstrategia ?? string.Empty;
    }

    public override string ToString() =>
        $"[{NombreEstrategia}] Subtotal={Subtotal:C} Descuento={Descuento:C} Base={BaseImponible:C} Impuesto={Impuesto:C} Total={Total:C}";
}