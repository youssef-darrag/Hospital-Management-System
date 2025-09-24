using Hospital.Core.Models;

namespace Hospital.Core.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<ApplicationUser> ApplicationUsers { get; }
        IGenericRepository<HospitalInfo> HospitalInfos { get; }
        IGenericRepository<Room> Rooms { get; }
        IGenericRepository<Contact> Contacts { get; }
        IGenericRepository<Timing> Timings { get; }

        int Complete();
        Task<int> CompleteAsync();
    }
}
