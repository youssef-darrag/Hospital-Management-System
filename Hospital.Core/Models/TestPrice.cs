namespace Hospital.Core.Models
{
    public class TestPrice
    {
        public int Id { get; set; }
        public string TestCode { get; set; } = default!;
        public decimal Price { get; set; }
        public int LabId { get; set; }
        public Lab Lab { get; set; } = default!;
        public int? BillId { get; set; }
        public Bill Bill { get; set; } = default!;
    }
}
