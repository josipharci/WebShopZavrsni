using Microsoft.AspNetCore.Mvc;

namespace ZavrsniRadWebShop.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
