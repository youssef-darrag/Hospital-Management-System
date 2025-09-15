namespace Hospital.Core.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public string Number { get; set; } = default!;
        public string Type { get; set; } = default!;
        public DateTime CreatedDate { get; set; }
        public string Description { get; set; } = default!;
        public string DoctorId { get; set; } = default!;
        public ApplicationUser Doctor { get; set; } = default!;
        public string PatientId { get; set; } = default!;
        public ApplicationUser Patient { get; set; } = default!;
    }
}