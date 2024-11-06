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
    public class ModulesController : BaseController
    {
        private readonly AimDbContext _context;

        public ModulesController(AimDbContext context):base(context)
        {
            _context = context;
        }

        // GET: Modules
        public async Task<IActionResult> Index()
        {
            var aimDbContext = _context.tbl_aim_modules.Include(m => m.Category).Include(m => m.CreatedBy).Include(m => m.Status).Include(m => m.UpdatedBy);
            return PartialView("Index",await aimDbContext.ToListAsync());
        }

        // GET: Modules/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.tbl_aim_modules == null)
            {
                return NotFound();
            }

            var @module = await _context.tbl_aim_modules
                .Include(m => m.Category)
                .Include(m => m.CreatedBy)
                .Include(m => m.Status)
                .Include(m => m.UpdatedBy)
                .FirstOrDefaultAsync(m => m.ModuleId == id);
            if (@module == null)
            {
                return NotFound();
            }

            return View(@module);
        }

        // GET: Modules/Create
        public IActionResult Create()
        {
            ViewData["ModuleCategory"] = new SelectList(_context.tbl_aim_category, "CategoryId", "CategoryName");
            ViewData["ModuleCreated"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName");
            ViewData["ModuleStatus"] = new SelectList(_context.tbl_aim_status, "StatusCode", "StatusName");
            ViewData["ModuleUpdated"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullname");
            return View();
        }

        // POST: Modules/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ModuleId,ModuleTitle,ModuleCategory,ModuleStatus,ModuleCreated,ModuleDtCreated,ModuleUpdated,ModuleDtUpdated")] Module @module)
        {
            if (ModelState.IsValid)
            {
                // Fetch the current module_created value from tbl_aim_parameters
                var moduleCreatedParameter = await _context.tbl_aim_parameters.FirstOrDefaultAsync(p => p.ParmCode == "module_no");
                if (moduleCreatedParameter != null && moduleCreatedParameter.ParmValue != null)
                {
                    // Assign the currentModuleCreated directly
                    var currentModuleCreated = moduleCreatedParameter.ParmValue;

                    // Update the module_created in the incoming 'module' object
                    @module.ModuleId = currentModuleCreated + 1;

                    // Increment the currentModuleCreated by 1 for the next module
                    moduleCreatedParameter.ParmValue = currentModuleCreated + 1;
            @module.ModuleCreated = HttpContext.Session.GetString("UserName");

                    @module.ModuleDtCreated = DateTime.Now;
                    @module.ModuleTitle = module.ModuleTitle.ToUpper();
                    _context.Add(@module);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }

            ViewData["ModuleCategory"] = new SelectList(_context.tbl_aim_category, "CategoryId", "CategoryName", @module.ModuleCategory);
            ViewData["ModuleCreated"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserCode", @module.ModuleCreated);
            ViewData["ModuleStatus"] = new SelectList(_context.tbl_aim_status, "StatusCode", "StatusCode", @module.ModuleStatus);
            ViewData["ModuleUpdated"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserCode", @module.ModuleUpdated);
            return View(@module);
        }


        // GET: Modules/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.tbl_aim_modules == null)
            {
                return NotFound();
            }

            var @module = await _context.tbl_aim_modules.FindAsync(id);
            if (@module == null)
            {
                return NotFound();
            }
            ViewData["ModuleCategory"] = new SelectList(_context.tbl_aim_category, "CategoryId", "CategoryName", @module.ModuleCategory);
            ViewData["ModuleCreated"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserCode", @module.ModuleCreated);
            ViewData["ModuleStatus"] = new SelectList(_context.tbl_aim_status, "StatusCode", "StatusName", @module.ModuleStatus);
            ViewData["ModuleUpdated"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserCode", @module.ModuleUpdated);
            return View(@module);
        }

        // POST: Modules/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ModuleId,ModuleTitle,ModuleCategory,ModuleStatus,ModuleCreated,ModuleDtCreated,ModuleUpdated,ModuleDtUpdated")] Module updatedModule)
        {
            if (id != updatedModule.ModuleId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Retrieve the existing entity with the same key
                    var existingModule = await _context.tbl_aim_modules.FindAsync(id);

                    if (existingModule == null)
                    {
                        return NotFound();
                    }

                    // Update the properties of the existing entity
                    existingModule.ModuleTitle = updatedModule.ModuleTitle;
                    existingModule.ModuleCategory = updatedModule.ModuleCategory;
                    existingModule.ModuleStatus = updatedModule.ModuleStatus;
                    existingModule.ModuleCreated = HttpContext.Session.GetString("UserName");
                    existingModule.ModuleDtCreated = updatedModule.ModuleDtCreated;
                    existingModule.ModuleUpdated = updatedModule.ModuleUpdated;
                    existingModule.ModuleDtUpdated = DateTime.Now;

                    _context.Update(existingModule);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ModuleExists(updatedModule.ModuleId))
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

            // Repopulate the ViewData for dropdowns (if needed)
            ViewData["ModuleCategory"] = new SelectList(_context.tbl_aim_category, "CategoryId", "CategoryName", updatedModule.ModuleCategory);
            ViewData["ModuleCreated"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserCode", updatedModule.ModuleCreated);
            ViewData["ModuleStatus"] = new SelectList(_context.tbl_aim_status, "StatusCode", "StatusCode", updatedModule.ModuleStatus);
            ViewData["ModuleUpdated"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserCode", updatedModule.ModuleUpdated);
            return View(updatedModule);
        }


        // GET: Modules/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.tbl_aim_modules == null)
            {
                return NotFound();
            }

            var @module = await _context.tbl_aim_modules
                .Include(m=> m.Category)
                .Include(m => m.CreatedBy)
                .Include(m => m.Status)
                .Include(m => m.UpdatedBy)
                .FirstOrDefaultAsync(m => m.ModuleId == id);
            if (@module == null)
            {
                return NotFound();
            }

            return View(@module);
        }

        // POST: Modules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.tbl_aim_modules == null)
            {
                return Problem("Entity set 'AimDbContext.tbl_aim_modules'  is null.");
            }
            var @module = await _context.tbl_aim_modules.FindAsync(id);
            if (@module != null)
            {
                _context.tbl_aim_modules.Remove(@module);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ModuleExists(int id)
        {
          return (_context.tbl_aim_modules?.Any(e => e.ModuleId == id)).GetValueOrDefault();
        }
    }
}
