using Microsoft.AspNetCore.Mvc;
using ReportesConDapper.Interfaces;
using ReportesConDapper.Models;
using Rotativa.AspNetCore;
using System.Diagnostics;

namespace ReportesConDapper.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IVenta venta;

        public HomeController(ILogger<HomeController> logger,IVenta venta)
        {
            _logger = logger;
            this.venta = venta;
        }

        public async Task<IActionResult> IndexAsync()
        {
            var ventas =  await venta.GetAllVentas();


            return View(ventas);
        }

        public async Task<IActionResult> GenerarPdf(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ventaConDetalles = await venta.GetVentaConDetallesAsync(id.Value);

            if (ventaConDetalles == null)
            {
                return NotFound();
            }

            // Verifica si la venta tiene los datos correctos
            if (!ventaConDetalles.DetallesVenta.Any())
            {
                return BadRequest("No hay detalles para esta venta.");
            }

            return new ViewAsPdf("GenerarPdf", ventaConDetalles) // Cambia `venta` por `ventaConDetalles`
            {
                FileName = $"Venta_{id}.pdf",
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                PageSize = Rotativa.AspNetCore.Options.Size.A4
            };
        }



        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
