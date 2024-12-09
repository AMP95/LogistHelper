using System.Linq.Expressions;

namespace Utilities
{
    public interface ICrudRepository: IDisposable
    {
        IEnumerable<T> Get<T>(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "") where T : class;

        T Get<T>(object id) where T : class;

        bool Update<T>(T entity) where T : class;

        bool UpdateRange<T>(IEnumerable<T> entitiesToUpdate) where T : class;

        bool Remove<T>(object id) where T : class;

        bool Remove<T>(T entityToDelete) where T : class;

        bool RemoveRange<T>(IEnumerable<T> entitiesToDelete) where T : class;

    }
}
