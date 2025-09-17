using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Hospital.Core.ViewModels
{
    public class ContactViewModel
    {
        public int Id { get; set; }
        public string Email { get; set; } = default!;
        public string Phone { get; set; } = default!;

        [Display(Name = "Hospital")]
        public int HospitalId { get; set; }
        public IEnumerable<SelectListItem> Hospitals { get; set; } = Enumerable.Empty<SelectListItem>();
    }
}
