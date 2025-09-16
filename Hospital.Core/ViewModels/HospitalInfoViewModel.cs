namespace Hospital.Core.ViewModels
{
    public class HospitalInfoViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Type { get; set; } = default!;
        public string City { get; set; } = default!;
        public string PinCode { get; set; } = default!;
        public string Country { get; set; } = default!;
    }
}
