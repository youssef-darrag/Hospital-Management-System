using System.ComponentModel.DataAnnotations;

namespace Hospital.Core.ViewModels
{
    public class DoctorViewModel
    {
        public string Id { get; set; } = default!;
        public string Name { get; set; } = default!;
        public Gender Gender { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = default!;
        public string Specialist { get; set; } = default!;
    }
}
