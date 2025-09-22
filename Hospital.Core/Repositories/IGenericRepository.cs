using Hospital.Core.Consts;
using System.Linq.Expressions;

namespace Hospital.Core.Repositories
{
    public interface IGenericRepository<T> : IDisposable
    {
        IEnumerable<T> GetAll(Expression<Func<T, bool>>? criteria = null,
            Expression<Func<T, object>>? orderBy = null, string orderByDirection = OrderBy.Ascending,
            string includeProperties = "");
        T? GetById(object id);
        T? GetById(Expression<Func<T, bool>> criteria, string includeProperties = "");
        Task<T?> GetByIdAsync(object id);
        Task<T?> GetByIdAsync(Expression<Func<T, bool>> criteria, string includeProperties = "");
        void Add(T entity);
        Task<T> AddAsync(T entity);
        T Update(T entity);
        T Delete(T entity);
    }
}
