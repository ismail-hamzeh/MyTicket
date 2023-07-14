using System.ComponentModel.DataAnnotations;

namespace MyTicket.ViewModel
{
    public class ProfileViewModel
    {
        [Required]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Full Name is required")]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [Required(AllowEmptyStrings = true)]
        public string Password { get; set; }

        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Password and confirmation password did not match")]
        [Required(AllowEmptyStrings = true)]
        public string ConfirmPassword { get; set; }
    }
}
