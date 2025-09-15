namespace Hospital.Core.Models
{
    public class Medicine
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Type { get; set; } = default!;
        public decimal Cost { get; set; }
        public string Description { get; set; } = default!;
        public ICollection<MedicineReport> MedicineReports { get; set; } = new List<MedicineReport>();
        public ICollection<PrescribedMedicine> PrescribedMedicines { get; set; } = new List<PrescribedMedicine>();
    }
}
