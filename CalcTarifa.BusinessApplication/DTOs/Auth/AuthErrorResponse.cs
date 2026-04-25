namespace CalcTarifa.BusinessApplication.DTOs.Auth
{
    public record AuthErrorResponse(
        string Mensaje,
        IEnumerable<string> Errores);
}
