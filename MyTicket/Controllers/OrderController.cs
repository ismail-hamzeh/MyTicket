using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyTicket.Data.Cart;
using MyTicket.Data.Repositories;
using MyTicket.ViewModel;
using System.Security.Claims;

namespace MyTicket.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {

        private readonly IMovieRepo movieRepo;
        private readonly IOrderRepo orderRepo;
        private readonly Cart cart;

        public OrderController(IMovieRepo movieRepo, IOrderRepo orderRepo, Cart cart)
        {
            this.movieRepo = movieRepo;
            this.orderRepo = orderRepo;
            this.cart = cart;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        public async Task<IActionResult> Orders()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            string userRole = User.FindFirstValue(ClaimTypes.Role);

            var orderItems = await orderRepo.GetOrdersByUserIdAndRoleAsync(userId, userRole);
            return View(orderItems);
        }

        public IActionResult ShoppingCart()
        {
            var items = cart.GetShoppingCartItems();
            cart.ShoppingCarts = items;

            var newCartItem = new ShoppingCartViewModel()
            {
                Cart = cart,
                ShoppingCartTotal = cart.GetShoppingCartTotal(),
            };

            return View(newCartItem);
        }

        public async Task<IActionResult> AddItemToCart(int id)
        {
            var item = await movieRepo.GetMovieByIdAsync(id);

            if(item != null)
            {
                cart.AddItemToCart(item);
            }
            return RedirectToAction(nameof(ShoppingCart));
        }

        public async Task<IActionResult> RemoveItemFromCart(int id)
        {
            var item = await movieRepo.GetMovieByIdAsync(id);

            if (item != null)
            {
                cart.RemoveItemFromCart(item);
            }
            return RedirectToAction(nameof(ShoppingCart));
        }


        public async Task<IActionResult> Payment()
        {
            var items = cart.GetShoppingCartItems();
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var email = User.FindFirstValue(ClaimTypes.Email);  

            await orderRepo.AddOrderAsync(items, userId, email);
            await cart.ClearShoppingCartAsync();

            return View("PaymentCompleted");
        }

        public async Task<IActionResult> PaymentCompleted()
        {
            return View();
        }
    }
}
