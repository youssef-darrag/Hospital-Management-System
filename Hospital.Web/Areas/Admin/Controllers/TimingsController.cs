using Hospital.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace Hospital.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TimingsController : Controller
    {
        private readonly ITimingService _timingService;

        public TimingsController(ITimingService timingService)
        {
            _timingService = timingService;
        }

        [HttpGet]
        public IActionResult Create()
        {
            var viewModel = new CreateTimingViewModel
            {
                DoctorId = User.FindFirstValue(ClaimTypes.NameIdentifier)!,
                DoctorName = User.FindFirstValue(ClaimTypes.GivenName)!,
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
                MorningShiftStartTimes = ShiftPeriods(1, 10),
                MorningShiftEndTimes = ShiftPeriods(3, 13),
                AfternoonShiftStartTimes = ShiftPeriods(13, 18),
                AfternoonShiftEndTimes = ShiftPeriods(15, 21)
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateTimingViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.DoctorId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
                viewModel.DoctorName = User.FindFirstValue(ClaimTypes.GivenName)!;
                viewModel.Date = DateOnly.FromDateTime(DateTime.Now.AddDays(1));
                viewModel.MorningShiftStartTimes = ShiftPeriods(1, 10);
                viewModel.MorningShiftEndTimes = ShiftPeriods(3, 13);
                viewModel.AfternoonShiftStartTimes = ShiftPeriods(13, 18);
                viewModel.AfternoonShiftEndTimes = ShiftPeriods(15, 21);

                return View(viewModel);
            }

            await _timingService.CreateAsync(viewModel);

            return RedirectToAction(nameof(Index), new { id = viewModel.DoctorId });
        }

        private IEnumerable<SelectListItem> ShiftPeriods(int from, int to)
        {
            List<SelectListItem> selectListItems = new List<SelectListItem>();

            for (int i = from; i <= to; i++)
                selectListItems.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString() });

            return selectListItems;
        }
    }
}
