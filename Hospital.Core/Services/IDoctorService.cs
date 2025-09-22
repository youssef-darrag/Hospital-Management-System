using Hospital.Core.Helpers;
using Hospital.Core.ViewModels;

namespace Hospital.Core.Services
{
    public interface IDoctorService
    {
        PagedResult<DoctorViewModel> GetAll(int pageNumber, int pageSize);
    }
}
