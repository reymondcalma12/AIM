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
    public class BrowserCompatibilitiesController : BaseController
    {
        private readonly AimDbContext _context;

        public BrowserCompatibilitiesController(AimDbContext context):base(context)
        {
            _context = context;
        }



        public async Task<IActionResult> Index(string id)
        {
           

            var browserCompatibilities = await _context.tbl_aim_app_browsercompatibility
                .Include(b => b.Application)
                .Include(b => b.Browser)
                .Include(b => b.CreatedUser)
                .Where(m => m.AppCode == id)
                .ToListAsync();

          
            // Store the "id" as "AppCode" in the session
            HttpContext.Session.SetString("AppCode", id);
            return PartialView(browserCompatibilities);
        }


        // GET: BrowserCompatibilities/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Split the id into appCode and browserCode
            var idParts = id.Split('-');
            if (idParts.Length != 2)
            {
                return NotFound();
            }

            string appCode = idParts[0];
            int browserCode;
            if (!int.TryParse(idParts[1], out browserCode))
            {
                return NotFound();
            }

            var browserCompatibility = await _context.tbl_aim_app_browsercompatibility
                .Include(b => b.Application)
                .Include(b => b.Browser)
                .Include(b => b.CreatedUser)
                .FirstOrDefaultAsync(m => m.AppCode == appCode && m.BrowserCode == browserCode);

            if (browserCompatibility == null)
            {
                return NotFound();
            }

            return PartialView(browserCompatibility);
        }

        


        // GET: BrowserCompatibilities/Create
        public IActionResult Create()
        {
            ViewData["AppCode"] = new SelectList(_context.tbl_aim_applications, "AppCode", "AppName", HttpContext.Session.GetString("AppCode"));
            ViewData["BrowserCode"] = new SelectList(_context.tbl_aim_browser, "BrowserCode", "BrowserName");
            ViewData["CreatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName");
            return PartialView();
        }

        // POST: BrowserCompatibilities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AppCode,BrowserCode,CreatedBy,CreatedDt")] BrowserCompatibility browserCompatibility)
        {
            if (ModelState.IsValid)
            {
                browserCompatibility.CreatedBy = HttpContext.Session.GetString("UserName");
                browserCompatibility.CreatedDt = DateTime.Now;
                _context.Add(browserCompatibility);
                await _context.SaveChangesAsync();
                // Redirect to the "Index" action of the "APPLICATIONS" controller with the "AppCode" from the session
                return RedirectToAction("Details", "APPLICATIONS", new { id = HttpContext.Session.GetString("AppCode") });
            }
            ViewData["AppCode"] = new SelectList(_context.tbl_aim_applications, "AppCode", "AppName", browserCompatibility.AppCode);
            ViewData["BrowserCode"] = new SelectList(_context.tbl_aim_browser, "BrowserCode", "BrowserName", browserCompatibility.BrowserCode);
            ViewData["CreatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName", browserCompatibility.CreatedBy);
            return PartialView(browserCompatibility);
        }


        // GET: BrowserCompatibilities/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Split the id into appCode and browserCode
            var idParts = id.Split('-');
            if (idParts.Length != 2)
            {
                return NotFound();
            }

            string appCode = idParts[0];
            int browserCode;
            if (!int.TryParse(idParts[1], out browserCode))
            {
                return NotFound();
            }

            var browserCompatibility = await _context.tbl_aim_app_browsercompatibility
                .Include(b => b.Application)
                .Include(b => b.Browser)
                .Include(b => b.CreatedUser)
                .FirstOrDefaultAsync(m => m.AppCode == appCode && m.BrowserCode == browserCode);

            if (browserCompatibility == null)
            {
                return NotFound();
            }

            return PartialView(browserCompatibility);
        }

        // POST: BrowserCompatibilities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Split the id into appCode and browserCode
            var idParts = id.Split('-');
            if (idParts.Length != 2)
            {
                return NotFound();
            }

            string appCode = idParts[0];
            int browserCode;
            if (!int.TryParse(idParts[1], out browserCode))
            {
                return NotFound();
            }

            var browserCompatibility = await _context.tbl_aim_app_browsercompatibility.FindAsync(appCode, browserCode);

            if (browserCompatibility != null)
            {
                _context.tbl_aim_app_browsercompatibility.Remove(browserCompatibility);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return NotFound();
        }


        private bool BrowserCompatibilityExists(string id)
        {
          return (_context.tbl_aim_app_browsercompatibility?.Any(e => e.AppCode == id)).GetValueOrDefault();
        }
    }
}
