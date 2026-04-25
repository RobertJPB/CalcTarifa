using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using CalcTarifa.Web.Models;
using CalcTarifa.Web.Services;

namespace CalcTarifa.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IApiClientService _api;

        public AuthController(IApiClientService api) => _api = api;

        [HttpGet]
        public IActionResult Login() => View(new LoginViewModel());

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var result = await _api.LoginAsync(model.Email, model.Password);
            if (result.Succeeded)
            {
                await SignUserIn(model.Email, result.Nombre ?? model.Email, result.Token!, result.Roles);
                return RedirectToAction("Index", "Home");
            }

            model.ErrorMensaje = result.Error;
            return View(model);
        }

        [HttpGet]
        public IActionResult Registro() => View(new RegistroViewModel());

        [HttpPost]
        public async Task<IActionResult> Registro(RegistroViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var result = await _api.RegistroAsync(model.Email, model.NombreCompleto, model.Password, model.ConfirmarPassword, 1);
            if (result.Succeeded)
            {
                await SignUserIn(model.Email, result.Nombre ?? model.NombreCompleto, result.Token!, result.Roles);
                return RedirectToAction("Index", "Home");
            }

            model.Errores = result.Errors;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        private async Task SignUserIn(string email, string nombre, string token, IEnumerable<string>? roles)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name,  nombre),
                new Claim(ClaimTypes.Email, email),
                new Claim("Token",          token)
            };

            if (roles != null)
            {
                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }
            }

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }
    }
}
