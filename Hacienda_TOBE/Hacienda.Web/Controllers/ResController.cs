using Hacienda.Application.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hacienda.Web.Controllers;

[Authorize]
public class ResController : Controller
{
    private readonly IResAppService _res;
    private readonly IPotreroAppService _potreros;

    public ResController(IResAppService res, IPotreroAppService potreros)
    {
        _res = res;
        _potreros = potreros;
    }

    public async Task<IActionResult> Index()
    {
        var list = await _res.ListarTodasAsync();
        return View(list);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        ViewBag.Potreros = await _potreros.ListarAsync();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(string potreroId, string nombre, ushort edad, uint peso)
    {
        try
        {
            TempData["Msg"] = await _potreros.AgregarResAsync(potreroId, nombre, edad, peso);
        }
        catch (Exception ex)
        {
            TempData["Err"] = ex.Message;
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Alimentar(string potreroId, string nombre, uint incremento)
    {
        try
        {
            TempData["Msg"] = await _res.AlimentarAsync(potreroId, nombre, incremento);
        }
        catch (Exception ex)
        {
            TempData["Err"] = ex.Message;
        }

        return RedirectToAction(nameof(Index));
    }
}
