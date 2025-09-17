using Hospital.Core.Helpers;
using Hospital.Core.Models;
using Hospital.Core.ViewModels;

namespace Hospital.Core.Services
{
    public interface IContactService
    {
        PagedResult<Contact> GetAll(int pageNumber, int pageSize);
        Task<GenericResponse<Contact>> GetByIdAsync(int id, string includeProperties = "");
        Task CreateAsync(ContactViewModel viewModel);
        Task<GenericResponse<Contact>> UpdateAsync(ContactViewModel viewModel);
        Task<bool> DeleteAsync(int id);
    }
}
