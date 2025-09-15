using Microsoft.AspNetCore.Identity;

namespace Hospital.Core.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; } = default!;
        public Gender Gender { get; set; }
        public string Nationality { get; set; } = default!;
        public string Address { get; set; } = default!;
        public DateTime DOB { get; set; }
        public string Specialist { get; set; } = default!;
        public int? DepartmentId { get; set; }
        public Department Department { get; set; } = default!;
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
        public ICollection<Payroll> Payrolls { get; set; } = new List<Payroll>();
    }
}

namespace Hospital.Core
{
    public enum Gender
    {
        Male, Female
    }
}