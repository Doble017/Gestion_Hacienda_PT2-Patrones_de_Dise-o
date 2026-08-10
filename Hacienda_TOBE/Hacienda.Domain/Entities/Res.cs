namespace Hacienda.Domain.Entities;

public abstract class Res
{
    public string Nombre { get; private set; }
    public uint Peso { get; private set; }
    public abstract ushort Edad { get; protected set; }
    private readonly List<Vacuna> _vacunasAplicadas = new();
    public IReadOnlyList<Vacuna> VacunasAplicadas => _vacunasAplicadas.AsReadOnly();

    /// <summary>SC-2: chip de geolocalización opcional (null = sin chip).</summary>
    public ChipGeolocalizacion? Chip { get; private set; }

    protected Res(string nombre, uint peso)
    {
        if (string.IsNullOrWhiteSpace(nombre))
            throw new ArgumentException("El nombre de la res no puede estar vacío.", nameof(nombre));
        Nombre = nombre.Trim();
        Peso = peso;
    }

    public void Alimentar(uint incremento)
    {
        Peso += incremento;
    }

    public void RegistrarVacuna(Vacuna vacuna)
    {
        if (vacuna is null) throw new ArgumentNullException(nameof(vacuna));
        _vacunasAplicadas.Add(vacuna);
    }

    public void CargarVacunasAplicadas(IEnumerable<Vacuna> vacunas)
    {
        _vacunasAplicadas.Clear();
        foreach (var v in vacunas)
            _vacunasAplicadas.Add(v);
    }
    public void AsignarChip(ChipGeolocalizacion chip)
    {
        if (chip is null) throw new ArgumentNullException(nameof(chip));
        Chip = chip;
    }

    public void QuitarChip() => Chip = null;

    public void CargarChip(ChipGeolocalizacion? chip) => Chip = chip;

}

public class Ternero : Res
{
    private ushort _edad;
    public override ushort Edad
    {
        get => _edad;
        protected set
        {
            if (value > Rules.ReglaRes.EdadMaxTernero)
                throw new ArgumentException($"Un ternero no puede superar {Rules.ReglaRes.EdadMaxTernero} meses.");
            _edad = value;
        }
    }

    public Ternero(string nombre, uint peso, ushort edad) : base(nombre, peso)
    {
        Edad = edad;
    }
}

public class Cebon : Res
{
    private ushort _edad;
    public override ushort Edad
    {
        get => _edad;
        protected set
        {
            if (value <= Rules.ReglaRes.EdadMaxTernero || value > Rules.ReglaRes.EdadMaxCebon)
                throw new ArgumentException($"Un cebón debe tener entre {Rules.ReglaRes.EdadMaxTernero + 1} y {Rules.ReglaRes.EdadMaxCebon} meses.");
            _edad = value;
        }
    }

    public Cebon(string nombre, uint peso, ushort edad) : base(nombre, peso)
    {
        Edad = edad;
    }
}

public class Novillo : Res
{
    private ushort _edad;
    public override ushort Edad
    {
        get => _edad;
        protected set
        {
            if (value <= Rules.ReglaRes.EdadMaxCebon)
                throw new ArgumentException($"Un novillo debe tener más de {Rules.ReglaRes.EdadMaxCebon} meses.");
            _edad = value;
        }
    }

    public Novillo(string nombre, uint peso, ushort edad) : base(nombre, peso)
    {
        Edad = edad;
    }
}
