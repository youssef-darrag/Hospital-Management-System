using Hospital.Core.ViewModels;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Hospital.Core.ViewModels
{
    public class ApplicationUserViewModel
    {
        public string Id { get; set; } = default!;
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

public class EditApplicationUserViewModel : ApplicationUserViewModel
{
    public string Nationality { get; set; } = default!;
    public string Address { get; set; } = default!;

    [DataType(DataType.DateTime)]
    [Display(Name = "Date Of Birth")]
    public DateTime DOB { get; set; }
    public string? Specialist { get; set; }

    [DataType(DataType.Password)]
    [Display(Name = "Current Password")]
    public string CurrentPassword { get; set; } = default!;

    [StringLength(100)]
    [DataType(DataType.Password)]
    [Display(Name = "New Password")]
    public string NewPassword { get; set; } = default!;

    [Display(Name = "Picture Url")]
    public IFormFile? PictureUrl { get; set; }
}