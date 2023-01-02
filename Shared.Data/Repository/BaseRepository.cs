using Microsoft.EntityFrameworkCore;
using Shared.Data.Contexts;
using System.Linq.Expressions;

namespace Shared.Data.Repository
{
    public class BaseRepository<T> : IAsyncRepository<T> where T : class
    {
        private readonly DbContext _dbContext;
        private readonly DbSet<T> DbSet;
        public BaseRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            DbSet = dbContext.Set<T>();

        }
        public DbSet<T> Table => DbSet;

        public async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            await _dbContext.Set<T>().AddAsync(entity, cancellationToken);
            return entity;
        }

        public async Task<int> AddRangeAsync(IList<T> entities, CancellationToken cancellationToken = default)
        {
            await _dbContext.Set<T>().AddRangeAsync(entities, cancellationToken);
            return entities.Count;
        }

        public void Delete(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
        }

        public void DeleteRange(IList<T> entities)
        {
            _dbContext.Set<T>().RemoveRange(entities);
        }

        public async Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var keyValues = new object[] { id };
            return await _dbContext.Set<T>().FindAsync(keyValues, cancellationToken);
        }

        public async Task<IReadOnlyList<T>> ListAllAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.Set<T>().ToListAsync(cancellationToken);
        }

        public async Task<T> SingleAsync(Expression<Func<T, bool>> expression, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Set<T>().SingleAsync(expression, cancellationToken);
        }

        public async Task<T?> SingleOrDefaultAsync(Expression<Func<T, bool>> expression, CancellationToken cancellationToken = default) => await _dbContext.Set<T>().SingleOrDefaultAsync(expression, cancellationToken);

        public void Update(T entity)
        {
            _dbContext.Update(entity);
        }
        public async Task UpdateAsync(T entity)
        {
            _dbContext.Update(entity);
        }

        public void UpdateRange(IList<T> entities)
        {
            _dbContext.UpdateRange(entities);
        }
        public int Commit()
        {
            return _dbContext.SaveChanges();
        }

        public async Task<int> CommitAsync(CancellationToken cancellationToken)
        {
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }

    }
}
