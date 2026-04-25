using CalcTarifa.Domain.Entities;

namespace CalcTarifa.BusinessApplication.Interfaces.Persistence
{
    public interface IRegistroCalculoReadRepository
    {
        Task<IEnumerable<RegistroCalculo>> ObtenerUltimosAsync(int cantidad);
        Task<IEnumerable<RegistroCalculo>> ObtenerPorUsuarioAsync(string userId, int cantidad);
        Task<IEnumerable<RegistroCalculo>> ObtenerPorEmailAsync(string email);
    }
}
