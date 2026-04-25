using Microsoft.AspNetCore.Mvc;
using CalcTarifa.BusinessApplication.DTOs.Auth;
using CalcTarifa.BusinessApplication.Interfaces.Persistence;
using CalcTarifa.BusinessApplication.Interfaces.Services;
using CalcTarifa.Domain.Entities;

namespace CalcTarifa.API.Controllers
{
    // gestión de autenticación y registro
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUnitOfWork _uow;
        private readonly ITokenService _tokenService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(
            IUnitOfWork uow,
            ITokenService tokenService,
            ILogger<AuthController> logger)
        {
            _uow = uow;
            _tokenService = tokenService;
            _logger = logger;
        }

        // POST api/auth/registro
        [HttpPost("registro")]
        public async Task<IActionResult> Registro([FromBody] RegistroRequest request)
        {
            if (request.Password != request.ConfirmarPassword)
                return BadRequest(new AuthErrorResponse("las contraseñas no coinciden", []));

            var nuevoUsuario = new Usuario(
                Guid.NewGuid().ToString(),
                request.Email,
                request.NombreCompleto,
                request.Rol
            );

            var (succeeded, error) = await _uow.Usuarios.CrearAsync(nuevoUsuario, request.Password);

            if (!succeeded)
            {
                _logger.LogWarning("registro fallido para {Email}: {Error}", request.Email, error);
                return BadRequest(new AuthErrorResponse("no se pudo crear la cuenta", [error ?? "error desconocido"]));
            }

            _logger.LogInformation("nuevo usuario registrado: {Email}", request.Email);

            var roles = await _uow.Usuarios.ObtenerRolesAsync(nuevoUsuario.Id);
            var token = await _tokenService.GenerarTokenAsync(nuevoUsuario, roles);
            
            var userDto = new UserDto(nuevoUsuario.Email, nuevoUsuario.NombreCompleto, roles);
            return Ok(new AuthResponse(token.Token, userDto, token.Expiracion));
        }

        // POST api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var usuario = await _uow.Usuarios.ObtenerPorEmailAsync(request.Email);
            if (usuario == null || !await _uow.Usuarios.VerificarPasswordAsync(usuario, request.Password))
            {
                _logger.LogWarning("intento de login fallido para {Email}", request.Email);
                return Unauthorized(new AuthErrorResponse("credenciales incorrectas", []));
            }

            _logger.LogInformation("login exitoso: {Email}", request.Email);
            
            var roles = await _uow.Usuarios.ObtenerRolesAsync(usuario.Id);
            var token = await _tokenService.GenerarTokenAsync(usuario, roles);

            var userDto = new UserDto(usuario.Email, usuario.NombreCompleto, roles);
            return Ok(new AuthResponse(token.Token, userDto, token.Expiracion));
        }
    }
}
