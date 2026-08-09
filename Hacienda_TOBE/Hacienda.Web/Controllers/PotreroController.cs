using Hacienda.Application.Abstractions;
using Hacienda.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hacienda.Web.Controllers;

[Authorize]
public class PotreroController : Controller
{
    private readonly IPotreroAppService _service;

    public PotreroController(IPotreroAppService service) => _service = service;

    public async Task<IActionResult> Index()
    {
        var list = await _service.ListarAsync();
        return View(list);
    }

    [HttpGet]
    public IActionResult Create() => View();

    [HttpPost]
    public async Task<IActionResult> Create(string identificacion, string tipo)
    {
        try
        {
            if (!Enum.TryParse<TipoPotrero>(tipo, true, out var t))
                throw new ArgumentException("Tipo inválido");

            TempData["Msg"] = await _service.CrearPotreroAsync(identificacion, t);
        }
        catch (Exception ex)
        {
            TempData["Err"] = ex.Message;
        }

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Details(string id)
    {
        var p = await _service.ObtenerAsync(id);
        if (p is null) return NotFound();
        return View(p);
    }
}
