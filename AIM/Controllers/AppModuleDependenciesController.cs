using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AIM.Data;
using AIM.Models;

namespace AIM.Controllers
{
    public class AppModuleDependenciesController : BaseController
    {
        private readonly AimDbContext _context;

        public AppModuleDependenciesController(AimDbContext context):base(context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appModuleDependencies = await _context.tbl_aim_app_moduledependencies
                .Include(a => a.Application)
                .Include(a => a.DependencyApplication)
                .Include(a => a.User)
                .Where(m => m.AppCode == id)
                .ToListAsync();

            // Store the "id" as "AppCode" in the session
            HttpContext.Session.SetString("AppCode", id);
            return PartialView(appModuleDependencies);
        }


        // GET: AppModuleDependencies/Details/A-B
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || !_context.tbl_aim_app_moduledependencies.Any(a => a.AppCode == id))
            {
                return NotFound();
            }

            // Split the composite key into AppCode and ModuleCode
            var keyParts = id.Split('-');
            if (keyParts.Length != 2)
            {
                return BadRequest(); // Handle invalid composite key format
            }

            var appCode = keyParts[0];
            var moduleCode = keyParts[1];

            var appModuleDependency = await _context.tbl_aim_app_moduledependencies
                .Include(a => a.Application)
                .Include(a => a.DependencyApplication)
                .Include(a => a.User)
                .FirstOrDefaultAsync(m => m.AppCode == appCode && m.ModuleCode == moduleCode);

            if (appModuleDependency == null)
            {
                return NotFound();
            }

            return PartialView(appModuleDependency);
        }


        // GET: AppModuleDependencies/Create
        public IActionResult Create()
        {
            ViewData["AppCode"] = new SelectList(_context.tbl_aim_applications, "AppCode", "AppName", HttpContext.Session.GetString("AppCode"));
            ViewData["ModuleCode"] = new SelectList(_context.tbl_aim_applications, "AppCode", "AppName");
            ViewData["CreatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName");
            return PartialView();
        }

        // POST: AppModuleDependencies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AppCode,ModuleCode,CreatedBy,CreatedDt")] AppModuleDependency appModuleDependency)
        {
            if (ModelState.IsValid)
            {
                appModuleDependency.CreatedBy = HttpContext.Session.GetString("UserName");
                appModuleDependency.CreatedDt = DateTime.Now;
                _context.Add(appModuleDependency);
                await _context.SaveChangesAsync();
                // Redirect to the "Index" action of the "APPLICATIONS" controller with the "AppCode" from the session
                return RedirectToAction("Details", "APPLICATIONS", new { id = HttpContext.Session.GetString("AppCode") });
            }
            ViewData["AppCode"] = new SelectList(_context.tbl_aim_applications, "AppCode", "AppName", appModuleDependency.AppCode);
            ViewData["ModuleCode"] = new SelectList(_context.tbl_aim_applications, "AppCode", "AppName", appModuleDependency.ModuleCode);
            ViewData["CreatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName", appModuleDependency.CreatedBy);
            return PartialView(appModuleDependency);
        }



        // GET: AppModuleDependencies/Delete/A-B
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || !_context.tbl_aim_app_moduledependencies.Any(a => a.AppCode == id))
            {
                return NotFound();
            }

            // Split the composite key into AppCode and ModuleCode
            var keyParts = id.Split('-');
            if (keyParts.Length != 2)
            {
                return BadRequest(); // Handle invalid composite key format
            }

            var appCode = keyParts[0];
            var moduleCode = keyParts[1];

            var appModuleDependency = await _context.tbl_aim_app_moduledependencies
                .Include(a => a.Application)
                .Include(a => a.DependencyApplication)
                .Include(a => a.User)
                .FirstOrDefaultAsync(m => m.AppCode == appCode && m.ModuleCode == moduleCode);

            if (appModuleDependency == null)
            {
                return NotFound();
            }

            return PartialView(appModuleDependency);
        }

        // POST: AppModuleDependencies/Delete/A-B
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.tbl_aim_app_moduledependencies == null)
            {
                return Problem("Entity set 'AimDbContext.tbl_aim_app_moduledependencies'  is null.");
            }

            // Split the composite key into AppCode and ModuleCode
            var keyParts = id.Split('-');
            if (keyParts.Length != 2)
            {
                return BadRequest(); // Handle invalid composite key format
            }

            var appCode = keyParts[0];
            var moduleCode = keyParts[1];

            var appModuleDependency = await _context.tbl_aim_app_moduledependencies.FindAsync(appCode, moduleCode);
            if (appModuleDependency != null)
            {
                _context.tbl_aim_app_moduledependencies.Remove(appModuleDependency);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        private bool AppModuleDependencyExists(string id)
        {
          return (_context.tbl_aim_app_moduledependencies?.Any(e => e.AppCode == id)).GetValueOrDefault();
        }
    }
}
