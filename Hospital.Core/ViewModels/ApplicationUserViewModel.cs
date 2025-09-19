using Hospital.Core.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace Hospital.Core.ViewModels
{
    public class ApplicationUserViewModel
    {
        public string Name { get; set; } = default!;
        public Gender Gender { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = default!;

        [Display(Name = "Is Doctor")]
        public bool IsDoctor { get; set; }
    }
}

public class ListApplicationUserViewModel : ApplicationUserViewModel
{

}

public class DetailsApplicationUserViewModel : ApplicationUserViewModel
{
    public string Nationality { get; set; } = default!;
    public string? Specialist { get; set; }

    [Display(Name = "Picture Url")]
    public string PictureUrl { get; set; } = default!;
}

public class EditApplicationUserViewModel : DetailsApplicationUserViewModel
{
    public string Id { get; set; } = default!;
    public string Address { get; set; } = default!;

    [DataType(DataType.DateTime)]
    [Display(Name = "Date Of Birth")]
    public DateTime DOB { get; set; }

    [StringLength(100)]
    [DataType(DataType.Password)]
    public string Password { get; set; } = default!;
}