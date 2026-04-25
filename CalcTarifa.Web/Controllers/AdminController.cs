using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CalcTarifa.Web.Services;
using CalcTarifa.Web.Models;

namespace CalcTarifa.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IApiClientService _api;

        public AdminController(IApiClientService api) => _api = api;

        // GET /Admin
        public async Task<IActionResult> Index()
        {
            var token = User.FindFirst("Token")?.Value;
            _api.SetToken(token);

            var tarifas = await _api.ObtenerTarifasAsync();
            var model = tarifas.Select(t => new TarifaVM
            {
                Codigo      = t.Codigo,
                NombrePais  = t.NombrePais,
                TarifaPorKg = t.TarifaPorKg,
                Region      = Enum.TryParse<CalcTarifa.Domain.Enums.RegionEnvio>(t.Codigo, true, out var r) ? r : default
            }).ToList();

            return View(model);
        }

        // POST /Admin/Actualizar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Actualizar(int region, decimal nuevaTarifa)
        {
            var token = User.FindFirst("Token")?.Value;
            _api.SetToken(token);

            var success = await _api.ActualizarTarifaAsync(region, nuevaTarifa);
            
            if (success)
                TempData["Success"] = "Tarifa actualizada correctamente.";
            else
                TempData["Error"] = "No se pudo actualizar la tarifa.";

            return RedirectToAction(nameof(Index));
        }

        // POST /Admin/Eliminar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Eliminar(int region)
        {
            var token = User.FindFirst("Token")?.Value;
            _api.SetToken(token);

            var success = await _api.EliminarTarifaAsync(region);

            if (success)
                TempData["Success"] = "Tarifa desactivada correctamente.";
            else
                TempData["Error"] = "No se pudo desactivar la tarifa.";

            return RedirectToAction(nameof(Index));
        }
    }
}
