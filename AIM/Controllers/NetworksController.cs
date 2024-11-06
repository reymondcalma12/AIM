using AIM.Data;
using Microsoft.AspNetCore.Mvc;

namespace AIM.Controllers
{
    public class NetworksController : BaseController
    {

        private readonly AimDbContext _context;

        public NetworksController(AimDbContext context) : base(context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
