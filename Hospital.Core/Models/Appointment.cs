namespace Hospital.Core.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public DateOnly AppointmentDate { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public Status Status { get; set; }
        public string Description { get; set; } = default!;
        public string DoctorId { get; set; } = default!;
        public ApplicationUser Doctor { get; set; } = default!;
        public string PatientId { get; set; } = default!;
        public ApplicationUser Patient { get; set; } = default!;
        public int TimingId { get; set; }
        public Timing Timing { get; set; } = default!;
    }
}
