namespace Hospital.Core.Models
{
    public class Timing
    {
        public int Id { get; set; }
        public string DoctorId { get; set; } = default!;
        public ApplicationUser Doctor { get; set; } = default!;
        public DateOnly Date { get; set; }
        public int MorningShiftStartTime { get; set; }
        public int MorningShiftEndTime { get; set; }
        public int AfternoonShiftStartTime { get; set; }
        public int AfternoonShiftEndTime { get; set; }
        public int Duration { get; set; } // Duration in minutes
        public Status Status { get; set; }
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    }
}

namespace Hospital.Core
{
    public enum Status
    {
        Available, Pending, Confirmed, Cancelled
    }
}