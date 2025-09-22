using Hospital.Core.Consts;
using Hospital.Core.Services;
using Hospital.Core.Settings;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ApplicationUsersController : Controller
    {
        private readonly IApplicationUserService _applicationUserService;

        public ApplicationUsersController(IApplicationUserService applicationUserService)
        {
            _applicationUserService = applicationUserService;
        }

        public IActionResult Index(int pageNumber = 1, int pageSize = 10)
        {
            var users = _applicationUserService.GetAll(pageNumber, pageSize);

            return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id, string returnController = "ApplicationUsers")
        {
            var genericResponse = await _applicationUserService.GetByIdAsync(id);

            if (!genericResponse.Succeeded)
                return NotFound(genericResponse.Message);

            if (genericResponse.Result!.PictureUrl is not null)
            {
                var path = genericResponse.Result.IsDoctor ? ImagePaths.Doctor : ImagePaths.Patient;

                var image = Path.Combine(FileSettings.ImagesPath, path, genericResponse.Result.PictureUrl);

                genericResponse.Result.PictureUrl = image;
            }

            ViewBag.ReturnController = returnController;

            return View(genericResponse.Result);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id, string returnController = "ApplicationUsers")
        {
            var genericResponse = await _applicationUserService.GetByIdAsync(id);

            if (genericResponse is null)
                return NotFound("User not found");

            var viewModel = new EditApplicationUserViewModel
            {
                Id = genericResponse.Result!.Id,
                Name = genericResponse.Result.Name,
                Gender = genericResponse.Result.Gender,
                Nationality = genericResponse.Result.Nationality,
                Address = genericResponse.Result.Address,
                DOB = genericResponse.Result.DOB,
                Specialist = genericResponse.Result.Specialist,
                IsDoctor = genericResponse.Result.IsDoctor
            };

            ViewBag.ReturnController = returnController;

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditApplicationUserViewModel viewModel, string returnController)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            var genericResponse = await _applicationUserService.UpdateAsync(viewModel);

            if (!genericResponse.Succeeded)
            {
                ModelState.AddModelError(string.Empty, genericResponse.Message ?? "An error occurred while updating the user.");
                return View(viewModel);
            }

            return RedirectToAction(nameof(Index), returnController);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            var isDeleted = await _applicationUserService.DeleteAsync(id);

            if (!isDeleted)
                return BadRequest("An error occurred while deleting the user.");

            return Ok();
        }
    }
}
