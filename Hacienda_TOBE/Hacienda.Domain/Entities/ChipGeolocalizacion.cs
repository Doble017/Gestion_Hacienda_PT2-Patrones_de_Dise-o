namespace Hacienda.Domain.Entities;

/// <summary>
/// SC-2: chip de geolocalización asociado a una res.
/// Tipo nuevo (OCP): no modifica el contrato base de Res más allá de una asociación opcional.
/// </summary>
public enum EstadoChip
{
    Inactivo = 0,
    Activo = 1,
    Mantenimiento = 2,
    Perdido = 3
}

public sealed class ChipGeolocalizacion
{
    public string Identificador { get; private set; }
    public EstadoChip Estado { get; private set; }
    public double? Latitud { get; private set; }
    public double? Longitud { get; private set; }
    public DateTime FechaAsignacion { get; private set; }

    public ChipGeolocalizacion(string identificador, EstadoChip estado = EstadoChip.Activo,
        double? latitud = null, double? longitud = null, DateTime? fechaAsignacion = null)
    {
        if (string.IsNullOrWhiteSpace(identificador))
            throw new ArgumentException("El identificador del chip no puede estar vacío.", nameof(identificador));
        Identificador = identificador.Trim();
        Estado = estado;
        Latitud = latitud;
        Longitud = longitud;
        FechaAsignacion = fechaAsignacion ?? DateTime.UtcNow;
    }

    public void ActualizarEstado(EstadoChip estado) => Estado = estado;

    public void ActualizarCoordenadas(double latitud, double longitud)
    {
        if (latitud is < -90 or > 90) throw new ArgumentOutOfRangeException(nameof(latitud));
        if (longitud is < -180 or > 180) throw new ArgumentOutOfRangeException(nameof(longitud));
        Latitud = latitud;
        Longitud = longitud;
    }
}
