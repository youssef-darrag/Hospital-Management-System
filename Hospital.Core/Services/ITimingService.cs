using Hospital.Core.Helpers;
using Hospital.Core.Models;

namespace Hospital.Core.Services
{
    public interface ITimingService
    {
        GenericResponse<IEnumerable<Timing>> GetAll(string id);
        Task<GenericResponse<Timing>> GetByIdAsync(int id, string includeProperties = "");
        Task CreateAsync(CreateTimingViewModel viewModel);
        Task<GenericResponse<Timing>> UpdateAsync(UpdateTimingViewModel viewModel);
        Task<bool> DeleteAsync(int id);
    }
}
