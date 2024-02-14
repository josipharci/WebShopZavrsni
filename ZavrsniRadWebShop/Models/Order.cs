using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ZavrsniRadWebShop.Models
{
    public class Order
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public DateTime OrderDate { get; set; }

        public string Status { get; set; }

        [Required]
        public decimal TotalPrice { get; set; }

    }
}
