using Hospital.Core.Models;

namespace Hospital.Core.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<ApplicationUser> ApplicationUsers { get; }
        IGenericRepository<HospitalInfo> HospitalInfos { get; }

        int Complete();
        Task<int> CompleteAsync();
    }
}
