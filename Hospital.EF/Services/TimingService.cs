using Hospital.Core.Consts;
using Hospital.Core.Helpers;
using Hospital.Core.Models;
using Hospital.Core.Repositories;
using Hospital.Core.Services;

namespace Hospital.EF.Services
{
    public class TimingService : ITimingService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TimingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public GenericResponse<IEnumerable<Timing>> GetAll(string id)
        {
            var timings = _unitOfWork.Timings.GetAll(criteria: t => t.DoctorId == id, orderBy: t => t.Date,
                orderByDirection: OrderBy.Descending);

            if (timings == null)
                return new GenericResponse<IEnumerable<Timing>> { Message = $"No timings with this doctor." };

            return new GenericResponse<IEnumerable<Timing>>
            {
                Succeeded = true,
                Result = timings
            };
        }

        public async Task<GenericResponse<Timing>> GetByIdAsync(int id, string includeProperties = "")
        {
            var timing = new Timing();

            if (string.IsNullOrEmpty(includeProperties))
                timing = await _unitOfWork.Timings.GetByIdAsync(id);
            else
                timing = await _unitOfWork.Timings.GetByIdAsync(t => t.Id == id, includeProperties);

            if (timing is null)
                return new GenericResponse<Timing> { Message = $"Timing with Id {id} not found." };

            return new GenericResponse<Timing>
            {
                Succeeded = true,
                Result = timing
            };
        }

        public async Task CreateAsync(CreateTimingViewModel viewModel)
        {
            var timing = new Timing
            {
                DoctorId = viewModel.DoctorId,
                Date = viewModel.Date,
                MorningShiftStartTime = viewModel.MorningShiftStartTime ?? 0,
                MorningShiftEndTime = viewModel.MorningShiftEndTime ?? 0,
                AfternoonShiftStartTime = viewModel.AfternoonShiftStartTime ?? 0,
                AfternoonShiftEndTime = viewModel.AfternoonShiftEndTime ?? 0,
                Duration = viewModel.Duration ?? 0,
                Status = viewModel.Status
            };

            await _unitOfWork.Timings.AddAsync(timing);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<GenericResponse<Timing>> UpdateAsync(UpdateTimingViewModel viewModel)
        {
            var timing = await _unitOfWork.Timings.GetByIdAsync(viewModel.Id);

            if (timing is null)
                return new GenericResponse<Timing> { Message = $"Timing with Id {viewModel.Id} not found." };

            timing.DoctorId = viewModel.DoctorId;
            timing.Date = viewModel.Date;
            timing.MorningShiftStartTime = viewModel.MorningShiftStartTime ?? 0;
            timing.MorningShiftEndTime = viewModel.MorningShiftEndTime ?? 0;
            timing.AfternoonShiftStartTime = viewModel.AfternoonShiftStartTime ?? 0;
            timing.AfternoonShiftEndTime = viewModel.AfternoonShiftEndTime ?? 0;
            timing.Duration = viewModel.Duration ?? 0;
            timing.Status = viewModel.Status;

            _unitOfWork.Timings.Update(timing);
            var effectedRows = await _unitOfWork.CompleteAsync();

            if (effectedRows <= 0)
                return new GenericResponse<Timing> { Message = "Update failed. No changes were made." };

            return new GenericResponse<Timing>
            {
                Succeeded = true,
                Result = timing
            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var isDeleted = false;

            var timing = await _unitOfWork.Timings.GetByIdAsync(id);

            if (timing is null)
                return isDeleted;

            _unitOfWork.Timings.Delete(timing);
            var effectedRows = await _unitOfWork.CompleteAsync();

            if (effectedRows > 0)
                isDeleted = true;

            return isDeleted;
        }
    }
}
