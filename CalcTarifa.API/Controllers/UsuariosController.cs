using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using CalcTarifa.BusinessApplication.DTOs.Auth;
using CalcTarifa.BusinessApplication.Interfaces.Persistence;

namespace CalcTarifa.API.Controllers
{
    // gestión de perfiles de usuario
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UsuariosController : ControllerBase
    {
        private readonly IUnitOfWork _uow;

        public UsuariosController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        // retorna la información del usuario autenticado
        [HttpGet("me")]
        public async Task<IActionResult> GetProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var usuario = await _uow.Usuarios.ObtenerPorIdAsync(userId);
            if (usuario == null)
                return NotFound(new { mensaje = "usuario no encontrado" });

            var roles = await _uow.Usuarios.ObtenerRolesAsync(userId);
            
            var userDto = new UserDto(usuario.Email, usuario.NombreCompleto, roles);
            return Ok(userDto);
        }
    }
}
