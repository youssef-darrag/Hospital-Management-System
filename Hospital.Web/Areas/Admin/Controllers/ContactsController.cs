using Hospital.Core.Services;
using Hospital.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ContactsController : Controller
    {
        private readonly IContactService _contactService;
        private readonly IHospitalInfoService _hospitalInfoService;

        public ContactsController(IContactService contactService, IHospitalInfoService hospitalInfoService)
        {
            _contactService = contactService;
            _hospitalInfoService = hospitalInfoService;
        }

        public IActionResult Index(int pageNumber = 1, int pageSize = 10)
        {
            var contacts = _contactService.GetAll(pageNumber, pageSize);

            return View(contacts);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var genericResponse = await _contactService.GetByIdAsync(id, "Hospital");

            if (!genericResponse.Succeeded)
                return NotFound(genericResponse.Message);

            return View(genericResponse.Result);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var viewModel = new ContactViewModel
            {
                Hospitals = _hospitalInfoService.GetSelectList()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ContactViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Hospitals = _hospitalInfoService.GetSelectList();

                return View(viewModel);
            }

            await _contactService.CreateAsync(viewModel);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var genericResponse = await _contactService.GetByIdAsync(id);

            if (!genericResponse.Succeeded)
                return NotFound(genericResponse.Message);

            var viewModel = new ContactViewModel
            {
                Id = genericResponse.Result!.Id,
                Email = genericResponse.Result.Email,
                Phone = genericResponse.Result.Phone,
                HospitalId = genericResponse.Result.HospitalId,
                Hospitals = _hospitalInfoService.GetSelectList()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ContactViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Hospitals = _hospitalInfoService.GetSelectList();

                return View(viewModel);
            }

            var genericResponse = await _contactService.UpdateAsync(viewModel);

            if (!genericResponse.Succeeded)
            {
                ModelState.AddModelError(string.Empty, genericResponse.Message ?? "An error occurred while updating the contact.");
                return View(viewModel);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var isDeleted = await _contactService.DeleteAsync(id);

            if (!isDeleted)
                return BadRequest("An error occurred while deleting the contact.");

            return Ok();
        }
    }
}
