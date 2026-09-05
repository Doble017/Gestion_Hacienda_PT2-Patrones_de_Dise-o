using Hacienda.Domain.Entities;
using Xunit;

namespace Hacienda.Tests;

// SC-02: evidencia de que el chip es aditivo y no rompe reglas de negocio (edad/peso/vacunas).
public class ChipTests
{
    [Fact]
    public void AsignarChip_NoAlteraReglasDeNegocio()
    {
        var res = new Ternero("T1", 200, 10);
        var chip = new ChipGeolocalizacion("CHIP-001", EstadoChip.Activo, 4.6, -74.0);

        res.AsignarChip(chip);

        Assert.NotNull(res.Chip);
        Assert.Equal("CHIP-001", res.Chip.Identificador);
        Assert.Equal(EstadoChip.Activo, res.Chip.Estado);
        Assert.Equal((ushort)10, res.Edad); // invariante edad intacta
        Assert.Equal((uint)200, res.Peso);  // invariante peso intacta
    }

    [Fact]
    public void QuitarChip_DejaResOperativa()
    {
        var res = new Cebon("C1", 300, 20);
        res.AsignarChip(new ChipGeolocalizacion("CHIP-002"));
        res.QuitarChip();

        Assert.Null(res.Chip);
        res.Alimentar(10); // negocio sigue operativo
        Assert.Equal((uint)310, res.Peso);
    }

    [Fact]
    public void Chip_RechazaCoordenadasInvalidas()
    {
        var chip = new ChipGeolocalizacion("CHIP-003");
        Assert.Throws<ArgumentOutOfRangeException>(() => chip.ActualizarCoordenadas(200, 0));
    }
}
