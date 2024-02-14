using Microsoft.AspNetCore.Mvc;
using ZavrsniRadWebShop.Data;

namespace ZavrsniRadWebShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderItemsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public OrderItemsController(ApplicationDbContext context)
        {
            _context = context;
        }

        
    }
}
