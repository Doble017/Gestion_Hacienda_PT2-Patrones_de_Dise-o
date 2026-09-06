using Hacienda.Application.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hacienda.Web.Controllers;

[Authorize]
public class VentaController : Controller
{
    private readonly IHaciendaFachada _hacienda;

    public VentaController(IHaciendaFachada hacienda)
    {
        _hacienda = hacienda;
    }

    public async Task<IActionResult> Index()
    {
        var list = await _hacienda.ListarVentasAsync();
        ViewBag.TotalVentas = list.Count;
        ViewBag.MontoTotal = list.Sum(v => v.Monto);
        return View(list);
    }

    // ---------- Venta de RES ----------

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        ViewBag.Reses = await _hacienda.ListarResesAsync();
        return View();
    }

      [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(string potreroId, string nombreRes, decimal monto, string? estrategiaCobro)
    {
        try
        {
            TempData["Msg"] = await _hacienda.VenderResAsync(potreroId, nombreRes, monto, estrategiaCobro);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            TempData["Err"] = ex.Message;
            ViewBag.Reses = await _hacienda.ListarResesAsync();
            return View();
        }
    }
    
    // ---------- Venta de PRODUCTO ----------

    [HttpGet]
    public async Task<IActionResult> CreateProducto()
    {
        ViewBag.Potreros = await _hacienda.ListarPotrerosAsync();
        return View();
    }

        [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateProducto(
        string potreroId,
        string tipoProducto,
        string nombreProducto,
        decimal cantidad,
        string unidad,
        decimal precioUnitario,
        string? atributoEspecifico,
        string? estrategiaCobro)
    {
        try
        {
            TempData["Msg"] = await _hacienda.VenderProductoAsync(
                potreroId,
                tipoProducto,
                nombreProducto,
                cantidad,
                unidad,
                precioUnitario,
                atributoEspecifico,
                estrategiaCobro);

            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            TempData["Err"] = ex.Message;
            ViewBag.Potreros = await _hacienda.ListarPotrerosAsync();
            return View();
        }
    }
}