using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ZavrsniRadWebShop.Models;

namespace ZavrsniRadWebShop.Controllers
{
    public class ProductController : Controller
    {
        public static List<Product> product = new List<Product>{
              new Product { Name="Samesung" , Description="Najbolji mobitel na svijetu" , Price=15},
              new Product { Name="Samesung sm" , Description="Najbolji mobitel na svijetu" , Price=16}
        };
  

        public IActionResult Index()
        {
            return View(product);
        }

        public IActionResult Create()
        {
            return View();
        }

        // POST: AdminProduct/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("Index"); 
            }

            return View(product); 
        }
    }
}
