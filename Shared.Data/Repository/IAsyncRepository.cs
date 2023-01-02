using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Shared.Data.Repository
{
    public interface IAsyncRepository<T> where T : class
    {
        Task<T> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<T>> ListAllAsync(CancellationToken cancellationToken = default);

        Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);
        Task<int> AddRangeAsync(IList<T> entities, CancellationToken cancellationToken = default);
        void Update(T entity);
        void UpdateRange(IList<T> entities);
        void Delete(T entity);

        Task<T> SingleOrDefaultAsync(Expression<Func<T, bool>> expression,
            CancellationToken cancellationToken = default);

        Task<T> SingleAsync(Expression<Func<T, bool>> expression, CancellationToken cancellationToken = default);
        void DeleteRange(IList<T> entities);

        DbSet<T> Table { get; }

        int Commit();
        Task<int> CommitAsync(CancellationToken cancellationToken);
        Task UpdateAsync(T entity);
    }
}
