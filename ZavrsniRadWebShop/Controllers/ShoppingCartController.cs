using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
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
                ? new List<OrderItem>()
                : JsonConvert.DeserializeObject<List<OrderItem>>(cartItemsJson);
            return View(cartItems);
        }

        public IActionResult AddToCart(int productId)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == productId);
            if (product == null)
            {
                return NotFound();
            }

            var session = _httpContextAccessor.HttpContext.Session;
            var cartItemsJson = session.GetString("CartItems");
            var cartItems = string.IsNullOrEmpty(cartItemsJson)
                ? new List<OrderItem>()
                : JsonConvert.DeserializeObject<List<OrderItem>>(cartItemsJson);

            var existingItem = cartItems.FirstOrDefault(item => item.ProductId == productId);
            if (existingItem != null)
            {
                existingItem.Quantity++;
            }
            else
            {
             
                cartItems.Add(new OrderItem
                {
                    ProductId = productId,
                    Quantity = 1,
                    UnitPrice = (decimal)product.Price
            });
            }

            session.SetString("CartItems", JsonConvert.SerializeObject(cartItems));

            return RedirectToAction(nameof(Index));
        }
    }
}
