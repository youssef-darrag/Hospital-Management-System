namespace Hospital.Core.Models
{
    public class Supplier
    {
        public int Id { get; set; }
        public string Company { get; set; } = default!;
        public string Phone { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Address { get; set; } = default!;
        public ICollection<MedicineReport> MedicineReports { get; set; } = new List<MedicineReport>();
    }
}
