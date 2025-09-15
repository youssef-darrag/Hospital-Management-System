namespace Hospital.Core.Models
{
    public class Room
    {
        public int Id { get; set; }
        public string RoomNumber { get; set; } = default!;
        public string Type { get; set; } = default!;
        public string Status { get; set; } = default!;
        public int HospitalId { get; set; }
        public HospitalInfo Hospital { get; set; } = default!;
    }
}