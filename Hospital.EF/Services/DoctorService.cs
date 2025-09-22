using Hospital.Core.Helpers;
using Hospital.Core.Repositories;
using Hospital.Core.Services;
using Hospital.Core.ViewModels;

namespace Hospital.EF.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DoctorService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public PagedResult<DoctorViewModel> GetAll(int pageNumber, int pageSize)
        {
            pageNumber = pageNumber <= 0 ? 1 : pageNumber;
            pageSize = pageSize <= 0 ? 10 : pageSize;

            int excludedRecords = (pageNumber * pageSize) - pageSize;

            var doctors = _unitOfWork.ApplicationUsers.GetAll(criteria: u => u.IsDoctor, orderBy: d => d.Name);
            var totalRecords = doctors.Count();

            var result = doctors.Skip(excludedRecords).Take(pageSize).ToList();

            var viewModel = result.Select(d => new DoctorViewModel
            {
                Id = d.Id,
                Name = d.Name,
                Gender = d.Gender,
                Email = d.Email!,
                Specialist = d.Specialist

            }).ToList();

            return new PagedResult<DoctorViewModel>
            {
                Data = viewModel,
                TotalRecords = totalRecords,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }
    }
}
