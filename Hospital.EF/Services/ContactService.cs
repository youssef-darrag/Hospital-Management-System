using Hospital.Core.Helpers;
using Hospital.Core.Models;
using Hospital.Core.Repositories;
using Hospital.Core.Services;
using Hospital.Core.ViewModels;

namespace Hospital.EF.Services
{
    public class ContactService : IContactService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ContactService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public PagedResult<Contact> GetAll(int pageNumber, int pageSize)
        {
            pageNumber = pageNumber <= 0 ? 1 : pageNumber;
            pageSize = pageSize <= 0 ? 10 : pageSize;

            int excludedRecords = (pageNumber * pageSize) - pageSize;

            var contacts = _unitOfWork.Contacts.GetAll(includeProperties: "Hospital", orderBy: c => c.Hospital.Name);
            var totalRecords = contacts.Count();

            var result = contacts.Skip(excludedRecords).Take(pageSize).ToList();

            return new PagedResult<Contact>
            {
                Data = result.ToList(),
                TotalRecords = totalRecords,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<GenericResponse<Contact>> GetByIdAsync(int id, string includeProperties = "")
        {
            var contact = new Contact();

            if (string.IsNullOrEmpty(includeProperties))
                contact = await _unitOfWork.Contacts.GetByIdAsync(id);
            else
                contact = await _unitOfWork.Contacts.GetByIdAsync(r => r.Id == id, includeProperties);

            if (contact is null)
                return new GenericResponse<Contact> { Message = $"Contact with Id {id} not found." };

            return new GenericResponse<Contact>
            {
                Succeeded = true,
                Result = contact
            };
        }

        public async Task CreateAsync(ContactViewModel viewModel)
        {
            var contact = new Contact
            {
                Email = viewModel.Email,
                Phone = viewModel.Phone,
                HospitalId = viewModel.HospitalId
            };

            await _unitOfWork.Contacts.AddAsync(contact);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<GenericResponse<Contact>> UpdateAsync(ContactViewModel viewModel)
        {
            var contact = await _unitOfWork.Contacts.GetByIdAsync(viewModel.Id);

            if (contact is null)
                return new GenericResponse<Contact> { Message = $"Contact with Id {viewModel.Id} not found." };

            contact.Email = viewModel.Email;
            contact.Phone = viewModel.Phone;
            contact.HospitalId = viewModel.HospitalId;

            _unitOfWork.Contacts.Update(contact);
            var effectedRows = await _unitOfWork.CompleteAsync();

            if (effectedRows <= 0)
                return new GenericResponse<Contact> { Message = "Update failed. No changes were made." };

            return new GenericResponse<Contact>
            {
                Succeeded = true,
                Result = contact
            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var isDeleted = false;

            var contact = await _unitOfWork.Contacts.GetByIdAsync(id);

            if (contact is null)
                return isDeleted;

            _unitOfWork.Contacts.Delete(contact);
            var effectedRows = await _unitOfWork.CompleteAsync();

            if (effectedRows > 0)
                isDeleted = true;

            return isDeleted;
        }
    }
}
