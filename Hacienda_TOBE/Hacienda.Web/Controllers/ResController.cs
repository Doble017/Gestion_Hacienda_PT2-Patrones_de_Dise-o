using Hacienda.Application.Abstractions;
using Hacienda.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hacienda.Web.Controllers;

[Authorize]
public class ResController : Controller
{
    private readonly IHaciendaFachada _hacienda;

    public ResController(IHaciendaFachada hacienda) => _hacienda = hacienda;

    public async Task<IActionResult> Index()
    {
        var list = await _hacienda.ListarResesAsync();
        ViewBag.TotalReses = list.Count;
        ViewBag.TotalPeso = list.Sum(x => (long)x.Res.Peso);
        return View(list);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        ViewBag.Potreros = await _hacienda.ListarPotrerosAsync();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(string potreroId, string nombre, ushort edad, uint peso)
    {
        try
        {
            TempData["Msg"] = await _hacienda.AgregarResAsync(potreroId, nombre, edad, peso);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            TempData["Err"] = ex.Message;
            ViewBag.Potreros = await _hacienda.ListarPotrerosAsync();
            return View();
        }
    }

    public async Task<IActionResult> DetalleVacunas(string potreroId, string nombre)
    {
        var res = await _hacienda.BuscarResAsync(potreroId, nombre);
        if (res is null) return NotFound();
        ViewBag.PotreroId = potreroId;
        return View(res);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Alimentar(string potreroId, string nombre, uint incremento)
    {
        try
        {
            if (incremento == 0) incremento = 10;
            TempData["Msg"] = await _hacienda.AlimentarResAsync(potreroId, nombre, incremento);
        }
        catch (Exception ex) { TempData["Err"] = ex.Message; }
        return RedirectToAction(nameof(Index));
    }

        [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Vender(string potreroId, string nombre, decimal monto, string? estrategiaCobro)
    {
        try
        {
            TempData["Msg"] = await _hacienda.VenderResAsync(potreroId, nombre, monto, estrategiaCobro);
        }
        catch (Exception ex) { TempData["Err"] = ex.Message; }
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AsignarChip(string potreroId, string nombre, string chipId, int estado, double? lat, double? lon)
    {
        try
        {
            var est = Enum.IsDefined(typeof(EstadoChip), estado)
                ? (EstadoChip)estado
                : EstadoChip.Activo;
            TempData["Msg"] = await _hacienda.AsignarChipAsync(potreroId, nombre, chipId, est, lat, lon);
        }
        catch (Exception ex) { TempData["Err"] = ex.Message; }
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> QuitarChip(string potreroId, string nombre)
    {
        try { TempData["Msg"] = await _hacienda.QuitarChipAsync(potreroId, nombre); }
        catch (Exception ex) { TempData["Err"] = ex.Message; }
        return RedirectToAction(nameof(Index));
    }
}