using Hospital.Core.Consts;
using Hospital.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Hospital.EF.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>>? criteria = null,
            Expression<Func<T, object>>? orderBy = null, string orderByDirection = OrderBy.Ascending,
            Expression<Func<T, object>>? thenBy = null, string thenByDirection = OrderBy.Ascending,
            string includeProperties = "")
        {
            IQueryable<T> query = _dbSet;

            if (criteria != null)
                query = query.Where(criteria);

            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                query = query.Include(includeProperty);

            if (orderBy != null)
            {
                query = orderByDirection == OrderBy.Ascending
                    ? query.OrderBy(orderBy)
                    : query.OrderByDescending(orderBy);

                // Then ordering (if exists)
                if (thenBy != null)
                {
                    query = thenByDirection == OrderBy.Ascending
                        ? ((IOrderedQueryable<T>)query).ThenBy(thenBy)
                        : ((IOrderedQueryable<T>)query).ThenByDescending(thenBy);
                }
            }

            return query.ToList();
        }

        public T? GetById(object id)
        {
            return _dbSet.Find(id);
        }

        public T? GetById(Expression<Func<T, bool>> criteria, string includeProperties = "")
        {
            IQueryable<T> query = _dbSet;

            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                query = query.Include(includeProperty);

            return query.FirstOrDefault(criteria);
        }

        public async Task<T?> GetByIdAsync(object id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<T?> GetByIdAsync(Expression<Func<T, bool>> criteria, string includeProperties = "")
        {
            IQueryable<T> query = _dbSet;

            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                query = query.Include(includeProperty);

            return await query.FirstOrDefaultAsync(criteria);
        }

        public void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        public async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            return entity;
        }

        public T Update(T entity)
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            return entity;
        }

        public IEnumerable<T> UpdateRange(IEnumerable<T> entities)
        {
            _dbSet.AttachRange(entities);
            _context.Entry(entities).State = EntityState.Modified;
            return entities;
        }

        public T Delete(T entity)
        {
            if (_context.Entry(entity).State == EntityState.Detached)
            {
                _dbSet.Attach(entity);
            }

            _dbSet.Remove(entity);
            return entity;
        }

        public IEnumerable<T> DeleteRange(IEnumerable<T> entities)
        {
            if (_context.Entry(entities).State == EntityState.Detached)
            {
                _dbSet.AttachRange(entities);
            }

            _dbSet.RemoveRange(entities);
            return entities;
        }

        private bool disposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }
    }
}
