using Hacienda.Application.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hacienda.Web.Controllers;

[Authorize]
public class UsuarioController : Controller
{
    private readonly IUsuarioAppService _usuarios;

    public UsuarioController(IUsuarioAppService usuarios) => _usuarios = usuarios;

    public async Task<IActionResult> Index()
    {
        var list = await _usuarios.ListarAsync();
        return View(list);
    }

    [HttpGet]
    public IActionResult Create() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(string nombre, string contrasena)
    {
        try
        {
            TempData["Msg"] = await _usuarios.CrearAsync(nombre, contrasena);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            TempData["Err"] = ex.Message;
            return View();
        }
    }
}
