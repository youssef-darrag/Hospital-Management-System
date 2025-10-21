using Hospital.Core;
using Hospital.Core.Helpers;
using Hospital.Core.Models;
using Hospital.Core.Repositories;
using Hospital.Core.Services;
using Hospital.Core.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Hospital.EF.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AppointmentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public List<SlotViewModel> GetSlotsWithStatus(string doctorId, DateOnly date)
        {
            var timing = _unitOfWork.Timings.GetById(t => t.DoctorId == doctorId && t.Date == date);

            if (timing == null)
                return new List<SlotViewModel>();

            var slots = new List<SlotViewModel>();

            // Morning slots
            slots.AddRange(GenerateSlots(timing.MorningShiftStartTime, timing.MorningShiftEndTime, timing.Duration, date, doctorId, timing.Id));

            // Afternoon slots
            slots.AddRange(GenerateSlots(timing.AfternoonShiftStartTime, timing.AfternoonShiftEndTime, timing.Duration, date, doctorId, timing.Id));

            return slots;
        }

        public IEnumerable<SelectListItem> GetSelectList(string doctorId, DateOnly date)
        {
            return GetSlotsWithStatus(doctorId, date)
                .Where(s => !s.IsBooked)
                .OrderByDescending(s => s.Start)
                .Select(s => new SelectListItem { Value = s.Start.ToString("HH:mm"), Text = s.Start.ToString() })
                .ToList();
        }

        public IEnumerable<Appointment> GetAppointmentsByDoctorId(string doctorId)
        {
            var appointments = _unitOfWork.Appointments.GetAll(criteria: a => a.DoctorId == doctorId,
                includeProperties: "Patient", orderBy: a => a.AppointmentDate, thenBy: a => a.StartTime);

            return appointments;
        }

        public async Task<GenericResponse<Appointment>> GetByIdAsync(int id, string includeProperties = "")
        {
            var appointment = new Appointment();

            if (string.IsNullOrEmpty(includeProperties))
                appointment = await _unitOfWork.Appointments.GetByIdAsync(id);
            else
                appointment = await _unitOfWork.Appointments.GetByIdAsync(r => r.Id == id, includeProperties);

            if (appointment is null)
                return new GenericResponse<Appointment> { Message = $"Appointment with Id {id} not found." };

            return new GenericResponse<Appointment>
            {
                Succeeded = true,
                Result = appointment
            };
        }

        public async Task<GenericResponse<Appointment>> CreateAsync(BookAppointmentViewModel viewModel)
        {
            var appointment = new Appointment
            {
                DoctorId = viewModel.DoctorId,
                PatientId = viewModel.PatientId,
                TimingId = viewModel.TimingId,
                AppointmentDate = viewModel.AppointmentDate,
                StartTime = viewModel.StartTime,
                EndTime = viewModel.EndTime ?? default!,
                Description = viewModel.Description,
                Status = Status.Pending
            };

            await _unitOfWork.Appointments.AddAsync(appointment);

            var effectedRows = await _unitOfWork.CompleteAsync();

            if (effectedRows <= 0)
                return new GenericResponse<Appointment> { Message = "Added failed. No changes were made." };

            return new GenericResponse<Appointment>
            {
                Succeeded = true,
                Result = appointment
            };
        }

        public async Task<GenericResponse<Appointment>> UpdateStatus(int id, Status status)
        {
            var genericResponse = await GetByIdAsync(id, "Patient,Doctor");

            if (!genericResponse.Succeeded)
                return new GenericResponse<Appointment> { Message = genericResponse.Message };

            genericResponse.Result!.Status = status;

            var effectedRows = await _unitOfWork.CompleteAsync();

            if (effectedRows <= 0)
                return new GenericResponse<Appointment>
                {
                    Succeeded = false,
                    Message = "Update failed. No changes were made."
                };

            return genericResponse;
        }

        private List<SlotViewModel> GenerateSlots(int startHour, int endHour,
            int duration, DateOnly date, string doctorId, int timingId)
        {
            var slots = new List<SlotViewModel>();

            if (startHour >= endHour)
                return slots;

            var startTime = new TimeOnly(startHour, 0, 0);
            var endTime = new TimeOnly(endHour, 0, 0);

            while (startTime < endTime)
            {
                var slotEnd = startTime.AddMinutes(duration);

                bool isBooked = _unitOfWork.Appointments.GetById(a =>
                    a.DoctorId == doctorId &&
                    a.TimingId == timingId &&
                    a.AppointmentDate == date &&
                    a.StartTime == startTime &&
                    (a.Status == Status.Pending ||
                    a.Status == Status.Confirmed)) != null;

                slots.Add(new SlotViewModel
                {
                    Start = startTime,
                    End = slotEnd,
                    IsBooked = isBooked
                });

                startTime = slotEnd;
            }

            return slots;
        }
    }
}
