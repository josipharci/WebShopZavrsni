using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using ZavrsniRadWebShop.Models;
using ZavrsniRadWebShop.Data;

namespace ZavrsniRadWebShop.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ShoppingCartController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Index()
        {
            var session = _httpContextAccessor.HttpContext.Session;
            var cartItemsJson = session.GetString("CartItems");
            var cartItems = string.IsNullOrEmpty(cartItemsJson)
                ? new List<CartItem>()
                : JsonConvert.DeserializeObject<List<CartItem>>(cartItemsJson);

            var shoppingCart = new ShoppingCart
            {
                Items = cartItems
            };

            return View(shoppingCart);
        }

        public IActionResult AddToCart(int productId, int quantity)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == productId);
            if (product == null)
            {
                return NotFound();
            }

            var session = _httpContextAccessor.HttpContext.Session;
            var cartItemsJson = session.GetString("CartItems");
            var cartItems = string.IsNullOrEmpty(cartItemsJson)
                ? new List<CartItem>()
                : JsonConvert.DeserializeObject<List<CartItem>>(cartItemsJson);

            if (quantity < 1)
            {
                quantity = 1;
            }

            cartItems.Add(new CartItem
            {
                ProductId = productId,
                ProductName = product.Name, 
                Quantity = quantity, 
                UnitPrice = (decimal)product.Price
            });

            session.SetString("CartItems", JsonConvert.SerializeObject(cartItems));

            return RedirectToAction("Index", "Products");
        }

        public IActionResult RemoveFromCart(int productId)
        {
            var session = _httpContextAccessor.HttpContext.Session;
            var cartItemsJson = session.GetString("CartItems");
            var cartItems = string.IsNullOrEmpty(cartItemsJson)
                ? new List<CartItem>()
                : JsonConvert.DeserializeObject<List<CartItem>>(cartItemsJson);

            var itemToRemove = cartItems.FirstOrDefault(item => item.ProductId == productId);
            if (itemToRemove != null)
            {
                cartItems.Remove(itemToRemove);
                session.SetString("CartItems", JsonConvert.SerializeObject(cartItems));
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Checkout()
        {
            var session = _httpContextAccessor.HttpContext.Session;
            var cartItemsJson = session.GetString("CartItems");
            var cartItems = string.IsNullOrEmpty(cartItemsJson)
                ? new List<CartItem>()
                : JsonConvert.DeserializeObject<List<CartItem>>(cartItemsJson);

            var totalPrice = cartItems.Sum(item => item.Quantity * item.UnitPrice);

            var orderItems = _context.OrderItems
                .Where(item => cartItems.Select(cartItem => cartItem.ProductId).Contains(item.ProductId))
                .ToList();

            var viewModel = new ShoppingCartViewModel
            {
                CartItems = cartItems,
                TotalPrice = totalPrice,
                OrderDate = DateTime.Now,
                OrderItems = orderItems  
            };

            return View(viewModel);
        }
    }
}
