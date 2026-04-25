namespace CalcTarifa.BusinessApplication.DTOs.Auth
{
    public record AuthResponse(
        string Token,
        UserDto User,
        DateTime Expiracion);
}
