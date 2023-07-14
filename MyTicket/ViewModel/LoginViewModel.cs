using System.ComponentModel.DataAnnotations;

namespace MyTicket.ViewModel
{
    public class LoginViewModel
    {

        [Required(ErrorMessage = "Email address is required")]
        [Display(Name ="Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
