using Hospital.Core.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Hospital.Core.ViewModels
{
    public class TimingViewModel
    {
        [Required]
        [Display(Name = "Doctor")]
        public string DoctorId { get; set; } = default!;

        [Display(Name = "Doctor Name")]
        public string DoctorName { get; set; } = default!;

        [Required]
        [DataType(DataType.Date)]
        public DateOnly Date { get; set; }

        [Required]
        [Display(Name = "Morning Shift Start Time")]
        public int? MorningShiftStartTime { get; set; }

        [Required]
        [Display(Name = "Morning Shift End Time")]
        public int? MorningShiftEndTime { get; set; }

        [Required]
        [Display(Name = "Afternoon Shift Start Time")]
        public int? AfternoonShiftStartTime { get; set; }

        [Required]
        [Display(Name = "Afternoon Shift End Time")]
        public int? AfternoonShiftEndTime { get; set; }

        [Required]
        public int? Duration { get; set; }

        [Required]
        public Status Status { get; set; }

        public IEnumerable<SelectListItem> MorningShiftStartTimes { get; set; } = Enumerable.Empty<SelectListItem>();
        public IEnumerable<SelectListItem> MorningShiftEndTimes { get; set; } = Enumerable.Empty<SelectListItem>();
        public IEnumerable<SelectListItem> AfternoonShiftStartTimes { get; set; } = Enumerable.Empty<SelectListItem>();
        public IEnumerable<SelectListItem> AfternoonShiftEndTimes { get; set; } = Enumerable.Empty<SelectListItem>();
    }
}

public class CreateTimingViewModel : TimingViewModel
{

}

public class UpdateTimingViewModel : TimingViewModel
{
    public int Id { get; set; }
}