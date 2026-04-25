using CalcTarifa.BusinessApplication.Interfaces.Persistence;
using CalcTarifa.Domain.Entities;
using CalcTarifa.Infrastructure.Repositories.Base;

namespace CalcTarifa.Infrastructure.Repositories
{
    public class RegistroCalculoWriteRepository : BaseRepository<RegistroCalculo>, IRegistroCalculoWriteRepository
    {
        public RegistroCalculoWriteRepository(CalcTarifaDbContext ctx) : base(ctx) { }

        public async Task AgregarAsync(RegistroCalculo registro) =>
            await AddAsync(registro);
    }
}
