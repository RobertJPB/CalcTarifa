using CalcTarifa.Domain.Entities;
using CalcTarifa.Domain.Enums;

namespace CalcTarifa.BusinessApplication.Interfaces.Persistence
{
    public interface ITarifaRepository
    {
        Task<TarifaRegion?> ObtenerPorRegionAsync(RegionEnvio region);
        Task<IEnumerable<TarifaRegion>> ObtenerTodasActivasAsync();
    }
}
