using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Linq;
using ZavrsniRadWebShop.Data;
using ZavrsniRadWebShop.Models;
using Microsoft.AspNetCore.Http;

namespace ZavrsniRadWebShop.Controllers
{
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public OrderController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Index()
        {
            var orders = _context.Orders.ToList();
            return View(orders);
        }

        public IActionResult CreateOrder(string customerName)
        {
            var session = _httpContextAccessor.HttpContext.Session;
            var cartItemsJson = session.GetString("CartItems");
            var cartItems = string.IsNullOrEmpty(cartItemsJson)
                ? new List<CartItem>()
                : JsonConvert.DeserializeObject<List<CartItem>>(cartItemsJson);

            if (!cartItems.Any())
            {
                return RedirectToAction("Index", "ShoppingCart");
            }

            decimal totalAmount = cartItems.Sum(item => item.Quantity * item.UnitPrice);

            var order = new Order
            {
                CustomerName = customerName,
                OrderDate = DateTime.Now,
                Status = "Pending",
                TotalAmount = totalAmount,
                OrderItems = new List<OrderItems>() 
            };

            _context.Orders.Add(order);
            _context.SaveChanges();

            foreach (var item in cartItems)
            {
                var orderItem = new OrderItems
                {
                    OrderId = order.Id,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    ProductName = item.ProductName 
                };

                _context.OrderItems.Add(orderItem);
                order.OrderItems.Add(orderItem); 
            }

            _context.SaveChanges(); 

            session.Remove("CartItems");

            return RedirectToAction("OrderConfirmation", "Order", new { orderId = order.Id });
        }

        public IActionResult OrderConfirmation(int orderId)
        {
            return View(orderId);
        }
    }
}
