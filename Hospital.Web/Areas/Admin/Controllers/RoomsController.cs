using Hospital.Core.Services;
using Hospital.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RoomsController : Controller
    {
        private readonly IRoomService _roomService;
        private readonly IHospitalInfoService _hospitalInfoService;

        public RoomsController(IRoomService roomService, IHospitalInfoService hospitalInfoService)
        {
            _roomService = roomService;
            _hospitalInfoService = hospitalInfoService;
        }

        public IActionResult Index(int pageNumber = 1, int pageSize = 10)
        {
            var rooms = _roomService.GetAll(pageNumber, pageSize);

            return View(rooms);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var genericResponse = await _roomService.GetByIdAsync(id);

            if (!genericResponse.Succeeded)
                return NotFound(genericResponse.Message);

            return View(genericResponse.Result);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var viewModel = new RoomViewModel
            {
                Hospitals = _hospitalInfoService.GetSelectList()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RoomViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Hospitals = _hospitalInfoService.GetSelectList();

                return View(viewModel);
            }

            await _roomService.CreateAsync(viewModel);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var genericResponse = await _roomService.GetByIdAsync(id);

            if (!genericResponse.Succeeded)
                return NotFound(genericResponse.Message);

            var viewModel = new RoomViewModel
            {
                Id = genericResponse.Result!.Id,
                RoomNumber = genericResponse.Result.RoomNumber,
                Type = genericResponse.Result.Type,
                Status = genericResponse.Result.Status,
                HospitalId = genericResponse.Result.HospitalId,
                Hospitals = _hospitalInfoService.GetSelectList()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(RoomViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Hospitals = _hospitalInfoService.GetSelectList();

                return View(viewModel);
            }

            var genericResponse = await _roomService.UpdateAsync(viewModel);

            if (!genericResponse.Succeeded)
            {
                ModelState.AddModelError(string.Empty, genericResponse.Message ?? "An error occurred while updating the room.");
                return View(viewModel);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var isDeleted = await _roomService.DeleteAsync(id);

            if (!isDeleted)
                return BadRequest("An error occurred while deleting the room.");

            return Ok();
        }
    }
}
