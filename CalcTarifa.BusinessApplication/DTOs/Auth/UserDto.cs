namespace CalcTarifa.BusinessApplication.DTOs.Auth
{
    public record UserDto(
        string Email,
        string Nombre,
        IEnumerable<string> Roles);
}
