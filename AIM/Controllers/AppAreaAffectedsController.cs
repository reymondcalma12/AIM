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
    public class AppAreaAffectedsController : BaseController
    {
        private readonly AimDbContext _context;

        public AppAreaAffectedsController(AimDbContext context):base(context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appAreaAffecteds = await _context.tbl_aim_app_areaaffected
                .Include(a => a.Application) // Include the related Application entity
                .Include(a => a.CreatedByUser) // Include the related User entity
                .Include(a => a.Area) // Include the related User entity
                .Where(m => m.AppCode == id)
                .ToListAsync();

            // Store the "id" as "AppCode" in the session
            HttpContext.Session.SetString("AppCode", id);

            return PartialView(appAreaAffecteds);
        }


        // GET: AppAreaAffecteds/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.tbl_aim_app_areaaffected == null)
            {
                return NotFound();
            }

            var appAreaAffected = await _context.tbl_aim_app_areaaffected
                .Include(a => a.Application)
                .Include(a => a.Area)
                .Include(a => a.CreatedByUser)
                .FirstOrDefaultAsync(m => m.AppCode == id);
            if (appAreaAffected == null)
            {
                return NotFound();
            }

            return PartialView(appAreaAffected);
        }

        // GET: AppAreaAffecteds/Create
        public IActionResult Create()
        {
            ViewData["AppCode"] = new SelectList(_context.tbl_aim_applications, "AppCode", "AppName", HttpContext.Session.GetString("AppCode"));
            ViewData["AreaCode"] = new SelectList(_context.tbl_aim_area, "AreaCode", "AreaName");
            ViewData["CreatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName");
            return PartialView();
        }

        // POST: AppAreaAffecteds/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AppCode,AreaCode,CreatedBy,CreatedDt")] AppAreaAffected appAreaAffected)
        {
            if (ModelState.IsValid)
            {
                appAreaAffected.CreatedBy = HttpContext.Session.GetString("UserName");
                _context.Add(appAreaAffected);
                await _context.SaveChangesAsync();
                // Redirect to the "Index" action of the "APPLICATIONS" controller with the "AppCode" from the session
                return RedirectToAction("Details", "APPLICATIONS", new { id = HttpContext.Session.GetString("AppCode") });
            }
            ViewData["AppCode"] = new SelectList(_context.tbl_aim_applications, "AppCode", "AppName", appAreaAffected.AppCode);
            ViewData["AreaCode"] = new SelectList(_context.tbl_aim_area, "AreaCode", "AreaCode", appAreaAffected.AreaCode);
            ViewData["CreatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName", appAreaAffected.CreatedBy);
            return PartialView(appAreaAffected);
        }

        

        

        // GET: AppAreaAffecteds/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.tbl_aim_app_areaaffected == null)
            {
                return NotFound();
            }

            var appAreaAffected = await _context.tbl_aim_app_areaaffected
                .Include(a => a.Application)
                .Include(a => a.Area)
                .Include(a => a.CreatedByUser)
                .FirstOrDefaultAsync(m => m.AppCode == id);
            if (appAreaAffected == null)
            {
                return NotFound();
            }

            return PartialView(appAreaAffected);
        }

        // POST: AppAreaAffecteds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.tbl_aim_app_areaaffected == null)
            {
                return Problem("Entity set 'AimDbContext.tbl_aim_app_areaaffected'  is null.");
            }
            var appAreaAffected = await _context.tbl_aim_app_areaaffected.FindAsync(id);
            if (appAreaAffected != null)
            {
                _context.tbl_aim_app_areaaffected.Remove(appAreaAffected);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AppAreaAffectedExists(string id)
        {
          return (_context.tbl_aim_app_areaaffected?.Any(e => e.AppCode == id)).GetValueOrDefault();
        }
    }
}
