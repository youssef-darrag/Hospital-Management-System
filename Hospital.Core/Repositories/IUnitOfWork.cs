using Hospital.Core.Models;

namespace Hospital.Core.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<ApplicationUser> ApplicationUsers { get; }

        int Complete();
        Task<int> CompleteAsync();
    }
}
