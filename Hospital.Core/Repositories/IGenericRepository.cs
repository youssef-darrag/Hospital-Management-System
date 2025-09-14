using System.Linq.Expressions;

namespace Hospital.Core.Repositories
{
    public interface IGenericRepository<T> : IDisposable
    {
        IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            string includeProperties = "");
        T? GetById(object id);
        Task<T?> GetByIdAsync(object id);
        void Add(T entity);
        Task<T> AddAsync(T entity);
        T Update(T entity);
        T Delete(T entity);
    }
}
