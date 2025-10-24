using Hospital.Core;
using Hospital.Core.Consts;
using Hospital.Core.Hubs;
using Hospital.Core.Models;
using Hospital.Core.Services;
using Hospital.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace Hospital.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AppointmentsController : Controller
    {
        private readonly IAppointmentService _appointmentService;
        private readonly ITimingService _timingService;
        private readonly INotificationService _notificationService;

        public AppointmentsController(IAppointmentService appointmentService, ITimingService timingService,
            INotificationService notificationService)
        {
            _appointmentService = appointmentService;
            _timingService = timingService;
            _notificationService = notificationService;
        }

        [HttpGet]
        public IActionResult DoctorAppointments(string doctorId)
        {
            var appointments = _appointmentService.GetAppointmentsByDoctorId(doctorId);

            return View(appointments);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int id, Status status,
            [FromServices] IHubContext<NotificationHub> hubContext)
        {
            var genericResponseAppointment = await _appointmentService.UpdateStatus(id, status);

            if (!genericResponseAppointment.Succeeded)
                return BadRequest(genericResponseAppointment.Message);

            #region Notification Logic
            string message = status switch
            {
                Status.Confirmed => $"✅ Confirmed: Your appointment with {genericResponseAppointment.Result!.Doctor.Name} on {genericResponseAppointment.Result.AppointmentDate:dd MMM yyyy} at {genericResponseAppointment.Result.StartTime} has been confirmed.",
                Status.Cancelled => $"❌ Cancelled: Your appointment with {genericResponseAppointment.Result!.Doctor.Name} on {genericResponseAppointment.Result.AppointmentDate:dd MMM yyyy} at {genericResponseAppointment.Result.StartTime} has been cancelled.",
                _ => null!
            };

            if (message != null)
            {
                // Add notification to DB.
                var notification = new Notification
                {
                    UserId = genericResponseAppointment.Result!.Patient.Id,
                    Message = message
                };

                var notificationResponse = await _notificationService.CreateAsync(notification);

                if (notificationResponse.Succeeded)
                {
                    // 🔥 إرسال الإشعار فوراً بدون تأخير
                    await hubContext.Clients.User(genericResponseAppointment.Result.Patient.Id)
                        .SendAsync("ReceiveNotification", message);

                    // 🔥 تحديث العدد فوراً
                    var unreadCountResponse = _notificationService.GetUnreadCount(genericResponseAppointment.Result.Patient.Id);
                    if (unreadCountResponse.Succeeded)
                    {
                        await hubContext.Clients.User(genericResponseAppointment.Result.Patient.Id)
                            .SendAsync("UnreadCountUpdated", unreadCountResponse.Result);
                    }
                }
            }
            #endregion

            return RedirectToAction(nameof(DoctorAppointments), new { doctorId = genericResponseAppointment.Result!.DoctorId });
        }

        [Authorize(Roles = $"{WebSiteRoles.Patient},{WebSiteRoles.Doctor},{WebSiteRoles.Admin}")]
        [HttpGet]
        public async Task<IActionResult> Book(string doctorId)
        {
            var genericResponseTiming = await _timingService.GetTimingByDoctorIdAsync(doctorId, "Doctor");

            if (!genericResponseTiming.Succeeded)
                return NotFound(genericResponseTiming.Message);

            var viewModel = new BookAppointmentViewModel
            {
                DoctorId = genericResponseTiming.Result!.DoctorId,
                Doctor = genericResponseTiming.Result.Doctor.Name,
                TimingId = genericResponseTiming.Result.Id,
                AppointmentDate = genericResponseTiming.Result.Date,
                Status = genericResponseTiming.Result.Status,
                PatientId = User.FindFirstValue(ClaimTypes.NameIdentifier)!,
                StartTimes = _appointmentService.GetSelectList(doctorId, genericResponseTiming.Result.Date),
                Duration = genericResponseTiming.Result.Duration
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Book(BookAppointmentViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                var genericResponseTiming = await _timingService.GetTimingByDoctorIdAsync(viewModel.DoctorId, "Doctor");

                if (!genericResponseTiming.Succeeded)
                    return NotFound(genericResponseTiming.Message);

                viewModel.DoctorId = genericResponseTiming.Result!.DoctorId;
                viewModel.Doctor = genericResponseTiming.Result.Doctor.Name;
                viewModel.TimingId = genericResponseTiming.Result.Id;
                viewModel.AppointmentDate = genericResponseTiming.Result.Date;
                viewModel.Status = genericResponseTiming.Result.Status;
                viewModel.PatientId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
                viewModel.StartTimes = _appointmentService.GetSelectList(viewModel.DoctorId, genericResponseTiming.Result.Date);
                viewModel.Duration = genericResponseTiming.Result.Duration;

                return View(viewModel);
            }

            var genericResponseAppointment = await _appointmentService.CreateAsync(viewModel);

            return RedirectToAction(nameof(Confirmation), new { appointmentId = genericResponseAppointment.Result!.Id });
        }

        public async Task<IActionResult> Confirmation(int appointmentId)
        {
            var genericResponseAppointment = await _appointmentService
                .GetByIdAsync(appointmentId, "Doctor,Patient");

            if (!genericResponseAppointment.Succeeded)
                return NotFound(genericResponseAppointment.Message);

            return View(genericResponseAppointment.Result);
        }
    }
}
