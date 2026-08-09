namespace Hacienda.Domain.Entities;

public class Venta
{
    public string PotreroId { get; private set; }
    public DateTime Fecha { get; private set; }
    public string NombreRes { get; private set; }
    public uint PesoRes { get; private set; }
    public ushort EdadRes { get; private set; }
    public string TipoRes { get; private set; }
    public decimal Monto { get; private set; }

    public Venta(string potreroId, DateTime fecha, string nombreRes, uint pesoRes, ushort edadRes, string tipoRes, decimal monto)
    {
        PotreroId = potreroId;
        Fecha = fecha;
        NombreRes = nombreRes;
        PesoRes = pesoRes;
        EdadRes = edadRes;
        TipoRes = tipoRes;
        Monto = monto;
    }
}
