namespace Hacienda.Infrastructure.Persistence;

public class FileStoragePaths
{
    public string DataDirectory { get; }

    public FileStoragePaths(string dataDirectory)
    {
        DataDirectory = dataDirectory;
        Directory.CreateDirectory(DataDirectory);
    }

    public string Potreros => Path.Combine(DataDirectory, "Potreros.txt");
    public string Reses => Path.Combine(DataDirectory, "Reses.txt");
    public string Vacunas => Path.Combine(DataDirectory, "Vacunas.txt");
    public string VacunasAplicadas => Path.Combine(DataDirectory, "VacunasAplicadas.txt");
    public string Ventas => Path.Combine(DataDirectory, "Ventas.txt");
    public string Usuarios => Path.Combine(DataDirectory, "Usuarios.txt");
}
