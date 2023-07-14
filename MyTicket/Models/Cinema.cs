using MyTicket.Data.Repositories.Base;
using System.ComponentModel.DataAnnotations;

namespace MyTicket.Models
{
    public class Cinema : IEntityBase
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Logo")]
        [Required(ErrorMessage = "Cinema Logo Required")]
        public string Logo { get; set; }

        [Display(Name = "Cinema Name")]
        [Required(ErrorMessage = "Cinema Name Required")]
        public string Name { get; set; }

        [Display(Name = "Discription")]
        [Required(ErrorMessage = "Discription Required")]
        public string Description { get; set; }

        public List<Movie> Movies { get; set; }
    }
}
