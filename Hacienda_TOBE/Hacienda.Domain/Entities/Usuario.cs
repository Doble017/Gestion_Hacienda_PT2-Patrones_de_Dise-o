namespace Hacienda.Domain.Entities;

public class Usuario
{
    public string Nombre { get; private set; }
    public string Contrasena { get; private set; }

    public Usuario(string nombre, string contrasena)
    {
        if (string.IsNullOrWhiteSpace(nombre)) throw new ArgumentException("Nombre vacío.");
        if (string.IsNullOrWhiteSpace(contrasena)) throw new ArgumentException("Contraseña vacía.");
        Nombre = nombre.Trim();
        Contrasena = contrasena;
    }

    public bool CredencialesValidas(string nombre, string contrasena) =>
        Nombre.Equals(nombre, StringComparison.OrdinalIgnoreCase) && Contrasena == contrasena;
}
