using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ZavrsniRadWebShop.Models
{
    public class Order
    {
        public int Id { get; set; }

        [Required]
        public string CustomerName { get; set; }

        public DateTime OrderDate { get; set; }

        public string Status { get; set; }

        [Required]
        public decimal TotalAmount { get; set; }

        public List<OrderItems> OrderItems { get; set; } 
    }

    public class OrderItems
    {
        public int Id { get; set; }

        [Required]
        public int OrderId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public decimal UnitPrice { get; set; }

       
        public string ProductName { get; set; }

        public Product Product { get; set; }
    }


}
