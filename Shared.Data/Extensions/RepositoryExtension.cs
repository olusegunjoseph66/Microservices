using LinqKit;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Shared.Data.Extensions
{
    public static class RepositoryExtension
    {
        public static IQueryable<T> IncludeMultiple<T>(this IQueryable<T> query, params Expression<Func<T, object>>[] includes)
            where T : class
        {
            if (includes != null)
            {
                query = includes.Aggregate(query,
                    (current, include) => current.Include(include));
            }

            return query;
        }


        public static IQueryable<T> Paginate<T>(this IQueryable<T> query, int page, int pageSize)
        {
            if (page <= 0) page = 1;
            if (pageSize <= 0) pageSize = 10;
            query = query.Skip((page - 1) * pageSize).Take(pageSize);
            return query;
        }

        public static IQueryable<T> OrderByWhere<T>(this IQueryable<T> query, Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy)
        {
            if (orderBy != null)
                query = orderBy(query);
            if (predicate != null)
                query = query.AsExpandable().Where(predicate);
            return query;
        }

    }
}
