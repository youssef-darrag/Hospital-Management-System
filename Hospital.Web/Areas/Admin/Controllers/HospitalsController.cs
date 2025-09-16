using Hospital.Core.Services;
using Hospital.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HospitalsController : Controller
    {
        private readonly IHospitalInfoService _hospitalInfoService;

        public HospitalsController(IHospitalInfoService hospitalInfoService)
        {
            _hospitalInfoService = hospitalInfoService;
        }

        public IActionResult Index(int pageNumber = 1, int pageSize = 10)
        {
            var hospitals = _hospitalInfoService.GetAll(pageNumber, pageSize);

            return View(hospitals);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var genericResponse = await _hospitalInfoService.GetByIdAsync(id);

            if (!genericResponse.Succeeded)
                return NotFound(genericResponse.Message);

            return View(genericResponse.Result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(HospitalInfoViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            await _hospitalInfoService.CreateAsync(viewModel);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var genericResponse = await _hospitalInfoService.GetByIdAsync(id);

            if (!genericResponse.Succeeded)
                return NotFound(genericResponse.Message);

            return View(genericResponse.Result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(HospitalInfoViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            var genericResponse = await _hospitalInfoService.UpdateAsync(viewModel);

            if (!genericResponse.Succeeded)
            {
                ModelState.AddModelError(string.Empty, genericResponse.Message ?? "An error occurred while updating the hospital info.");
                return View(viewModel);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var isDeleted = _hospitalInfoService.Delete(id);

            if (!isDeleted)
                return BadRequest($"Failed to delete hospital info with Id {id}.");

            return Ok();
        }
    }
}
