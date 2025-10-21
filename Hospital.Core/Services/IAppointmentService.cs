using Hospital.Core.Helpers;
using Hospital.Core.Models;
using Hospital.Core.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Hospital.Core.Services
{
    public interface IAppointmentService
    {
        List<SlotViewModel> GetSlotsWithStatus(string doctorId, DateOnly date);
        IEnumerable<SelectListItem> GetSelectList(string doctorId, DateOnly date);
        IEnumerable<Appointment> GetAppointmentsByDoctorId(string doctorId);
        Task<GenericResponse<Appointment>> GetByIdAsync(int id, string includeProperties = "");
        Task<GenericResponse<Appointment>> CreateAsync(BookAppointmentViewModel viewModel);
        Task<GenericResponse<Appointment>> UpdateStatus(int id, Status status);
    }
}
