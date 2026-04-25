using CalcTarifa.BusinessApplication.Interfaces.Persistence;
using CalcTarifa.Infrastructure.Repositories;

namespace CalcTarifa.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CalcTarifaDbContext _ctx;
        private Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction? _transaction;

        public ITarifaRepository               Tarifas        { get; }
        public IRegistroCalculoWriteRepository RegistrosWrite { get; }
        public IRegistroCalculoReadRepository  RegistrosRead  { get; }
        public IUsuarioRepository             Usuarios       { get; }

        public UnitOfWork(CalcTarifaDbContext ctx, Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> userManager)
        {
            _ctx           = ctx;
            Tarifas        = new TarifaRepository(ctx);
            RegistrosWrite = new RegistroCalculoWriteRepository(ctx);
            RegistrosRead  = new RegistroCalculoReadRepository(ctx);
            Usuarios       = new UsuarioRepository(userManager);
        }

        public Task<int> CommitAsync()  => _ctx.SaveChangesAsync();

        public async Task BeginTransactionAsync()
        {
            _transaction = await _ctx.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _ctx.Dispose();
        }

        public async ValueTask DisposeAsync()
        {
            if (_transaction != null)
                await _transaction.DisposeAsync();
            await _ctx.DisposeAsync();
        }
    }
}
