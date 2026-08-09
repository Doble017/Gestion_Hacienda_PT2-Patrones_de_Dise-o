namespace Hacienda.Domain.Rules;

public static class ReglaPotrero
{
    public const ushort MaxReses = 150;
}

public static class ReglaRes
{
    public const ushort PesoMinTernero = 150;
    public const ushort PesoMinCebon = 290;
    public const ushort PesoMinNovillo = 400;
    public const ushort PesoRecomVentaTernero = 250;
    public const ushort PesoRecomVentaCebon = 420;
    public const ushort PesoRecomVentaNovillo = 550;
    public const byte EdadMaxTernero = 12;
    public const byte EdadMaxCebon = 48;
}

public static class ReglaVacuna
{
    // Valores alineados al código AS-IS original
    public const byte MaxBacTernero = 3;
    public const byte MaxBacCebon = 1;
    public const byte MaxBacNovillo = 2;
    public const byte MaxVivTernero = 1;
    public const byte MaxVivCebon = 4;
    public const byte MaxVivNovillo = 2;
}
