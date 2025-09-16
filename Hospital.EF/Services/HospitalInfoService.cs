using Hospital.Core.Helpers;
using Hospital.Core.Models;
using Hospital.Core.Repositories;
using Hospital.Core.Services;
using Hospital.Core.ViewModels;

namespace Hospital.EF.Services
{
    public class HospitalInfoService : IHospitalInfoService
    {
        private readonly IUnitOfWork _unitOfWork;

        public HospitalInfoService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public PagedResult<HospitalInfoViewModel> GetAll(int pageNumber, int pageSize)
        {
            pageNumber = pageNumber <= 0 ? 1 : pageNumber;
            pageSize = pageSize <= 0 ? 10 : pageSize;

            int excludedRecords = (pageNumber * pageSize) - pageSize;

            var hospitalInfos = _unitOfWork.HospitalInfos.GetAll();
            var totalRecords = hospitalInfos.Count();

            var result = hospitalInfos.Skip(excludedRecords).Take(pageSize).ToList();

            var viewModels = result.Select(h => new HospitalInfoViewModel
            {
                Id = h.Id,
                Name = h.Name,
                Type = h.Type,
                City = h.City,
                PinCode = h.PinCode,
                Country = h.Country
            }).ToList();

            return new PagedResult<HospitalInfoViewModel>
            {
                Data = viewModels,
                TotalRecords = totalRecords,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<GenericResponse<HospitalInfoViewModel>> GetByIdAsync(int id)
        {
            var hospitalInfo = await _unitOfWork.HospitalInfos.GetByIdAsync(id);

            if (hospitalInfo is null)
                return new GenericResponse<HospitalInfoViewModel> { Message = $"HospitalInfo with Id {id} not found." };

            return new GenericResponse<HospitalInfoViewModel>
            {
                Success = true,
                Result = new()
                {
                    Id = hospitalInfo.Id,
                    Name = hospitalInfo.Name,
                    Type = hospitalInfo.Type,
                    City = hospitalInfo.City,
                    PinCode = hospitalInfo.PinCode,
                    Country = hospitalInfo.Country
                }
            };
        }

        public async Task CreateAsync(HospitalInfoViewModel viewModel)
        {
            var hospitalInfo = new HospitalInfo
            {
                Name = viewModel.Name,
                Type = viewModel.Type,
                City = viewModel.City,
                PinCode = viewModel.PinCode,
                Country = viewModel.Country
            };

            await _unitOfWork.HospitalInfos.AddAsync(hospitalInfo);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<GenericResponse<HospitalInfo>> UpdateAsync(HospitalInfoViewModel viewModel)
        {
            var hospitalInfo = await _unitOfWork.HospitalInfos.GetByIdAsync(viewModel.Id);

            if (hospitalInfo is null)
                return new GenericResponse<HospitalInfo> { Message = $"HospitalInfo with Id {viewModel.Id} not found." };

            hospitalInfo.Name = viewModel.Name;
            hospitalInfo.Type = viewModel.Type;
            hospitalInfo.City = viewModel.City;
            hospitalInfo.PinCode = viewModel.PinCode;
            hospitalInfo.Country = viewModel.Country;

            _unitOfWork.HospitalInfos.Update(hospitalInfo);
            var effectedRows = await _unitOfWork.CompleteAsync();

            if (effectedRows <= 0)
                return new GenericResponse<HospitalInfo> { Message = "Update failed. No changes were made." };

            return new GenericResponse<HospitalInfo>
            {
                Success = true,
                Result = hospitalInfo
            };
        }

        public bool Delete(int id)
        {
            var isDeleted = false;

            var hospitalInfo = _unitOfWork.HospitalInfos.GetById(id);

            if (hospitalInfo is null)
                return isDeleted;

            _unitOfWork.HospitalInfos.Delete(hospitalInfo);
            var effectedRows = _unitOfWork.Complete();

            if (effectedRows > 0)
                isDeleted = true;

            return isDeleted;
        }
    }
}
