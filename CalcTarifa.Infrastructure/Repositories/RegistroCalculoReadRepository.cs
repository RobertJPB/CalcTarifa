using Microsoft.EntityFrameworkCore;
using CalcTarifa.BusinessApplication.Interfaces.Persistence;
using CalcTarifa.Domain.Entities;
using CalcTarifa.Infrastructure.Repositories.Base;

namespace CalcTarifa.Infrastructure.Repositories
{
    public class RegistroCalculoReadRepository : BaseRepository<RegistroCalculo>, IRegistroCalculoReadRepository
    {
        public RegistroCalculoReadRepository(CalcTarifaDbContext ctx) : base(ctx) { }

        public async Task<IEnumerable<RegistroCalculo>> ObtenerUltimosAsync(int cantidad) =>
            await _ctx.Registros
                     .OrderByDescending(r => r.FechaCalculo)
                     .Take(cantidad)
                     .ToListAsync();

        public async Task<IEnumerable<RegistroCalculo>> ObtenerPorUsuarioAsync(string userId, int cantidad) =>
            await _ctx.Registros
                     .Where(r => r.UserId == userId)
                     .OrderByDescending(r => r.FechaCalculo)
                     .Take(cantidad)
                     .ToListAsync();

        public async Task<IEnumerable<RegistroCalculo>> ObtenerPorEmailAsync(string email) =>
            await _ctx.Registros
                     .Where(r => r.EmailCliente == email)
                     .OrderByDescending(r => r.FechaCalculo)
                     .ToListAsync();
    }
}
