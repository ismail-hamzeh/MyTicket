using Microsoft.AspNetCore.Mvc;
using MyTicket.Data.Cart;

namespace MyTicket.ViewComponents
{
    public class ShoppingCartSummary : ViewComponent
    {
        private readonly Cart cart;

        public ShoppingCartSummary(Cart cart)
        {
            this.cart = cart;
        }

        public IViewComponentResult Invoke()
        {
            var items = cart.GetShoppingCartItems().Count();
            return View(items);
        }
    }
}
