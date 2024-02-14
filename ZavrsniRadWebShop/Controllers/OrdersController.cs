using Microsoft.AspNetCore.Mvc;
using ZavrsniRadWebShop.Data;

namespace ZavrsniRadWebShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

       
    }

}
