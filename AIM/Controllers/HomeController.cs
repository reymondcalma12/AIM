using AIM.Data;
using AIM.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace AIM.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;

        private readonly AimDbContext _context;

        public HomeController(AimDbContext context) : base(context)
        {
            _context = context;
        }


        public IActionResult Index()
        {
            var userCode = HttpContext.Session.GetString("UserName");
            var fullname = HttpContext.Session.GetString("FullName");

            if (string.IsNullOrEmpty(userCode) || string.IsNullOrEmpty(fullname))
            {
                return RedirectToAction("Login", "Users");
            }

            // Continue with the rest of your action logic for the "Index" view
            return View();
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}