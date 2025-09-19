using Hospital.Core.Helpers;
using Hospital.Core.Models;

namespace Hospital.Core.Services
{
    public interface IApplicationUserService
    {
        PagedResult<ListApplicationUserViewModel> GetAll(int pageNumber, int pageSize);
        Task<GenericResponse<ApplicationUser>> GetByIdAsync(string id, string includeProperties = "");
        Task<GenericResponse<ApplicationUser>> UpdateAsync(EditApplicationUserViewModel viewModel);
        Task<bool> DeleteAsync(string id);
    }
}
