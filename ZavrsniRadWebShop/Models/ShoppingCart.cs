﻿using System.Collections.Generic;

namespace ZavrsniRadWebShop.Models
{
    public class ShoppingCartViewModel
    {
        public List<CartItem> CartItems { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime OrderDate { get; set; }
        public List<OrderItems> OrderItems { get; set; }
    }

    public class ShoppingCart
    {
        public List<CartItem> Items { get; set; }
        public decimal TotalPrice { get; set; }
    }

    public class CartItem
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Subtotal => Quantity * UnitPrice;
    }
}