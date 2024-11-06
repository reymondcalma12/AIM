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
    public class AppFunctionalAreaOwnersController : BaseController
    {
        private readonly AimDbContext _context;

        public AppFunctionalAreaOwnersController(AimDbContext context):base(context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appFunctionalAreaOwners = await _context.tbl_aim_app_functionalarea_owner
                .Include(a => a.Application)
                .Include(a => a.FunctionalArea)
                .Include(a => a.User)
                .Where(m => m.AppCode == id)
                .ToListAsync();
            // Store the "id" as "AppCode" in the session
            HttpContext.Session.SetString("AppCode", id);


            return PartialView(appFunctionalAreaOwners);
        }


        // GET: AppFunctionalAreaOwners/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Split the composite ID into its components
            var idComponents = id.Split('-');
            if (idComponents.Length != 2)
            {
                return NotFound();
            }

            string appCode = idComponents[0];
            int functionalCode;

            if (!int.TryParse(idComponents[1], out functionalCode))
            {
                return NotFound();
            }

            var appFunctionalAreaOwner = await _context.tbl_aim_app_functionalarea_owner
                .Include(a => a.Application)
                .Include(a => a.FunctionalArea)
                .Include(a => a.User)
                .FirstOrDefaultAsync(m => m.AppCode == appCode && m.FunctionalCode == functionalCode);

            if (appFunctionalAreaOwner == null)
            {
                return NotFound();
            }

            return PartialView(appFunctionalAreaOwner);
        }

        // GET: AppFunctionalAreaOwners/Create
        public IActionResult Create()
        {
            ViewData["AppCode"] = new SelectList(_context.tbl_aim_applications, "AppCode", "AppName", HttpContext.Session.GetString("AppCode"));
            ViewData["FunctionalCode"] = new SelectList(_context.tbl_aim_functionalarea, "FunctionalCode", "FunctionalName");
            ViewData["CreatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName");
            return PartialView();
        }

        // POST: AppFunctionalAreaOwners/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AppCode,FunctionalCode,CreatedBy,CreatedDt")] AppFunctionalAreaOwner appFunctionalAreaOwner)
        {
            if (ModelState.IsValid)
            {
                appFunctionalAreaOwner.CreatedDt = DateTime.Now;
                appFunctionalAreaOwner.CreatedBy = HttpContext.Session.GetString("UserName");
                _context.Add(appFunctionalAreaOwner);
                await _context.SaveChangesAsync();
                // Redirect to the "Index" action of the "APPLICATIONS" controller with the "AppCode" from the session
                return RedirectToAction("Details", "APPLICATIONS", new { id = HttpContext.Session.GetString("AppCode") });
            }
            ViewData["AppCode"] = new SelectList(_context.tbl_aim_applications, "AppCode", "AppName", appFunctionalAreaOwner.AppCode);
            ViewData["FunctionalCode"] = new SelectList(_context.tbl_aim_functionalarea, "FunctionalCode", "FunctionalName", appFunctionalAreaOwner.FunctionalCode);
            ViewData["CreatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName", appFunctionalAreaOwner.CreatedBy);
            return PartialView(appFunctionalAreaOwner);
        }



        // GET: AppFunctionalAreaOwners/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Split the composite ID into its components
            var idComponents = id.Split('-');
            if (idComponents.Length != 2)
            {
                return NotFound();
            }

            string appCode = idComponents[0];
            int functionalCode;

            if (!int.TryParse(idComponents[1], out functionalCode))
            {
                return NotFound();
            }

            var appFunctionalAreaOwner = await _context.tbl_aim_app_functionalarea_owner
                .Include(a => a.Application)
                .Include(a => a.FunctionalArea)
                .Include(a => a.User)
                .FirstOrDefaultAsync(m => m.AppCode == appCode && m.FunctionalCode == functionalCode);

            if (appFunctionalAreaOwner == null)
            {
                return NotFound();
            }

            return PartialView(appFunctionalAreaOwner);
        }

        // POST: AppFunctionalAreaOwners/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.tbl_aim_app_functionalarea_owner == null)
            {
                return Problem("Entity set 'AimDbContext.tbl_aim_app_functionalarea_owner' is null.");
            }

            // Split the composite ID into its components
            var idComponents = id.Split('-');
            if (idComponents.Length != 2)
            {
                return NotFound();
            }

            string appCode = idComponents[0];
            int functionalCode;

            if (!int.TryParse(idComponents[1], out functionalCode))
            {
                return NotFound();
            }

            var appFunctionalAreaOwner = await _context.tbl_aim_app_functionalarea_owner.FindAsync(appCode, functionalCode);
            if (appFunctionalAreaOwner != null)
            {
                _context.tbl_aim_app_functionalarea_owner.Remove(appFunctionalAreaOwner);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool AppFunctionalAreaOwnerExists(string id)
        {
          return (_context.tbl_aim_app_functionalarea_owner?.Any(e => e.AppCode == id)).GetValueOrDefault();
        }
    }
}
