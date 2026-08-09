namespace Hacienda.Domain.Entities;

public abstract class Vacuna
{
    public string Nombre { get; private set; }
    public string Lote { get; private set; }
    public DateTime FechaVencimiento { get; private set; }
    public DateTime FechaAplicacion { get; private set; }

    protected Vacuna(string nombre, string lote, DateTime fechaVencimiento, DateTime fechaAplicacion)
    {
        if (string.IsNullOrWhiteSpace(nombre)) throw new ArgumentException("Nombre de vacuna vacío.");
        if (string.IsNullOrWhiteSpace(lote)) throw new ArgumentException("Lote vacío.");
        Nombre = nombre.Trim();
        Lote = lote.Trim();
        FechaVencimiento = fechaVencimiento;
        FechaAplicacion = fechaAplicacion;
    }

    public bool EstaVencida(DateTime referencia) => FechaVencimiento < referencia;
}

public class Bacteriana : Vacuna
{
    public uint PeriodoAplicacion { get; private set; }

    public Bacteriana(string nombre, string lote, DateTime fechaVencimiento, DateTime fechaAplicacion, uint periodoAplicacion)
        : base(nombre, lote, fechaVencimiento, fechaAplicacion)
    {
        PeriodoAplicacion = periodoAplicacion;
    }
}

public enum GradoAtenuacion { Baja = 0, Media = 1, Alta = 2 }

public class Viva : Vacuna
{
    public GradoAtenuacion GradoAtenuacion { get; private set; }

    public Viva(string nombre, string lote, DateTime fechaVencimiento, DateTime fechaAplicacion, GradoAtenuacion grado)
        : base(nombre, lote, fechaVencimiento, fechaAplicacion)
    {
        GradoAtenuacion = grado;
    }
}
