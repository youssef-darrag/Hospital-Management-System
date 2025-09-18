using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hospital.Core.Models
{
    public class ApplicationUser : IdentityUser
    {
        public Gender Gender { get; set; }
        public string Nationality { get; set; } = default!;
        public string Address { get; set; } = default!;
        public DateTime DOB { get; set; }
        public string Specialist { get; set; } = default!;
        public bool IsDoctor { get; set; }
        public string PictureUrl { get; set; } = default!;
        public int? DepartmentId { get; set; }
        public Department Department { get; set; } = default!;

        [NotMapped]
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
        public ICollection<Payroll> Payrolls { get; set; } = new List<Payroll>();

        [NotMapped]
        public ICollection<PatientReport> PatientReports { get; set; } = new List<PatientReport>();
    }
}

namespace Hospital.Core
{
    public enum Gender
    {
        Male, Female
    }
}