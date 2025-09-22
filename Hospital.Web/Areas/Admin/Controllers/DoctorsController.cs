using Hospital.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DoctorsController : Controller
    {
        private readonly IDoctorService _doctorService;

        public DoctorsController(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }

        public IActionResult Index(int pageNumber = 1, int pageSize = 10)
        {
            var doctors = _doctorService.GetAll(pageNumber, pageSize);

            return View(doctors);
        }

        [HttpGet]
        public IActionResult Details(string id)
        {
            return RedirectToAction(nameof(Details), "ApplicationUsers", new { id = id, returnController = "Doctors" });
        }

        [HttpGet]
        public IActionResult Edit(string id)
        {
            return RedirectToAction(nameof(Edit), "ApplicationUsers", new { id = id, returnController = "Doctors" });
        }
    }
}
