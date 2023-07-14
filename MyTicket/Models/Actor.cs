using MyTicket.Data.Repositories.Base;
using System.ComponentModel.DataAnnotations;

namespace MyTicket.Models
{
    public class Actor : IEntityBase
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Profile Picture")]
        [Required(ErrorMessage = "Profile Picture Required")]
        public string ProfilePictureUrl { get; set; }

        [Display(Name = "Full Name")]
        [Required(ErrorMessage = "FullName Required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "FullName must be between 3 and 50 chars")]
        public string FullName { get; set; }

        [Display(Name = "Bio")]
        [Required(ErrorMessage = "Bio Required")]
        public string Bio { get; set; }

        public List<Actor_Movie> Actor_Movies { get; set; }
    }
}
