using Hacienda.Application.Abstractions;
using Hacienda.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hacienda.Web.Controllers;

[Authorize]
public class VacunaController : Controller
{
    private readonly IHaciendaFachada _hacienda;

    public VacunaController(IHaciendaFachada hacienda) => _hacienda = hacienda;

    public async Task<IActionResult> Index()
    {
        var list = await _hacienda.ListarVacunasDisponiblesAsync();
        return View(list);
    }

    [HttpGet]
    public IActionResult Create() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        string tipo,
        string nombre,
        string lote,
        DateTime fechaVencimiento,
        DateTime fechaAplicacion,
        uint? periodo,
        int? grado)
    {
        try
        {
            if (string.Equals(tipo, "Bacteriana", StringComparison.OrdinalIgnoreCase))
            {
                TempData["Msg"] = await _hacienda.CrearVacunaBacterianaAsync(
                    nombre, lote, fechaVencimiento, fechaAplicacion, periodo ?? 2);
            }
            else
            {
                var g = grado.HasValue && Enum.IsDefined(typeof(GradoAtenuacion), grado.Value)
                    ? (GradoAtenuacion)grado.Value
                    : GradoAtenuacion.Media;
                TempData["Msg"] = await _hacienda.CrearVacunaVivaAsync(
                    nombre, lote, fechaVencimiento, fechaAplicacion, g);
            }
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            TempData["Err"] = ex.Message;
            return View();
        }
    }

    [HttpGet]
    public async Task<IActionResult> Aplicar()
    {
        ViewBag.Potreros = await _hacienda.ListarPotrerosAsync();
        ViewBag.Vacunas = await _hacienda.ListarVacunasDisponiblesAsync();
        ViewBag.Reses = await _hacienda.ListarResesAsync();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Aplicar(string potreroId, string nombreRes, string lote)
    {
        try
        {
            TempData["Msg"] = await _hacienda.AplicarVacunaAsync(potreroId, nombreRes, lote);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            TempData["Err"] = ex.Message;
            ViewBag.Potreros = await _hacienda.ListarPotrerosAsync();
            ViewBag.Vacunas = await _hacienda.ListarVacunasDisponiblesAsync();
            ViewBag.Reses = await _hacienda.ListarResesAsync();
            return View();
        }
    }
}