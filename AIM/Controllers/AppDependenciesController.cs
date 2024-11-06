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
    public class AppDependenciesController : BaseController
    {
        private readonly AimDbContext _context;

        public AppDependenciesController(AimDbContext context) : base(context)
        {
            _context = context;
        }

        // GET: AppDependencies
        public async Task<IActionResult> Index()
        {
            var aimDbContext = _context.tbl_aim_dependencies.Include(a => a.Application).Include(a => a.CreatedBy).Include(a => a.RPortOsla);
            return PartialView(await aimDbContext.ToListAsync());
        }

        // GET: AppDependencies/Details/RPortOslaCode-AppCode
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Split the id parameter into parts
            var idParts = id.Split('-');
            if (idParts.Length != 2)
            {
                return NotFound();
            }

            string RPortOslaCode = idParts[0];
            string AppCode = idParts[1];

            var appDependency = await _context.tbl_aim_dependencies
                .Include(a => a.Application)
                .Include(a => a.CreatedBy)
                .Include(a => a.RPortOsla)
                .FirstOrDefaultAsync(m => m.RPortOslaCode == RPortOslaCode && m.AppCode == AppCode);

            if (appDependency == null)
            {
                return NotFound();
            }

            return PartialView(appDependency);
        }


        // GET: AppDependencies/Create
        public IActionResult Create()
        {
            ViewData["AppCode"] = new SelectList(_context.tbl_aim_applications, "AppCode", "AppName");
            ViewData["DependencyCreatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName");
            ViewData["RPortOslaCode"] = new SelectList(_context.tbl_aim_rportosla, "RPortOslaCode", "RPortOslaCode");
            return PartialView();
        }

        // POST: AppDependencies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RPortOslaCode,AppCode,DependencyCreatedBy,DependencyCreatedDt")] AppDependency appDependency)
        {
            appDependency.DependencyCreatedDt = DateTime.Now;
            if (ModelState.IsValid)
            {
                appDependency.DependencyCreatedBy = HttpContext.Session.GetString("UserName");
                _context.Add(appDependency);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AppCode"] = new SelectList(_context.tbl_aim_applications, "AppCode", "AppCode", appDependency.AppCode);
            ViewData["DependencyCreatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserCode", appDependency.DependencyCreatedBy);
            ViewData["RPortOslaCode"] = new SelectList(_context.tbl_aim_rportosla, "RPortOslaCode", "RPortOslaCode", appDependency.RPortOslaCode);
            return PartialView(appDependency);
        }

        // GET: AppDependencies/Edit/RPortOslaCode-AppCode
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Split the id parameter into parts
            var idParts = id.Split('-');
            if (idParts.Length != 2)
            {
                return NotFound();
            }

            string RPortOslaCode = idParts[0];
            string AppCode = idParts[1];

            var appDependency = await _context.tbl_aim_dependencies.FindAsync(RPortOslaCode, AppCode);
            if (appDependency == null)
            {
                return NotFound();
            }

            ViewData["AppCode"] = new SelectList(_context.tbl_aim_applications, "AppCode", "AppCode", appDependency.AppCode);
            ViewData["DependencyCreatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserCode", appDependency.DependencyCreatedBy);
            ViewData["RPortOslaCode"] = new SelectList(_context.tbl_aim_rportosla, "RPortOslaCode", "RPortOslaCode", appDependency.RPortOslaCode);

            return PartialView(appDependency);
        }


       

        // GET: AppSystemsAffecteds/Delete/RPortOslaCode-AppCode
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Split the id parameter into parts
            var idParts = id.Split('-');
            if (idParts.Length != 2)
            {
                return NotFound();
            }

            string RPortOslaCode = idParts[0];
            string AppCode = idParts[1];

            var appSystemsAffected = await _context.tbl_aim_dependencies
                .Include(a => a.Application)
                .Include(a => a.CreatedBy)
                .Include(a => a.RPortOsla)
                .FirstOrDefaultAsync(m => m.RPortOslaCode == RPortOslaCode && m.AppCode == AppCode);

            if (appSystemsAffected == null)
            {
                return NotFound();
            }

            return PartialView(appSystemsAffected);
        }

        // POST: AppSystemsAffecteds/Delete/RPortOslaCode-AppCode
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            // Split the id parameter into parts
            var idParts = id.Split('-');
            if (idParts.Length != 2)
            {
                return NotFound();
            }

            string RPortOslaCode = idParts[0];
            string AppCode = idParts[1];

        

            if (_context.tbl_aim_dependencies == null)
            {
                return Problem("Entity set 'AimDbContext.tbl_aim_appsystemsaffected' is null.");
            }

            var appDependency = await _context.tbl_aim_dependencies.FindAsync(RPortOslaCode, AppCode);
            if (appDependency != null)
            {
                _context.tbl_aim_dependencies.Remove(appDependency);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        private bool AppDependencyExists(string id)
        {
          return (_context.tbl_aim_dependencies?.Any(e => e.RPortOslaCode == id)).GetValueOrDefault();
        }
    }
}
