using Hacienda.Domain.Entities;
using Hacienda.Domain.Factories;
using Xunit;

namespace Hacienda.Tests;

public class PotreroTests
{
    [Fact]
    public void AnadirRes_CreaLaResCorrectaSegunTipoDePotrero()
    {
        var factory = new CebonFactory();
        var potrero = new Potrero("P1", TipoPotrero.Cebon, factory);

        var mensaje = potrero.AnadirRes("Cebón 1", 18, 300);

        Assert.Contains("añadida", mensaje);
        Assert.Single(potrero.Reses);
        Assert.IsType<Cebon>(potrero.Reses[0]);
    }

    [Fact]
    public void AnadirRes_UsaLaFactoryInyectadaParaCrearLaRes()
    {
        var potrero = new Potrero("P2", TipoPotrero.Ternero, new StubResFactory());

        potrero.AnadirRes("Res especial", 50, 100);

        Assert.IsType<Novillo>(potrero.Reses[0]);
    }

    private sealed class StubResFactory : IResFactory
    {
        public Res Create(string nombre, uint peso, ushort edad) => new Novillo(nombre, peso, edad);
    }
}