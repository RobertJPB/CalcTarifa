using Microsoft.AspNetCore.Identity;

namespace CalcTarifa.Infrastructure
{
    // usuario de la aplicacion (extiende identityuser)
    public class ApplicationUser : IdentityUser
    {
        // nombre descriptivo del usuario
        public string? NombreCompleto { get; set; }

        // fecha de registro de la cuenta
        public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;
    }
}
