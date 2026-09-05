using Hacienda.Application.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hacienda.Web.Controllers;

[Authorize]
public class VentaController : Controller
{
    private readonly IVentaAppService _ventas;
    private readonly IResAppService _reses;
    private readonly IPotreroAppService _potreros;

    public VentaController(
        IVentaAppService ventas,
        IResAppService reses,
        IPotreroAppService potreros)
    {
        _ventas = ventas;
        _reses = reses;
        _potreros = potreros;
    }

    public async Task<IActionResult> Index()
    {
        var list = await _ventas.ListarAsync();
        ViewBag.TotalVentas = list.Count;
        ViewBag.MontoTotal = list.Sum(v => v.Monto);
        return View(list);
    }

    // ---------- Venta de RES (se mantiene) ----------

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

    // ---------- Venta de PRODUCTO (nuevo) ----------

    [HttpGet]
    public async Task<IActionResult> CreateProducto()
    {
        ViewBag.Potreros = await _potreros.ListarAsync();
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
        string? atributoEspecifico)
    {
        try
        {
            TempData["Msg"] = await _ventas.VenderProductoAsync(
                potreroId,
                tipoProducto,
                nombreProducto,
                cantidad,
                unidad,
                precioUnitario,
                atributoEspecifico);

            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            TempData["Err"] = ex.Message;
            ViewBag.Potreros = await _potreros.ListarAsync();
            return View();
        }
    }
}