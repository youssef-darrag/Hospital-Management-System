using Hospital.Core.Helpers;
using Hospital.Core.Models;
using Hospital.Core.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Hospital.Core.Services
{
    public interface IHospitalInfoService
    {
        PagedResult<HospitalInfoViewModel> GetAll(int pageNumber, int pageSize);
        Task<GenericResponse<HospitalInfoViewModel>> GetByIdAsync(int id);
        Task CreateAsync(HospitalInfoViewModel viewModel);
        Task<GenericResponse<HospitalInfo>> UpdateAsync(HospitalInfoViewModel viewModel);
        bool Delete(int id);
        IEnumerable<SelectListItem> GetSelectList();
    }
}
