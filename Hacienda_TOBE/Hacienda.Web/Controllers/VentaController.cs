using Hacienda.Application.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hacienda.Web.Controllers;

[Authorize]
public class VentaController : Controller
{
    private readonly IVentaAppService _ventas;

    public VentaController(IVentaAppService ventas) => _ventas = ventas;

    public async Task<IActionResult> Index()
    {
        var list = await _ventas.ListarAsync();
        return View(list);
    }

    [HttpPost]
    public async Task<IActionResult> Create(string potreroId, string nombreRes, decimal monto)
    {
        try
        {
            TempData["Msg"] = await _ventas.VenderResAsync(potreroId, nombreRes, monto);
        }
        catch (Exception ex)
        {
            TempData["Err"] = ex.Message;
        }

        return RedirectToAction(nameof(Index));
    }
}
