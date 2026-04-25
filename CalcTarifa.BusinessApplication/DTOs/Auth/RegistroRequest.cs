using CalcTarifa.Domain.Enums;

namespace CalcTarifa.BusinessApplication.DTOs.Auth
{
    public record RegistroRequest(
        string Email, 
        string Password, 
        string ConfirmarPassword,
        string NombreCompleto,
        RolUsuario Rol = RolUsuario.Cliente);
}
