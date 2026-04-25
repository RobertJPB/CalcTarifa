using Microsoft.AspNetCore.Mvc;
using CalcTarifa.BusinessApplication.DTOs;
using CalcTarifa.Web.Models;
using CalcTarifa.Web.Services;

namespace CalcTarifa.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IApiClientService _api;

        public HomeController(IApiClientService api) => _api = api;

        // GET /
        public async Task<IActionResult> Index()
        {
            SetApiToken();
            try 
            {
                var vm = await ConstruirVMAsync();
                return View(vm);
            }
            catch (Exception ex)
            {
                // Si la API está caída o hay error, informamos
                var msg = ex is HttpRequestException httpEx && httpEx.StatusCode != null
                          ? $"Error de la API: {ex.Message}"
                          : "No se pudieron cargar los datos iniciales. Verifica que la API esté en línea.";
                
                ModelState.AddModelError(string.Empty, msg);
                return View(new HomeVM { 
                    Tarifas = new List<TarifaVM>() 
                });
            }
        }

        private void SetApiToken()
        {
            var token = User.FindFirst("Token")?.Value;
            _api.SetToken(token);
        }

        // POST /Home/Calcular
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Calcular(HomeVM model)
        {
            SetApiToken();
            var vm = await ConstruirVMAsync();
            vm.Formulario = model.Formulario;

            if (!ModelState.IsValid)
                return View("Index", vm);

            try
            {
                var nombre = User.Identity?.IsAuthenticated == true 
                             ? User.Identity.Name ?? "Usuario" 
                             : "Invitado";
                var email  = User.Identity?.IsAuthenticated == true 
                             ? User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value ?? "anonimo@correo.com" 
                             : "invitado@correo.com";

                var request = new CalcularEnvioRequest
                {
                    NombreCliente = nombre,
                    EmailCliente  = email,
                    Peso          = model.Formulario.Peso!.Value,
                    Unidad        = model.Formulario.Unidad,
                    Region        = model.Formulario.Region!.Value
                };

                var respuesta = await _api.CalcularAsync(request);

                if (respuesta is not null)
                {
                    vm.Resultado = new ResultadoEnvioVM
                    {
                        NombreCliente = respuesta.NombreCliente,
                        PesoKg        = respuesta.PesoKg,
                        Region        = vm.Tarifas.FirstOrDefault(t => t.Codigo == respuesta.Region || t.Region.ToString() == respuesta.Region)?.NombrePais ?? respuesta.Region,
                        TarifaPorKg   = respuesta.TarifaPorKg,
                        CostoTotal    = respuesta.CostoTotal,
                        FechaCalculo  = respuesta.FechaCalculo,
                        PesoVisual    = model.Formulario.PesoIngresado ?? respuesta.PesoKg,
                        Unidad        = model.Formulario.Unidad ?? "kg"
                    };
                    // Recargar historial para incluir el nuevo registro
                    vm.Historial = await CargarHistorialAsync(vm.Tarifas);
                }
            }
            catch (HttpRequestException ex)
            {
                var msg = ex.StatusCode == null 
                          ? "No se pudo conectar con el servidor. Verifica que la API esté en línea." 
                          : $"Error en el cálculo: {ex.Message}";
                ModelState.AddModelError(string.Empty, msg);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error inesperado: {ex.Message}");
            }

            return View("Index", vm);
        }

        // ── Helpers ──────────────────────────────────────────────────────────

        private async Task<HomeVM> ConstruirVMAsync()
        {
            var tarifas = (await _api.ObtenerTarifasAsync())
                .Select(t => new TarifaVM
                {
                    Codigo      = t.Codigo,
                    NombrePais  = t.NombrePais,
                    TarifaPorKg = t.TarifaPorKg,
                    Region      = Enum.TryParse<CalcTarifa.Domain.Enums.RegionEnvio>(t.Codigo, true, out var r) ? r : default
                }).ToList();

            return new HomeVM
            {
                Tarifas = tarifas,
                Historial = await CargarHistorialAsync(tarifas)
            };
        }

        private async Task<List<HistorialItemVM>> CargarHistorialAsync(List<TarifaVM> tarifas) =>
            (await _api.ObtenerHistorialAsync(6))
            .Select(h => new HistorialItemVM
            {
                NombreCliente = h.NombreCliente,
                PesoKg        = h.PesoKg,
                Region        = tarifas.FirstOrDefault(t => t.Codigo == h.Region || t.Region.ToString() == h.Region)?.NombrePais ?? h.Region,
                CostoTotal    = h.CostoTotal,
                FechaCalculo  = h.FechaCalculo
            }).ToList();
    }
}
