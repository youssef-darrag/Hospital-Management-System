using Hospital.Core.Helpers;
using Hospital.Core.Models;
using Hospital.Core.Repositories;
using Hospital.Core.Services;
using Hospital.Core.ViewModels;

namespace Hospital.EF.Services
{
    public class RoomService : IRoomService
    {
        private readonly IUnitOfWork _unitOfWork;

        public RoomService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public PagedResult<Room> GetAll(int pageNumber, int pageSize)
        {
            pageNumber = pageNumber <= 0 ? 1 : pageNumber;
            pageSize = pageSize <= 0 ? 10 : pageSize;

            int excludedRecords = (pageNumber * pageSize) - pageSize;

            var rooms = _unitOfWork.Rooms.GetAll(includeProperties: "Hospital", orderBy: r => r.RoomNumber);
            var totalRecords = rooms.Count();

            var result = rooms.Skip(excludedRecords).Take(pageSize).ToList();

            return new PagedResult<Room>
            {
                Data = result.ToList(),
                TotalRecords = totalRecords,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<GenericResponse<Room>> GetByIdAsync(int id, string includeProperties = "")
        {
            var room = new Room();

            if (string.IsNullOrEmpty(includeProperties))
                room = await _unitOfWork.Rooms.GetByIdAsync(id);
            else
                room = await _unitOfWork.Rooms.GetByIdAsync(r => r.Id == id, includeProperties);

            if (room is null)
                return new GenericResponse<Room> { Message = $"Room with Id {id} not found." };

            return new GenericResponse<Room>
            {
                Succeeded = true,
                Result = room
            };
        }

        public async Task CreateAsync(RoomViewModel viewModel)
        {
            var room = new Room
            {
                RoomNumber = viewModel.RoomNumber,
                Type = viewModel.Type,
                Status = viewModel.Status,
                HospitalId = viewModel.HospitalId
            };

            await _unitOfWork.Rooms.AddAsync(room);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<GenericResponse<Room>> UpdateAsync(RoomViewModel viewModel)
        {
            var room = await _unitOfWork.Rooms.GetByIdAsync(viewModel.Id);

            if (room is null)
                return new GenericResponse<Room> { Message = $"Room with Id {viewModel.Id} not found." };

            room.RoomNumber = viewModel.RoomNumber;
            room.Type = viewModel.Type;
            room.Status = viewModel.Status;
            room.HospitalId = viewModel.HospitalId;

            _unitOfWork.Rooms.Update(room);
            var effectedRows = await _unitOfWork.CompleteAsync();

            if (effectedRows <= 0)
                return new GenericResponse<Room> { Message = "Update failed. No changes were made." };

            return new GenericResponse<Room>
            {
                Succeeded = true,
                Result = room
            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var isDeleted = false;

            var room = await _unitOfWork.Rooms.GetByIdAsync(id);

            if (room is null)
                return isDeleted;

            _unitOfWork.Rooms.Delete(room);
            var effectedRows = await _unitOfWork.CompleteAsync();

            if (effectedRows > 0)
                isDeleted = true;

            return isDeleted;
        }
    }
}
