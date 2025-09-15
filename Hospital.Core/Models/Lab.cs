namespace Hospital.Core.Models
{
    public class Lab
    {
        public int Id { get; set; }
        public string LabNumber { get; set; } = default!;
        public string? PatientId { get; set; }
        public ApplicationUser Patient { get; set; } = default!;
        public string TestType { get; set; } = default!;
        public string TestCode { get; set; } = default!;
        public decimal Weight { get; set; }
        public decimal Height { get; set; }
        public int BloodPressure { get; set; }
        public decimal Temperature { get; set; }
        public string TestResult { get; set; } = default!;
    }
}
