namespace Hospital.Core.Models
{
    public class Bill
    {
        public int Id { get; set; }
        public int BillNumber { get; set; }
        public string? PatientId { get; set; } = default!;
        public ApplicationUser Patient { get; set; } = default!;
        public int? InsuranceId { get; set; }
        public Insurance Insurance { get; set; } = default!;
        public int DoctorCharge { get; set; }
        public decimal MedicineCharge { get; set; }
        public decimal RoomCharge { get; set; }
        public decimal OperationCharge { get; set; }
        public int NoOfDays { get; set; }
        public int NursingCharge { get; set; }
        public int LabCharge { get; set; }
        public decimal Advance { get; set; }
        public decimal TotalBill { get; set; }
    }
}