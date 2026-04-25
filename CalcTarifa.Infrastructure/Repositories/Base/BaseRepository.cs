using Microsoft.EntityFrameworkCore;

namespace CalcTarifa.Infrastructure.Repositories.Base
{
    public abstract class BaseRepository<TEntity> where TEntity : class
    {
        protected readonly CalcTarifaDbContext _ctx;
        protected readonly DbSet<TEntity> _dbSet;

        protected BaseRepository(CalcTarifaDbContext ctx)
        {
            _ctx = ctx;
            _dbSet = ctx.Set<TEntity>();
        }

        public virtual async Task<TEntity?> GetByIdAsync(object id) => await _dbSet.FindAsync(id);
        public virtual async Task<IEnumerable<TEntity>> GetAllAsync() => await _dbSet.ToListAsync();
        public virtual async Task AddAsync(TEntity entity) => await _dbSet.AddAsync(entity);
        public virtual void Update(TEntity entity) => _dbSet.Update(entity);
        public virtual void Delete(TEntity entity) => _dbSet.Remove(entity);
    }
}
