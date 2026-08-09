using Hacienda.Application.Abstractions;
using Hacienda.Domain.Entities;
using Hacienda.Domain.Ports;

namespace Hacienda.Application.Services;

public class UsuarioAppService : IUsuarioAppService
{
    private readonly IUsuarioRepository _usuarios;

    public UsuarioAppService(IUsuarioRepository usuarios)
    {
        _usuarios = usuarios;
    }

    public Task CargarAsync(CancellationToken ct = default) => Task.CompletedTask;

    public async Task<string> CrearAsync(string nombre, string contrasena, CancellationToken ct = default)
    {
        var usuarios = (await _usuarios.GetAllAsync(ct)).ToList();
        if (usuarios.Any(u => u.Nombre.Equals(nombre, StringComparison.OrdinalIgnoreCase)))
            throw new InvalidOperationException($"Ya existe el usuario '{nombre}'.");

        var nuevoUsuario = new Usuario(nombre, contrasena);
        usuarios.Add(nuevoUsuario);
        await _usuarios.SaveAllAsync(usuarios, ct);
        return $"Usuario '{nombre}' creado.";
    }

    public async Task<bool> AutenticarAsync(string nombre, string contrasena, CancellationToken ct = default)
    {
        var usuarios = await _usuarios.GetAllAsync(ct);
        return usuarios.Any(u => u.CredencialesValidas(nombre, contrasena));
    }

    public Task<IReadOnlyList<Usuario>> ListarAsync(CancellationToken ct = default) => _usuarios.GetAllAsync(ct);
}
