using Hacienda.Domain.Factories;

namespace Hacienda.Domain.Entities;

public enum TipoPotrero { Ternero, Cebon, Novillo }

public class Potrero
{
    public string Identificacion { get; private set; }
    public TipoPotrero Tipo { get; private set; }
    private readonly List<Res> _reses = new();
    public IReadOnlyList<Res> Reses => _reses.AsReadOnly();
    private readonly IResFactory _resFactory;

    
    /// Constructor principal: el llamador debe inyectar la fábrica correcta para el tipo de potrero.
    public Potrero(string identificacion, TipoPotrero tipo, IResFactory resFactory)
    {
        if (string.IsNullOrWhiteSpace(identificacion))
            throw new ArgumentException("Identificación de potrero vacía.");
        Identificacion = identificacion.Trim();
        Tipo = tipo;
        _resFactory = resFactory ?? throw new ArgumentNullException(nameof(resFactory));
    }

    public string AnadirRes(string nombre, ushort edad, uint peso)
    {
        if (_reses.Count >= Rules.ReglaPotrero.MaxReses)
            throw new InvalidOperationException($"El potrero '{Identificacion}' alcanzó el máximo de {Rules.ReglaPotrero.MaxReses} reses.");

        if (_reses.Any(r => r.Nombre.Equals(nombre, StringComparison.OrdinalIgnoreCase)))
            throw new InvalidOperationException($"Ya existe una res '{nombre}' en el potrero '{Identificacion}'.");

        // La fábrica ya sabe qué tipo de Res crear (no se pasa Tipo)
        var res = _resFactory.Create(nombre, peso, edad);
        _reses.Add(res);
        return $"Res '{nombre}' añadida al potrero '{Identificacion}'.";
    }

    public Res? BuscarRes(string nombre) =>
        _reses.FirstOrDefault(r => r.Nombre.Equals(nombre, StringComparison.OrdinalIgnoreCase));

    public bool RemoverRes(string nombre)
    {
        var res = BuscarRes(nombre);
        if (res is null) return false;
        _reses.Remove(res);
        return true;
    }

    /// <summary>Reconstrucción desde persistencia sin revalidar reglas de alta.</summary>
    public void CargarRes(Res res)
    {
        if (!_reses.Any(r => r.Nombre.Equals(res.Nombre, StringComparison.OrdinalIgnoreCase)))
            _reses.Add(res);
    }
}