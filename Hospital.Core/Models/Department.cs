namespace Hospital.Core.Models
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public ICollection<ApplicationUser> Employees { get; set; } = new List<ApplicationUser>();
    }
}
