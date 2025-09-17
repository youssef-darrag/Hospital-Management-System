using Hospital.Core.Models;
using Hospital.Core.Repositories;

namespace Hospital.EF.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public IGenericRepository<ApplicationUser> ApplicationUsers { get; private set; }
        public IGenericRepository<HospitalInfo> HospitalInfos { get; private set; }
        public IGenericRepository<Room> Rooms { get; private set; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;

            ApplicationUsers = new GenericRepository<ApplicationUser>(_context);
            HospitalInfos = new GenericRepository<HospitalInfo>(_context);
            Rooms = new GenericRepository<Room>(_context);
        }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
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
