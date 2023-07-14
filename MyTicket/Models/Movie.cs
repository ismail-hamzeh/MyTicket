using MyTicket.Data.Enums;
using MyTicket.Data.Repositories.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyTicket.Models
{
    public class Movie : IEntityBase
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public string ImageUrl { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public MovieCategory MovieCategory { get; set; }

        public List<Actor_Movie> Actor_Movies { get; set; }

        public int CinemaId { get; set; }

        [ForeignKey(nameof(CinemaId))]
        public Cinema Cinema { get; set; }

        public int ProducerId { get; set; }

        [ForeignKey(nameof(ProducerId))]
        public Producer Producer { get; set; }
    }
}
