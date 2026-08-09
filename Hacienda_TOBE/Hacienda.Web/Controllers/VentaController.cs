using Hacienda.Application.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hacienda.Web.Controllers;

[Authorize]
public class VentaController : Controller
{
    private readonly IVentaAppService _ventas;
    private readonly IResAppService _reses;

    public VentaController(IVentaAppService ventas, IResAppService reses)
    {
        _ventas = ventas;
        _reses = reses;
    }

    public async Task<IActionResult> Index()
    {
        var list = await _ventas.ListarAsync();
        ViewBag.TotalVentas = list.Count;
        ViewBag.MontoTotal = list.Sum(v => v.Monto);
        return View(list);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        ViewBag.Reses = await _reses.ListarTodasAsync();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(string potreroId, string nombreRes, decimal monto)
    {
        try
        {
            TempData["Msg"] = await _ventas.VenderResAsync(potreroId, nombreRes, monto);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            TempData["Err"] = ex.Message;
            ViewBag.Reses = await _reses.ListarTodasAsync();
            return View();
        }
    }
}
