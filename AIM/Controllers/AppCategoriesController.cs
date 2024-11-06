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
    public class AppCategoriesController : BaseController
    {
        private readonly AimDbContext _context;

        public AppCategoriesController(AimDbContext context):base(context)
        {
            _context = context;
        }

        // GET: AppCategories
        public async Task<IActionResult> Index()
        {
            var aimDbContext = _context.tbl_aim_appcategory.Include(a => a.CreatedBy).Include(a => a.Status).Include(a => a.UpdatedBy);
            return PartialView(await aimDbContext.ToListAsync());
        }

        // GET: AppCategories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.tbl_aim_appcategory == null)
            {
                return NotFound();
            }

            var appCategory = await _context.tbl_aim_appcategory
                .Include(a => a.CreatedBy)
                .Include(a => a.Status)
                .Include(a => a.UpdatedBy)
                .FirstOrDefaultAsync(m => m.CatCode == id);
            if (appCategory == null)
            {
                return NotFound();
            }

            return PartialView(appCategory);
        }

        // GET: AppCategories/Create
        public IActionResult Create()
        {
            ViewData["CatCreatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName");
            ViewData["CatStatus"] = new SelectList(_context.tbl_aim_status, "StatusCode", "StatusName");
            ViewData["CatUpdatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName");
            return PartialView();
        }

        // POST: AppCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CatCode,CatName,CatStatus,CatCreatedBy,CatCreatedDt,CatUpdatedBy,CatUpdatedDt")] AppCategory appCategory)
        {
            if (ModelState.IsValid)
            {
                // Fetch the current app_cat from tbl_aim_parameters
                var appCatParameter = await _context.tbl_aim_parameters.FirstOrDefaultAsync(p => p.ParmCode == "app_cat");
                if (appCatParameter != null && appCatParameter.ParmValue != null)
                {
                    // Assign the currentAppCat directly
                    var currentAppCat = appCatParameter.ParmValue;

                    // Increment the currentAppCat by 1 to get the new app_cat
                    var newAppCat = currentAppCat + 1;

                    // Update the app_cat in the incoming 'appCategory' object
                    appCategory.CatCode = newAppCat;

                    // Update the 'parm_value' in tbl_aim_parameters
                    appCatParameter.ParmValue = newAppCat;

                    appCategory.CatCreatedBy = HttpContext.Session.GetString("UserName");

                    // Save changes to the context
                    await _context.SaveChangesAsync();

                    // Continue with adding the 'appCategory' object to the context
                    _context.Add(appCategory);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }

            ViewData["CatCreatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserCode", appCategory.CatCreatedBy);
            ViewData["CatStatus"] = new SelectList(_context.tbl_aim_status, "StatusCode", "StatusCode", appCategory.CatStatus);
            ViewData["CatUpdatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserCode", appCategory.CatUpdatedBy);
            return PartialView(appCategory);
        }


        // GET: AppCategories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.tbl_aim_appcategory == null)
            {
                return NotFound();
            }

            var appCategory = await _context.tbl_aim_appcategory.FindAsync(id);
            if (appCategory == null)
            {
                return NotFound();
            }
            ViewData["CatCreatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName", appCategory.CatCreatedBy);
            ViewData["CatStatus"] = new SelectList(_context.tbl_aim_status, "StatusCode", "StatusName", appCategory.CatStatus);
            ViewData["CatUpdatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName", appCategory.CatUpdatedBy);
            return PartialView(appCategory);
        }

        // POST: AppCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CatCode,CatName,CatStatus,CatCreatedBy,CatCreatedDt,CatUpdatedBy,CatUpdatedDt")] AppCategory appCategory)
        {
            if (id != appCategory.CatCode)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    appCategory.CatUpdatedBy = HttpContext.Session.GetString("UserName");
                    appCategory.CatUpdatedDt = DateTime.Now;
                    _context.Update(appCategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppCategoryExists(appCategory.CatCode))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CatCreatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName", appCategory.CatCreatedBy);
            ViewData["CatStatus"] = new SelectList(_context.tbl_aim_status, "StatusCode", "StatusName", appCategory.CatStatus);
            ViewData["CatUpdatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName", appCategory.CatUpdatedBy);
            return PartialView(appCategory);
        }

        // GET: AppCategories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.tbl_aim_appcategory == null)
            {
                return NotFound();
            }

            var appCategory = await _context.tbl_aim_appcategory
                .Include(a => a.CreatedBy)
                .Include(a => a.Status)
                .Include(a => a.UpdatedBy)
                .FirstOrDefaultAsync(m => m.CatCode == id);
            if (appCategory == null)
            {
                return NotFound();
            }

            return PartialView(appCategory);
        }

        // POST: AppCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.tbl_aim_appcategory == null)
            {
                return Problem("Entity set 'AimDbContext.tbl_aim_appcategory'  is null.");
            }
            var appCategory = await _context.tbl_aim_appcategory.FindAsync(id);
            if (appCategory != null)
            {
                _context.tbl_aim_appcategory.Remove(appCategory);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AppCategoryExists(int id)
        {
          return (_context.tbl_aim_appcategory?.Any(e => e.CatCode == id)).GetValueOrDefault();
        }
    }
}
