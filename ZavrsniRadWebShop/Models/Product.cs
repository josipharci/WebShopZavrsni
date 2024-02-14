using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ZavrsniRadWebShop.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }


        public string Description { get; set; }

        [Required]
        public float Price { get; set; }


        [Required]
        public int QuantityInStock { get; set; }


        public string ImagePath { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        
    }
}
