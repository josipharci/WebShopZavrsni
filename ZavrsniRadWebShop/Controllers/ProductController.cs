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
            var products = _context.Products.AsQueryable();

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


        public IActionResult Create()
        {
            ViewBag.Categories = _context.Categories.ToList();
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(Product product, IFormFile imageFile)
        {
            if (imageFile != null && imageFile.Length > 0)
            {

                var uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;

                var filePath = Path.Combine(_hostingEnvironment.WebRootPath, "img", uniqueFileName);


                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }


                product.ImagePath = "../img/" + uniqueFileName;
            }

            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        public class ProductsViewModel
        {
            public List<Product> Products { get; set; }
            public int PageNumber { get; set; }
            public int TotalPages { get; set; }
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            ViewBag.Categories = await _context.Categories.ToListAsync();

            return View(product);
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product, IFormFile imageFile)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

         
            if (imageFile != null && imageFile.Length > 0)
            {
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                var filePath = Path.Combine(_hostingEnvironment.WebRootPath, "img", uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                product.ImagePath = uniqueFileName;
            }

           
            try
            {
                _context.Update(product);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(product.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

           
            return RedirectToAction(nameof(Index));
        }
    }

}

