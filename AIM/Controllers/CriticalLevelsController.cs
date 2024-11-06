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
    public class CriticalLevelsController : BaseController
    {
        private readonly AimDbContext _context;

        public CriticalLevelsController(AimDbContext context):base(context)
        {
            _context = context;
        }

        // GET: CriticalLevels
        public async Task<IActionResult> Index()
        {
            var aimDbContext = _context.tbl_aim_criticallevel.Include(c => c.CreatedBy).Include(c => c.Status).Include(c => c.UpdatedBy);
            return PartialView(await aimDbContext.ToListAsync());
        }

        // GET: CriticalLevels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.tbl_aim_criticallevel == null)
            {
                return NotFound();
            }

            var criticalLevel = await _context.tbl_aim_criticallevel
                .Include(c => c.CreatedBy)
                .Include(c => c.Status)
                .Include(c => c.UpdatedBy)
                .FirstOrDefaultAsync(m => m.CriticalLevelCode == id);
            if (criticalLevel == null)
            {
                return NotFound();
            }

            return PartialView(criticalLevel);
        }

        // GET: CriticalLevels/Create
        public IActionResult Create()
        {
            ViewData["CriticalLevelCreatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName");
            ViewData["CriticalLevelStatus"] = new SelectList(_context.tbl_aim_status, "StatusCode", "StatusName");
            ViewData["CriticalLevelUpdatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName");
            return PartialView();
        }

        // POST: CriticalLevels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CriticalLevelCode,CriticalLevelName,CriticalLevelStatus,CriticalLevelCreatedBy,CriticalLevelCreatedDt,CriticalLevelUpdatedBy,CriticalLevelUpdatedDt")] CriticalLevel criticalLevel)
        {
            criticalLevel.CriticalLevelCreatedDt = DateTime.Now;

            if (ModelState.IsValid)
            {
                // Fetch the current critical_level_code from tbl_aim_parameters
                var criticalLevelCodeParameter = await _context.tbl_aim_parameters.FirstOrDefaultAsync(p => p.ParmCode == "critic_no");
                if (criticalLevelCodeParameter != null)
                {
                    // Assign the current critical_level_code directly
                    criticalLevel.CriticalLevelCode = criticalLevelCodeParameter.ParmValue;

                    // Increment the 'parm_value' in tbl_aim_parameters
                    int newCriticalLevelCode = criticalLevelCodeParameter.ParmValue + 1;
                    criticalLevelCodeParameter.ParmValue = newCriticalLevelCode;

                    criticalLevel.CriticalLevelCreatedBy = HttpContext.Session.GetString("UserName");

                    // Save changes to the context
                    await _context.SaveChangesAsync();

                    // Continue with adding the 'criticalLevel' object to the context
                    _context.Add(criticalLevel);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }

            ViewData["CriticalLevelCreatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName", criticalLevel.CriticalLevelCreatedBy);
            ViewData["CriticalLevelStatus"] = new SelectList(_context.tbl_aim_status, "StatusCode", "StatusName", criticalLevel.CriticalLevelStatus);
            ViewData["CriticalLevelUpdatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName", criticalLevel.CriticalLevelUpdatedBy);
            return PartialView(criticalLevel);
        }


        // GET: CriticalLevels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.tbl_aim_criticallevel == null)
            {
                return NotFound();
            }

            var criticalLevel = await _context.tbl_aim_criticallevel.FindAsync(id);
            if (criticalLevel == null)
            {
                return NotFound();
            }
            ViewData["CriticalLevelCreatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName", criticalLevel.CriticalLevelCreatedBy);
            ViewData["CriticalLevelStatus"] = new SelectList(_context.tbl_aim_status, "StatusCode", "StatusName", criticalLevel.CriticalLevelStatus);
            ViewData["CriticalLevelUpdatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName", criticalLevel.CriticalLevelUpdatedBy);
            return PartialView(criticalLevel);
        }

        // POST: CriticalLevels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CriticalLevelCode,CriticalLevelName,CriticalLevelStatus,CriticalLevelCreatedBy,CriticalLevelCreatedDt,CriticalLevelUpdatedBy,CriticalLevelUpdatedDt")] CriticalLevel criticalLevel)
        {
            if (id != criticalLevel.CriticalLevelCode)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    criticalLevel.CriticalLevelUpdatedBy = HttpContext.Session.GetString("UserName");
                    _context.Update(criticalLevel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CriticalLevelExists(criticalLevel.CriticalLevelCode))
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
            ViewData["CriticalLevelCreatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName", criticalLevel.CriticalLevelCreatedBy);
            ViewData["CriticalLevelStatus"] = new SelectList(_context.tbl_aim_status, "StatusCode", "StatusName", criticalLevel.CriticalLevelStatus);
            ViewData["CriticalLevelUpdatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName", criticalLevel.CriticalLevelUpdatedBy);
            return PartialView(criticalLevel);
        }

        // GET: CriticalLevels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.tbl_aim_criticallevel == null)
            {
                return NotFound();
            }

            var criticalLevel = await _context.tbl_aim_criticallevel
                .Include(c => c.CreatedBy)
                .Include(c => c.Status)
                .Include(c => c.UpdatedBy)
                .FirstOrDefaultAsync(m => m.CriticalLevelCode == id);
            if (criticalLevel == null)
            {
                return NotFound();
            }

            return PartialView(criticalLevel);
        }

        // POST: CriticalLevels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.tbl_aim_criticallevel == null)
            {
                return Problem("Entity set 'AimDbContext.tbl_aim_criticallevel'  is null.");
            }
            var criticalLevel = await _context.tbl_aim_criticallevel.FindAsync(id);
            if (criticalLevel != null)
            {
                _context.tbl_aim_criticallevel.Remove(criticalLevel);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CriticalLevelExists(int id)
        {
          return (_context.tbl_aim_criticallevel?.Any(e => e.CriticalLevelCode == id)).GetValueOrDefault();
        }
    }
}
