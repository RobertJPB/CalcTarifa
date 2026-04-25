using CalcTarifa.Domain.Entities;

namespace CalcTarifa.BusinessApplication.Interfaces.Services
{
    // contrato para generación de tokens
    public interface ITokenService
    {
        Task<(string Token, DateTime Expiracion)> GenerarTokenAsync(Usuario usuario, IEnumerable<string> roles);
    }
}
