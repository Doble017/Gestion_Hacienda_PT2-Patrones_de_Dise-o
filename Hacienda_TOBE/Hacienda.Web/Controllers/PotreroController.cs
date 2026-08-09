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
        ViewBag.TotalPotreros = list.Count;
        ViewBag.TotalReses = list.Sum(p => p.Reses.Count);
        return View(list);
    }

    [HttpGet]
    public IActionResult Create() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(string identificacion, string tipo)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(identificacion))
                throw new ArgumentException("La identificación es obligatoria.");
            if (!Enum.TryParse<TipoPotrero>(tipo, true, out var t))
                throw new ArgumentException("Tipo de potrero inválido.");
            TempData["Msg"] = await _service.CrearPotreroAsync(identificacion, t);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            TempData["Err"] = ex.Message;
            return View();
        }
    }

    public async Task<IActionResult> Details(string id)
    {
        if (string.IsNullOrWhiteSpace(id)) return NotFound();
        var p = await _service.ObtenerAsync(id);
        if (p is null) return NotFound();
        return View(p);
    }
}
