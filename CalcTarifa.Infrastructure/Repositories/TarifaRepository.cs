using Microsoft.EntityFrameworkCore;
using CalcTarifa.BusinessApplication.Interfaces.Persistence;
using CalcTarifa.Domain.Entities;
using CalcTarifa.Domain.Enums;

namespace CalcTarifa.Infrastructure.Repositories
{
    public class TarifaRepository : ITarifaRepository
    {
        private readonly CalcTarifaDbContext _ctx;

        public TarifaRepository(CalcTarifaDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<TarifaRegion?> ObtenerPorRegionAsync(RegionEnvio region) =>
            await _ctx.Tarifas.FirstOrDefaultAsync(t => t.Region == region && t.Activo);

        public async Task<IEnumerable<TarifaRegion>> ObtenerTodasActivasAsync() =>
            await _ctx.Tarifas.Where(t => t.Activo)
                        .OrderBy(t => t.NombrePais)
                        .ToListAsync();
    }
}
