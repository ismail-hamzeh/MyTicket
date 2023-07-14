using MyTicket.Models;

namespace MyTicket.Data.Repositories
{
    public interface IOrderRepo
    {
        Task AddOrderAsync(List<ShoppingCart> shoopingCarts, string userId, string userEmail);
        Task<List<Orders>> GetOrdersByUserIdAndRoleAsync(string userId, string role);
    }
}
