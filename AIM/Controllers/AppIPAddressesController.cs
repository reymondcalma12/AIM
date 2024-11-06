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
    public class AppIPAddressesController : BaseController
    {
        private readonly AimDbContext _context;

        public AppIPAddressesController(AimDbContext context):base(context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appIPAddresses = await _context.tbl_aim_app_ipaddress
                .Include(a => a.Application)
                .Include(a => a.User)
                .Where(m => m.AppCode == id)
                .ToListAsync();

            // Store the "id" as "AppCode" in the session
            HttpContext.Session.SetString("AppCode", id);

            return PartialView(appIPAddresses);
        }


        // GET: AppIPAddresses/Details/5
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
            string ipAddress = idComponents[1];

            var appIPAddress = await _context.tbl_aim_app_ipaddress
                .Include(a => a.Application)
                .Include(a => a.User)
                .FirstOrDefaultAsync(m => m.AppCode == appCode && m.IPAddress == ipAddress);

            if (appIPAddress == null)
            {
                return NotFound();
            }

            return PartialView(appIPAddress);
        }


        // GET: AppIPAddresses/Create
        public IActionResult Create()
        {
            ViewData["AppCode"] = new SelectList(_context.tbl_aim_applications, "AppCode", "AppName", HttpContext.Session.GetString("AppCode"));
            ViewData["CreatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName");
            return PartialView();
        }

        // POST: AppIPAddresses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AppCode,IPAddress,CreatedBy,CreatedDt")] AppIPAddress appIPAddress)
        {
            if (ModelState.IsValid)
            {
                appIPAddress.CreatedBy = HttpContext.Session.GetString("UserName");
                appIPAddress.CreatedDt = DateTime.Now;
                _context.Add(appIPAddress);
                await _context.SaveChangesAsync();
                // Redirect to the "Index" action of the "APPLICATIONS" controller with the "AppCode" from the session
                return RedirectToAction("Details", "APPLICATIONS", new { id = HttpContext.Session.GetString("AppCode") });
            }
            ViewData["AppCode"] = new SelectList(_context.tbl_aim_applications, "AppCode", "AppName", appIPAddress.AppCode);
            ViewData["CreatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName", appIPAddress.CreatedBy);
            return PartialView(appIPAddress);
        }


        // GET: AppIPAddresses/Delete/5
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
            string ipAddress = idComponents[1];

            var appIPAddress = await _context.tbl_aim_app_ipaddress
                .Include(a => a.Application)
                .Include(a => a.User)
                .FirstOrDefaultAsync(m => m.AppCode == appCode && m.IPAddress == ipAddress);

            if (appIPAddress == null)
            {
                return NotFound();
            }

            return PartialView(appIPAddress);
        }

        // POST: AppIPAddresses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
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
            string ipAddress = idComponents[1];

            var appIPAddress = await _context.tbl_aim_app_ipaddress.FindAsync(appCode, ipAddress);
            if (appIPAddress != null)
            {
                _context.tbl_aim_app_ipaddress.Remove(appIPAddress);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool AppIPAddressExists(string id)
        {
          return (_context.tbl_aim_app_ipaddress?.Any(e => e.AppCode == id)).GetValueOrDefault();
        }
    }
}
