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
    public class AppSystemAffectedsController : BaseController
    {
        private readonly AimDbContext _context;

        public AppSystemAffectedsController(AimDbContext context):base(context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appSystemAffecteds = await _context.tbl_aim_app_systemaffected
                .Include(a => a.Application) // Include the related Application entity
                .Include(a => a.System) // Include the related Application entity
                .Include(a => a.CreatedByUser) // Include the related User entity
                .Where(m => m.AppCode == id)
                .ToListAsync();
            // Store the "id" as "AppCode" in the session
            HttpContext.Session.SetString("AppCode", id);
            return PartialView(appSystemAffecteds);
        }

        // GET: AppSystemAffecteds/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.tbl_aim_app_systemaffected == null)
            {
                return NotFound();
            }

            var appSystemAffected = await _context.tbl_aim_app_systemaffected
                .Include(a => a.CreatedByUser)
                .Include(a => a.System)
                .FirstOrDefaultAsync(m => m.AppCode == id);
            if (appSystemAffected == null)
            {
                return NotFound();
            }

            return PartialView(appSystemAffected);
        }

        // GET: AppSystemAffecteds/Create
        public IActionResult Create()
        {
            ViewData["CreatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName");
            ViewData["AppCode"] = new SelectList(_context.tbl_aim_applications, "AppCode", "AppName", HttpContext.Session.GetString("AppCode"));
            ViewData["SystemCode"] = new SelectList(_context.tbl_aim_applications, "AppCode", "AppName");
            return PartialView();
        }

        // POST: AppSystemAffecteds/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AppCode,SystemCode,CreatedBy,CreatedDt")] AppSystemAffected appSystemAffected)
        {
            if (ModelState.IsValid)
            {
                appSystemAffected.CreatedBy = HttpContext.Session.GetString("UserName");
                _context.Add(appSystemAffected);
                await _context.SaveChangesAsync();
                // Redirect to the "Index" action of the "APPLICATIONS" controller with the "AppCode" from the session
                return RedirectToAction("Details", "APPLICATIONS", new { id = HttpContext.Session.GetString("AppCode") });
            }
            ViewData["CreatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName", appSystemAffected.CreatedBy);
            ViewData["AppCode"] = new SelectList(_context.tbl_aim_applications, "AppCode", "AppName", appSystemAffected.AppCode);
            return PartialView(appSystemAffected);
        }

        
        // GET: AppSystemAffecteds/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.tbl_aim_app_systemaffected == null)
            {
                return NotFound();
            }

            var appSystemAffected = await _context.tbl_aim_app_systemaffected
                .Include(a => a.CreatedByUser)
                .Include(a => a.System)
                .FirstOrDefaultAsync(m => m.AppCode == id);
            if (appSystemAffected == null)
            {
                return NotFound();
            }

            return PartialView(appSystemAffected);
        }

        // POST: AppSystemAffecteds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.tbl_aim_app_systemaffected == null)
            {
                return Problem("Entity set 'AimDbContext.tbl_aim_app_systemaffected'  is null.");
            }
            var appSystemAffected = await _context.tbl_aim_app_systemaffected.FindAsync(id);
            if (appSystemAffected != null)
            {
                _context.tbl_aim_app_systemaffected.Remove(appSystemAffected);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AppSystemAffectedExists(string id)
        {
          return (_context.tbl_aim_app_systemaffected?.Any(e => e.AppCode == id)).GetValueOrDefault();
        }
    }
}
