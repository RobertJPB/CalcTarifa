using CalcTarifa.Domain.Entities;
using CalcTarifa.Domain.Enums;

namespace CalcTarifa.BusinessApplication.Interfaces.Persistence
{
    // contrato para persistencia de usuarios
    public interface IUsuarioRepository
    {
        Task<Usuario?> ObtenerPorEmailAsync(string email);
        Task<Usuario?> ObtenerPorIdAsync(string id);
        Task<(bool Succeeded, string? Error)> CrearAsync(Usuario usuario, string password);
        Task<bool> ExisteEmailAsync(string email);
        Task<IEnumerable<string>> ObtenerRolesAsync(string userId);
        Task<bool> VerificarPasswordAsync(Usuario usuario, string password);
    }
}
