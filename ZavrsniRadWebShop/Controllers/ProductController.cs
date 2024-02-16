using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZavrsniRadWebShop.Data;
using ZavrsniRadWebShop.Models;
using Microsoft.AspNetCore.Hosting;

namespace ZavrsniRadWebShop.Controllers
{

    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public ProductsController(ApplicationDbContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index(int page = 1, string search = null)
        {
            int pageSize = 12;
            var products = _context.Products.Include(p => p.Category).AsQueryable(); 

            if (!string.IsNullOrEmpty(search))
            {
                products = products.Where(p => p.Name.Contains(search) || p.Description.Contains(search));
            }

            var totalProducts = products.Count();
            var totalPages = (int)Math.Ceiling((double)totalProducts / pageSize);
            var paginatedProducts = products.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var viewModel = new ProductsViewModel
            {
                Products = paginatedProducts,
                PageNumber = page,
                TotalPages = totalPages
            };

            return View(viewModel);
        }

        public class ProductsViewModel
        {
            public List<Product> Products { get; set; }
            public int PageNumber { get; set; }
            public int TotalPages { get; set; }
        }

    }

}

