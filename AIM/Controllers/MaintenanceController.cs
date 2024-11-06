using AIM.Data;
using AIM.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AIM.Controllers
{
    public class MaintenanceController : BaseController
    {
        private readonly AimDbContext _context;

        public MaintenanceController(AimDbContext context) : base(context)
        {
            _context = context;
        }


        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> SearchResult(string? searchModules)
        {

            if (searchModules == null)
            {
                List<Module> modules = await _context.tbl_aim_modules.Include(s => s.Category).Where(a => a.Category.CategoryName == "MAINTENANCE").OrderBy(a => a.ModuleTitle).ToListAsync();

                return Json(modules);
            }
            else
            {
                
                var toLower = searchModules.ToLower();

                List<Module> modules = await _context.tbl_aim_modules.Include(s => s.Category).Where(a => a.ModuleTitle.ToLower().Contains(toLower)).Where(a => a.Category.CategoryName == "MAINTENANCE").OrderBy(a => a.ModuleTitle).ToListAsync();

                return Json(modules);
            }
        }

    }
}
