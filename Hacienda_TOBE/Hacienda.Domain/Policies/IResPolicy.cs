using Hacienda.Domain.Entities;

namespace Hacienda.Domain.Policies;

public interface IResPolicy
{
    uint GetPesoMinimo(Res res);
    uint GetPesoIdealVenta(Res res);
    (byte MaxBacterianas, byte MaxVivas) GetMaxVacunas(Res res);
}

public sealed class DefaultResPolicy : IResPolicy
{
    public uint GetPesoMinimo(Res res) => res switch
    {
        Ternero => Rules.ReglaRes.PesoMinTernero,
        Cebon => Rules.ReglaRes.PesoMinCebon,
        Novillo => Rules.ReglaRes.PesoMinNovillo,
        _ => 0
    };

    public uint GetPesoIdealVenta(Res res) => res switch
    {
        Ternero => Rules.ReglaRes.PesoRecomVentaTernero,
        Cebon => Rules.ReglaRes.PesoRecomVentaCebon,
        Novillo => Rules.ReglaRes.PesoRecomVentaNovillo,
        _ => 0
    };

    public (byte MaxBacterianas, byte MaxVivas) GetMaxVacunas(Res res) => res switch
    {
        Ternero => (Rules.ReglaVacuna.MaxBacTernero, Rules.ReglaVacuna.MaxVivTernero),
        Novillo => (Rules.ReglaVacuna.MaxBacNovillo, Rules.ReglaVacuna.MaxVivNovillo),
        Cebon => (Rules.ReglaVacuna.MaxBacCebon, Rules.ReglaVacuna.MaxVivCebon),
        _ => ((byte)0, (byte)0)
    };
}
