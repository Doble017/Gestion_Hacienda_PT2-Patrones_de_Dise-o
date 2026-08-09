using Hacienda.Application.Abstractions;
using Hacienda.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hacienda.Web.Controllers;

[Authorize]
public class ResController : Controller
{
    private readonly IResAppService _res;
    private readonly IPotreroAppService _potreros;
    private readonly IVentaAppService _ventas;

    public ResController(IResAppService res, IPotreroAppService potreros, IVentaAppService ventas)
    {
        _res = res;
        _potreros = potreros;
        _ventas = ventas;
    }

    public async Task<IActionResult> Index()
    {
        var list = await _res.ListarTodasAsync();
        ViewBag.TotalReses = list.Count;
        ViewBag.TotalPeso = list.Sum(x => (long)x.Res.Peso);
        return View(list);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        ViewBag.Potreros = await _potreros.ListarAsync();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(string potreroId, string nombre, ushort edad, uint peso)
    {
        try
        {
            TempData["Msg"] = await _potreros.AgregarResAsync(potreroId, nombre, edad, peso);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            TempData["Err"] = ex.Message;
            ViewBag.Potreros = await _potreros.ListarAsync();
            return View();
        }
    }

    public async Task<IActionResult> DetalleVacunas(string potreroId, string nombre)
    {
        var res = await _res.BuscarAsync(potreroId, nombre);
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
            TempData["Msg"] = await _res.AlimentarAsync(potreroId, nombre, incremento);
        }
        catch (Exception ex) { TempData["Err"] = ex.Message; }
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Vender(string potreroId, string nombre, decimal monto)
    {
        try
        {
            TempData["Msg"] = await _ventas.VenderResAsync(potreroId, nombre, monto);
        }
        catch (Exception ex) { TempData["Err"] = ex.Message; }
        return RedirectToAction(nameof(Index));
    }
}
