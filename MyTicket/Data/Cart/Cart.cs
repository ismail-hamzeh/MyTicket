using MyTicket.Models;
using Microsoft.EntityFrameworkCore;


namespace MyTicket.Data.Cart
{
    public class Cart
    {
        public AppDbContext db { get; set; }
        public string ShoppingCartId { get; set; }
        public List<ShoppingCart> ShoppingCarts { get; set; }

        public Cart(AppDbContext db)
        {
            this.db = db;
        }

        public static Cart GetShoppingCart(IServiceProvider serviceProvider)
        {
            ISession session = serviceProvider.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;

            var context = serviceProvider.GetService<AppDbContext>();

            string cartId = session.GetString("CartId") ?? Guid.NewGuid().ToString();
            session.SetString("CartId", cartId);

            return new Cart(context) { ShoppingCartId = cartId };
        }

        public void AddItemToCart(Movie movie)
        {
            var shoppingCartItem = db.ShoppingCarts.FirstOrDefault(n => n.Movie.Id == movie.Id && n.ShoppingCartId == ShoppingCartId);

            if (shoppingCartItem == null)
            {
                shoppingCartItem = new ShoppingCart()
                {
                    ShoppingCartId = ShoppingCartId,
                    Movie = movie,
                    Amount = 1
                };

                db.ShoppingCarts.Add(shoppingCartItem);
            }
            else
            {
                shoppingCartItem.Amount++;
            }
            db.SaveChanges();
        }

        public void RemoveItemFromCart(Movie movie)
        {
            var shoppingCartItem = db.ShoppingCarts.FirstOrDefault(n => n.Movie.Id == movie.Id && n.ShoppingCartId == ShoppingCartId);

            if (shoppingCartItem != null)
            {
                if (shoppingCartItem.Amount > 1)
                {
                    shoppingCartItem.Amount--;
                }
                else
                {
                    db.ShoppingCarts.Remove(shoppingCartItem);
                }
            }
            db.SaveChanges();
        }

        public List<ShoppingCart> GetShoppingCartItems()
        {
            return ShoppingCarts ?? (ShoppingCarts = db.ShoppingCarts.Where(n => n.ShoppingCartId == ShoppingCartId).Include(n => n.Movie).ToList());
        }

        public double GetShoppingCartTotal()
        {
            return db.ShoppingCarts.Where(n => n.ShoppingCartId == ShoppingCartId).Select(n => n.Movie.Price * n.Amount).Sum();
        }

        public async Task ClearShoppingCartAsync()
        {
            var items = await db.ShoppingCarts.Where(n => n.ShoppingCartId == ShoppingCartId).ToListAsync();
            db.ShoppingCarts.RemoveRange(items);
            await db.SaveChangesAsync();
        }
    }
}
