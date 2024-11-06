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
    public class AppDeptProcOwnersController : BaseController
    {
        private readonly AimDbContext _context;

        public AppDeptProcOwnersController(AimDbContext context):base(context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appDeptProcOwners = await _context.tbl_aim_app_deptprocowner
                .Include(a => a.Application)
                .Include(a => a.Department)
                .Include(a => a.User)
                .Where(m => m.AppCode == id)
                .ToListAsync();
            // Store the "id" as "AppCode" in the session
            HttpContext.Session.SetString("AppCode", id);

            return PartialView(appDeptProcOwners);
        }


        // GET: AppDeptProcOwners/Details/5
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
            int deptCode;

            if (!int.TryParse(idComponents[1], out deptCode))
            {
                return NotFound();
            }

            var appDeptProcOwner = await _context.tbl_aim_app_deptprocowner
                .Include(a => a.Application)
                .Include(a => a.Department)
                .Include(a => a.User)
                .FirstOrDefaultAsync(m => m.AppCode == appCode && m.DeptCode == deptCode);

            if (appDeptProcOwner == null)
            {
                return NotFound();
            }

            return PartialView(appDeptProcOwner);
        }

        // GET: AppDeptProcOwners/Create
        public IActionResult Create()
        {
            ViewData["AppCode"] = new SelectList(_context.tbl_aim_applications, "AppCode", "AppName",HttpContext.Session.GetString("AppCode"));
            ViewData["DeptCode"] = new SelectList(_context.tbl_aim_department, "DeptCode", "DeptName");
            ViewData["CreatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName");
            return PartialView();
        }

        // POST: AppDeptProcOwners/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AppCode,DeptCode,CreatedBy,CreatedDt")] AppDeptProcOwner appDeptProcOwner)
        {
            if (ModelState.IsValid)
            {
                appDeptProcOwner.CreatedBy = HttpContext.Session.GetString("UserName");
                _context.Add(appDeptProcOwner);
                await _context.SaveChangesAsync();
                // Redirect to the "Index" action of the "APPLICATIONS" controller with the "AppCode" from the session
                return RedirectToAction("Details", "APPLICATIONS", new { id = HttpContext.Session.GetString("AppCode") });
            }
            ViewData["AppCode"] = new SelectList(_context.tbl_aim_applications, "AppCode", "AppName", appDeptProcOwner.AppCode);
            ViewData["DeptCode"] = new SelectList(_context.tbl_aim_department, "DeptCode", "DeptName", appDeptProcOwner.DeptCode);
            ViewData["CreatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName", appDeptProcOwner.CreatedBy);
            return PartialView(appDeptProcOwner);
        }



        // GET: AppDeptProcOwners/Delete/5
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
            int deptCode;

            if (!int.TryParse(idComponents[1], out deptCode))
            {
                return NotFound();
            }

            var appDeptProcOwner = await _context.tbl_aim_app_deptprocowner
                .Include(a => a.Application)
                .Include(a => a.Department)
                .Include(a => a.User)
                .FirstOrDefaultAsync(m => m.AppCode == appCode && m.DeptCode == deptCode);

            if (appDeptProcOwner == null)
            {
                return NotFound();
            }

            return PartialView(appDeptProcOwner);
        }

        // POST: AppDeptProcOwners/Delete/5
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
            int deptCode;

            if (!int.TryParse(idComponents[1], out deptCode))
            {
                return NotFound();
            }

            var appDeptProcOwner = await _context.tbl_aim_app_deptprocowner.FindAsync(appCode, deptCode);
            if (appDeptProcOwner != null)
            {
                _context.tbl_aim_app_deptprocowner.Remove(appDeptProcOwner);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }


        private bool AppDeptProcOwnerExists(string id)
        {
          return (_context.tbl_aim_app_deptprocowner?.Any(e => e.AppCode == id)).GetValueOrDefault();
        }
    }
}
