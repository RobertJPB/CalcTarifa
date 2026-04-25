using CalcTarifa.BusinessApplication.Interfaces.Persistence;
using CalcTarifa.Domain.Entities;
using CalcTarifa.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace CalcTarifa.Infrastructure.Repositories
{
    // implementación de repositorio usando asp.net identity
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UsuarioRepository(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Usuario?> ObtenerPorEmailAsync(string email)
        {
            var identityUser = await _userManager.FindByEmailAsync(email);
            return Map(identityUser);
        }

        public async Task<Usuario?> ObtenerPorIdAsync(string id)
        {
            var identityUser = await _userManager.FindByIdAsync(id);
            return Map(identityUser);
        }

        public async Task<(bool Succeeded, string? Error)> CrearAsync(Usuario usuario, string password)
        {
            var identityUser = new ApplicationUser
            {
                UserName = usuario.Email,
                Email = usuario.Email,
                NombreCompleto = usuario.NombreCompleto
            };

            var result = await _userManager.CreateAsync(identityUser, password);
            if (!result.Succeeded)
                return (false, string.Join(", ", result.Errors.Select(e => e.Description)));

            // asignar rol
            var rolStr = usuario.Rol.ToString();
            var rolResult = await _userManager.AddToRoleAsync(identityUser, rolStr);
            
            if (!rolResult.Succeeded)
                return (false, "usuario creado pero no se pudo asignar el rol");

            return (true, null);
        }

        public async Task<bool> ExisteEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user != null;
        }

        public async Task<IEnumerable<string>> ObtenerRolesAsync(string userId)
        {
            var identityUser = await _userManager.FindByIdAsync(userId);
            if (identityUser == null) return [];
            return await _userManager.GetRolesAsync(identityUser);
        }

        public async Task<bool> VerificarPasswordAsync(Usuario usuario, string password)
        {
            var identityUser = await _userManager.FindByIdAsync(usuario.Id);
            if (identityUser == null) return false;
            return await _userManager.CheckPasswordAsync(identityUser, password);
        }

        private Usuario? Map(ApplicationUser? identityUser)
        {
            if (identityUser == null) return null;

            // nota: para el rol, en esta implementación simple, asumimos el rol
            // guardado en el usuario o lo obtenemos de forma diferida.
            // por ahora devolvemos un objeto base.
            return new Usuario(
                identityUser.Id, 
                identityUser.Email!, 
                identityUser.NombreCompleto,
                RolUsuario.Cliente // valor por defecto, se refina en lógica de negocio si es necesario
            );
        }
    }
}
