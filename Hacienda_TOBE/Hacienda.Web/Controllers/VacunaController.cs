using Hacienda.Application.Abstractions;
using Hacienda.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hacienda.Web.Controllers;

[Authorize]
public class VacunaController : Controller
{
    private readonly IVacunacionAppService _vacunas;

    public VacunaController(IVacunacionAppService vacunas)
    {
        _vacunas = vacunas;
    }

    public async Task<IActionResult> Index()
    {
        var list = await _vacunas.ListarDisponiblesAsync();
        return View(list);
    }

    [HttpPost]
    public async Task<IActionResult> CreateBacteriana(string nombre, string lote, DateTime vencimiento, DateTime aplicacion, uint periodo)
    {
        try { TempData["Msg"] = await _vacunas.CrearVacunaBacterianaAsync(nombre, lote, vencimiento, aplicacion, periodo); }
        catch (Exception ex) { TempData["Err"] = ex.Message; }
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Aplicar(string potreroId, string nombreRes, string lote)
    {
        try { TempData["Msg"] = await _vacunas.AplicarVacunaAsync(potreroId, nombreRes, lote); }
        catch (Exception ex) { TempData["Err"] = ex.Message; }
        return RedirectToAction(nameof(Index));
    }
}
