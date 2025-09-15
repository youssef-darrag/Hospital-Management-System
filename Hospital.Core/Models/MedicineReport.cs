namespace Hospital.Core.Models
{
    public class MedicineReport
    {
        public int Id { get; set; }
        public int SupplierId { get; set; }
        public Supplier Supplier { get; set; } = default!;
        public int MedicineId { get; set; }
        public Medicine Medicine { get; set; } = default!;
        public string Company { get; set; } = default!;
        public int Quantity { get; set; }
        public DateTime ProductionDate { get; set; }
        public DateTime ExpireDate { get; set; }
        public string Country { get; set; } = default!;
    }
}
