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
    public class AppSupportsController : BaseController
    {
        private readonly AimDbContext _context;

        public AppSupportsController(AimDbContext context):base(context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appSupports = await _context.tbl_aim_app_support
                .Include(a => a.Application)
                .Include(a => a.Support)
                .Include(a => a.SupportType)
                .Include(a => a.User)
                .Where(m => m.AppCode == id)
                .ToListAsync();
            // Store the "id" as "AppCode" in the session
            HttpContext.Session.SetString("AppCode", id);


            return PartialView(appSupports);
        }

        // GET: AppSupports/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.tbl_aim_app_support == null)
            {
                return NotFound();
            }

            // Split the id into its parts
            var idParts = id.Split('-');

            if (idParts.Length != 3)
            {
                return NotFound();
            }

            string appCode = idParts[0];
            int supportCode;
            int supportTypeCode;

            if (!int.TryParse(idParts[1], out supportCode) || !int.TryParse(idParts[2], out supportTypeCode))
            {
                return NotFound();
            }

            var appSupport = await _context.tbl_aim_app_support
                .Include(a => a.Application)
                .Include(a => a.Support)
                .Include(a => a.SupportType)
                .Include(a => a.User)
                .FirstOrDefaultAsync(m => m.AppCode == appCode && m.SupportCode == supportCode && m.SupportTypeCode == supportTypeCode);

            if (appSupport == null)
            {
                return NotFound();
            }

            return PartialView(appSupport);
        }


        // GET: AppSupports/Create
        public IActionResult Create()
        {
            ViewData["AppCode"] = new SelectList(_context.tbl_aim_applications, "AppCode", "AppName", HttpContext.Session.GetString("AppCode"));
            ViewData["SupportCode"] = new SelectList(_context.tbl_aim_supports, "SupportCode", "SupportName");
            ViewData["SupportTypeCode"] = new SelectList(_context.tbl_aim_supporttype, "SupportTypeCode", "SupportTypeName");
            ViewData["CreatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName");
            return PartialView();
        }

        // POST: AppSupports/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AppCode,SupportCode,SupportTypeCode,CreatedBy,CreatedDt")] AppSupport appSupport)
        {
            if (ModelState.IsValid)
            {
                appSupport.CreatedBy = HttpContext.Session.GetString("UserName");
                appSupport.CreatedDt = DateTime.Now;
                _context.Add(appSupport);
                await _context.SaveChangesAsync();
                // Redirect to the "Index" action of the "APPLICATIONS" controller with the "AppCode" from the session
                return RedirectToAction("Details", "APPLICATIONS", new { id = HttpContext.Session.GetString("AppCode") });
            }
            ViewData["AppCode"] = new SelectList(_context.tbl_aim_applications, "AppCode", "AppName", appSupport.AppCode);
            ViewData["SupportCode"] = new SelectList(_context.tbl_aim_supports, "SupportCode", "SupportName", appSupport.SupportCode);
            ViewData["SupportTypeCode"] = new SelectList(_context.tbl_aim_supporttype, "SupportTypeCode", "SupportTypeName", appSupport.SupportTypeCode);
            ViewData["CreatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName", appSupport.CreatedBy);
            return PartialView(appSupport);
        }


        /// GET: AppSupports/Delete/5-10-20-createdBy
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.tbl_aim_app_support == null)
            {
                return NotFound();
            }

            // Split the id string to get individual values
            var idValues = id.Split('-');
            if (idValues.Length != 3)
            {
                return NotFound();
            }

            var appCode = idValues[0];
            var supportCode = int.Parse(idValues[1]);
            var supportTypeCode = int.Parse(idValues[2]);

            var appSupport = await _context.tbl_aim_app_support
                .Include(a => a.Application)
                .Include(a => a.Support)
                .Include(a => a.SupportType)
                .FirstOrDefaultAsync(m => m.AppCode == appCode &&
                                          m.SupportCode == supportCode &&
                                          m.SupportTypeCode == supportTypeCode);

            if (appSupport == null)
            {
                return NotFound();
            }

            return PartialView(appSupport);
        }

        // POST: AppSupports/Delete/5-10-20-createdBy
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.tbl_aim_app_support == null)
            {
                return Problem("Entity set 'AimDbContext.tbl_aim_app_support' is null.");
            }

            // Split the id string to get individual values
            var idValues = id.Split('-');
            if (idValues.Length != 3)
            {
                return NotFound();
            }

            var appCode = idValues[0];
            var supportCode = int.Parse(idValues[1]);
            var supportTypeCode = int.Parse(idValues[2]);

            var appSupport = await _context.tbl_aim_app_support.FindAsync(appCode, supportCode, supportTypeCode);
            if (appSupport != null)
            {
                _context.tbl_aim_app_support.Remove(appSupport);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }



        private bool AppSupportExists(string id)
        {
          return (_context.tbl_aim_app_support?.Any(e => e.AppCode == id)).GetValueOrDefault();
        }
    }
}
