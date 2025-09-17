using Hospital.Core.Helpers;
using Hospital.Core.Models;
using Hospital.Core.ViewModels;

namespace Hospital.Core.Services
{
    public interface IRoomService
    {
        PagedResult<RoomViewModel> GetAll(int pageNumber, int pageSize);
        Task<GenericResponse<RoomViewModel>> GetByIdAsync(int id);
        Task CreateAsync(RoomViewModel viewModel);
        Task<GenericResponse<Room>> UpdateAsync(RoomViewModel viewModel);
        Task<bool> DeleteAsync(int id);
    }
}
