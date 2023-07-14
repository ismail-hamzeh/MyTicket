using Microsoft.EntityFrameworkCore;
using MyTicket.Models;

namespace MyTicket.Data.Repositories
{
    public class OrderRepo : IOrderRepo
    {

        private readonly AppDbContext db;

        public OrderRepo(AppDbContext db)
        {
            this.db = db;
        }

        public async Task AddOrderAsync(List<ShoppingCart> shoopingCarts, string userId, string userEmail)
        {
            var order = new Orders
            {
                UserId = userId,
                Email = userEmail
            };
            await db.Orders.AddAsync(order);
            await db.SaveChangesAsync();

            foreach(var item in shoopingCarts)
            {
                var orderItem = new OrderItem
                {
                    OrderId = order.Id,
                    MovieId = item.Movie.Id,
                    Amount = item.Amount,
                    Price = item.Movie.Price
                };
                await db.OrderItems.AddAsync(orderItem);
            }
            await db.SaveChangesAsync();

        }

        public async Task<List<Orders>> GetOrdersByUserIdAndRoleAsync(string userId, string userRole)
        {
            var orders = await db.Orders.Include(n => n.OrderItems).ThenInclude(n => n.Movie).Include(n => n.User).ToListAsync();

            if(userRole != "Admin")
            {
                orders = orders.Where(o => o.UserId == userId).ToList();
            }

            return orders;
        }
    }
}
