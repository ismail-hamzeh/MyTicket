using MyTicket.Data.Repositories.Base;
using System.ComponentModel.DataAnnotations;

namespace MyTicket.Models
{
    public class Producer : IEntityBase
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Profile Picture")]
        [Required(ErrorMessage = "Profile Picture Required")]
        public string ProfilePictureUrl { get; set; }

        [Display(Name = "Full Name")]
        [Required(ErrorMessage = "Full Name Required")]
        public string FullName { get; set; }

        [Display(Name = "Bio")]
        [Required(ErrorMessage = "Bio Required")]
        public string Bio { get; set; }

        public List<Movie> Movies { get; set; }
    }
}
