using Hacienda.Domain.Entities;
using Hacienda.Domain.Ports;

namespace Hacienda.Infrastructure.Persistence;

public class FileVentaRepository : IVentaRepository
{
    private readonly FileStoragePaths _paths;

    public FileVentaRepository(FileStoragePaths paths) => _paths = paths;

    public Task<IReadOnlyList<Venta>> GetAllAsync(CancellationToken ct = default)
    {
        var list = new List<Venta>();
        if (!File.Exists(_paths.Ventas))
            return Task.FromResult<IReadOnlyList<Venta>>(list);

        foreach (var line in File.ReadAllLines(_paths.Ventas))
        {
            if (string.IsNullOrWhiteSpace(line)) continue;

            var p = line.Split('|');

            // --- Venta de producto derivado ---
            // Formato: PROD|potrero|fecha|nombreProducto|tipoProducto|cantidad|unidad|monto
            if (p.Length >= 8 && p[0].Equals("PROD", StringComparison.OrdinalIgnoreCase))
            {
                if (!DateTime.TryParse(p[2], out var fechaProd)) continue;
                if (!decimal.TryParse(p[5], out var cantidad)) continue;
                if (!decimal.TryParse(p[7], out var montoProd)) continue;

                // Usamos el constructor de producto a través de un producto temporal mínimo
                // (solo para rehidratar la venta; no necesitamos la fábrica aquí)
                var productoTemp = CrearProductoTemporal(p[4], p[3], cantidad, p[6], montoProd);
                if (productoTemp is null) continue;

                list.Add(new Venta(p[1], fechaProd, productoTemp, montoProd));
                continue;
            }

            // --- Venta de res (formato original, 7 campos) ---
            // Formato: potrero|fecha|nombre|peso|edad|tipo|monto
            if (p.Length < 7) continue;
            if (!DateTime.TryParse(p[1], out var fecha)) continue;
            if (!uint.TryParse(p[3], out var peso)) continue;
            if (!ushort.TryParse(p[4], out var edad)) continue;
            if (!decimal.TryParse(p[6], out var monto)) continue;

            list.Add(new Venta(p[0], fecha, p[2], peso, edad, p[5], monto));
        }

        return Task.FromResult<IReadOnlyList<Venta>>(list);
    }

    public async Task AddAsync(Venta venta, CancellationToken ct = default)
    {
        var all = (await GetAllAsync(ct)).ToList();
        all.Add(venta);

        var lines = all.Select(Serializar);
        File.WriteAllLines(_paths.Ventas, lines);
    }

    private static string Serializar(Venta v)
    {
        if (v.EsVentaDeProducto)
        {
            // PROD|potrero|fecha|nombreProducto|tipoProducto|cantidad|unidad|monto
            return $"PROD|{v.PotreroId}|{v.Fecha:yyyy-MM-dd}|{v.NombreProducto}|{v.TipoProducto}|{v.Cantidad}|{v.Unidad}|{v.Monto}";
        }

        // Formato original de res (se mantiene intacto)
        return $"{v.PotreroId}|{v.Fecha:yyyy-MM-dd}|{v.NombreRes}|{v.PesoRes}|{v.EdadRes}|{v.TipoRes}|{v.Monto}";
    }


    /// Crea un ProductoVendible mínimo solo para rehidratar la venta desde archivo.
    /// No usa las fábricas porque aquí solo necesitamos los datos.
    private static ProductoVendible? CrearProductoTemporal(
        string tipo,
        string nombre,
        decimal cantidad,
        string unidad,
        decimal monto)
    {
        // Precio unitario se deriva del monto y la cantidad para no perder información
        var precioUnitario = cantidad > 0 ? monto / cantidad : 0m;

        return tipo.Trim().ToUpperInvariant() switch
        {
            "CARNE"  => new ProductoCarne(nombre, cantidad, unidad, precioUnitario),
            "PIEL"   => new ProductoPiel(nombre, cantidad, unidad, precioUnitario),
            "LACTEO" => new ProductoLacteo(nombre, cantidad, unidad, precioUnitario),
            _        => null
        };
    }
} 