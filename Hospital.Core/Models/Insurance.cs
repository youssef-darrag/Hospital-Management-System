namespace Hospital.Core.Models
{
    public class Insurance
    {
        public int Id { get; set; }
        public string PolicyNumber { get; set; } = default!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ICollection<Bill> Bills { get; set; } = new List<Bill>();
    }
}
