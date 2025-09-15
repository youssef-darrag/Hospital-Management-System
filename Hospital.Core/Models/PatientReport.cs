namespace Hospital.Core.Models
{
    public class PatientReport
    {
        public int Id { get; set; }
        public string Diagnose { get; set; } = default!;
        public string DoctorId { get; set; } = default!;
        public ApplicationUser Doctor { get; set; } = default!;
        public string PatientId { get; set; } = default!;
        public ApplicationUser Patient { get; set; } = default!;
        public ICollection<PrescribedMedicine> PrescribedMedicines { get; set; } = new List<PrescribedMedicine>();
    }
}
