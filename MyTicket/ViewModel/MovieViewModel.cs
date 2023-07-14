using MyTicket.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace MyTicket.ViewModel
{
    public class MovieViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="Movie name is required")]
        [Display(Name = "Movie Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Movie description is required")]
        [Display(Name = "Movie Desccription")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Movie price is required")]
        [Display(Name = "Movie Price")]
        public double Price { get; set; }

        [Required(ErrorMessage = "Movie image url is required")]
        [Display(Name = "Movie Image URL")]
        public string ImgUrl { get; set; }

        [Required(ErrorMessage = "Movie start date is required")]
        [Display(Name = "Movie Start Date")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "Movie end date is required")]
        [Display(Name = "Movie End Date")]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "Movie category is required")]
        [Display(Name = "Movie Category")]
        public MovieCategory MovieCategory { get; set; }

        [Required(ErrorMessage = "Select a cinema is required")]
        [Display(Name = "Select a Cinema")]
        public int CinemaId { get; set; }

        [Required(ErrorMessage = "Select a producer is required")]
        [Display(Name = "Select Movie Producer")]
        public int ProducerId{ get; set; }

        [Required(ErrorMessage = "Select Actor(s) is required")]
        [Display(Name = "Select Actor(s)")]
        public List<int> ActorIds { get; set; }
    }
}
