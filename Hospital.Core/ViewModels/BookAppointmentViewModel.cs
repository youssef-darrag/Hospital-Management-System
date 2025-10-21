using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Hospital.Core.ViewModels
{
    public class BookAppointmentViewModel
    {
        [Required]
        public string DoctorId { get; set; } = default!;

        [Display(Name = "Doctor Name")]
        public string Doctor { get; set; } = default!;

        [Required]
        public string PatientId { get; set; } = default!;

        [Required]
        public int TimingId { get; set; }

        [Required]
        [Display(Name = "Doctor Status")]
        public Status Status { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Appointment Date")]
        public DateOnly AppointmentDate { get; set; }

        [Required]
        [DataType(DataType.Time)]
        [Display(Name = "Start Time")]
        public TimeOnly StartTime { get; set; }

        public IEnumerable<SelectListItem> StartTimes { get; set; } = Enumerable.Empty<SelectListItem>();

        [Required]
        [DataType(DataType.Time)]
        [Display(Name = "End Time")]
        public TimeOnly? EndTime { get; set; }

        public int Duration { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; } = default!;
    }
}
