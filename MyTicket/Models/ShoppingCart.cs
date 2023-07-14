using System.ComponentModel.DataAnnotations;

namespace MyTicket.Models
{
    public class ShoppingCart
    {
        [Key]
        public int Id { get; set; }
        public int Amount { get; set; }
        public Movie Movie { get; set;}
        public string ShoppingCartId { get; set;}
    }
}
