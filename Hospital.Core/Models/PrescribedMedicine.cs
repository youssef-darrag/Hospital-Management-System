namespace Hospital.Core.Models
{
    public class PrescribedMedicine
    {
        public int Id { get; set; }
        public int MedicineId { get; set; }
        public Medicine Medicine { get; set; } = default!;
        public int PatientReportId { get; set; }
        public PatientReport PatientReport { get; set; } = default!;
    }
}
