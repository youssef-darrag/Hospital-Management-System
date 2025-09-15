namespace Hospital.Core.Models
{
    public class HospitalInfo
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Type { get; set; } = default!;
        public string City { get; set; } = default!;
        public string PinCode { get; set; } = default!;
        public string Country { get; set; } = default!;
        public ICollection<Room> Rooms { get; set; } = new List<Room>();
        public ICollection<Contact> Contacts { get; set; } = new List<Contact>();
    }
}
