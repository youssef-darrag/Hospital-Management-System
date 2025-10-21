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
        IGenericRepository<Appointment> Appointments { get; }
        IGenericRepository<Notification> Notifications { get; }

        int Complete();
        Task<int> CompleteAsync();
    }
}
