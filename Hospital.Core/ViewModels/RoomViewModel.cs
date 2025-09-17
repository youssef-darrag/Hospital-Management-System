using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Hospital.Core.ViewModels
{
    public class RoomViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Room Number")]
        public string RoomNumber { get; set; } = default!;
        public string Type { get; set; } = default!;
        public string Status { get; set; } = default!;

        [Display(Name = "Hospital")]
        public int HospitalId { get; set; }
        public IEnumerable<SelectListItem> Hospitals { get; set; } = Enumerable.Empty<SelectListItem>();
    }
}
