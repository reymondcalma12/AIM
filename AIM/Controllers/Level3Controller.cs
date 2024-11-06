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
    public class Level3Controller : BaseController
    {
        private readonly AimDbContext _context;

        public Level3Controller(AimDbContext context) : base(context)
        {
            _context = context;
        }

        // GET: Level3
        public async Task<IActionResult> Index()
        {
            var aimDbContext = _context.tbl_aim_level3.Include(l => l.CreatedBy).Include(l => l.Status).Include(l => l.UpdatedBy);
            return PartialView(await aimDbContext.ToListAsync());
        }

        // GET: Level3/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.tbl_aim_level3 == null)
            {
                return NotFound();
            }

            var level3 = await _context.tbl_aim_level3
                .Include(l => l.CreatedBy)
                .Include(l => l.Status)
                .Include(l => l.UpdatedBy)
                .FirstOrDefaultAsync(m => m.Level3Code == id);
            if (level3 == null)
            {
                return NotFound();
            }

            return PartialView(level3);
        }

        // GET: Level3/Create
        public IActionResult Create()
        {
            ViewData["Level3CreatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName");
            ViewData["Level3Status"] = new SelectList(_context.tbl_aim_status, "StatusCode", "StatusName");
            ViewData["Level3UpdatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName");
            return PartialView();
        }

        // POST: Level3/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Level3Code,Level3Name,Level3Status,Level3CreatedBy,Level3CreatedDt,Level3UpdatedBy,Level3UpdatedDt")] Level3 level3)
        {
            if (ModelState.IsValid)
            {
                // Fetch the current level3_code from tbl_aim_parameters
                var level3CodeParameter = await _context.tbl_aim_parameters.FirstOrDefaultAsync(p => p.ParmCode == "level3_code");
                if (level3CodeParameter != null && level3CodeParameter.ParmValue != null)
                {
                    // Assign the current level3_code directly
                    var currentLevel3Code = level3CodeParameter.ParmValue;

                    // Update the level3_code in the incoming 'level3' object
                    level3.Level3Code = currentLevel3Code+1;

                    // Increment the current level3_code by 1 for the next level3
                    level3CodeParameter.ParmValue = currentLevel3Code + 1;
                    level3.Level3CreatedBy = HttpContext.Session.GetString("UserName");
                    _context.Add(level3);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }

            ViewData["Level3CreatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserCode", level3.Level3CreatedBy);
            ViewData["Level3Status"] = new SelectList(_context.tbl_aim_status, "StatusCode", "StatusCode", level3.Level3Status);
            ViewData["Level3UpdatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserCode", level3.Level3UpdatedBy);
            return View(level3);
        }


        // GET: Level3/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.tbl_aim_level3 == null)
            {
                return NotFound();
            }

            var level3 = await _context.tbl_aim_level3.FindAsync(id);
            if (level3 == null)
            {
                return NotFound();
            }
            ViewData["Level3CreatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserCode", level3.Level3CreatedBy);
            ViewData["Level3Status"] = new SelectList(_context.tbl_aim_status, "StatusCode", "StatusCode", level3.Level3Status);
            ViewData["Level3UpdatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserCode", level3.Level3UpdatedBy);
            return PartialView(level3);
        }

        // POST: Level3/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Level3Code,Level3Name,Level3Status,Level3CreatedBy,Level3CreatedDt,Level3UpdatedBy,Level3UpdatedDt")] Level3 level3)
        {
            if (id != level3.Level3Code)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    level3.Level3UpdatedBy = HttpContext.Session.GetString("UserName");
                    level3.Level3UpdatedDt = DateTime.Now;
                    _context.Update(level3);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Level3Exists(level3.Level3Code))
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
            ViewData["Level3CreatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserCode", level3.Level3CreatedBy);
            ViewData["Level3Status"] = new SelectList(_context.tbl_aim_status, "StatusCode", "StatusCode", level3.Level3Status);
            ViewData["Level3UpdatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserCode", level3.Level3UpdatedBy);
            return View(level3);
        }

        // GET: Level3/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.tbl_aim_level3 == null)
            {
                return NotFound();
            }

            var level3 = await _context.tbl_aim_level3
                .Include(l => l.CreatedBy)
                .Include(l => l.Status)
                .Include(l => l.UpdatedBy)
                .FirstOrDefaultAsync(m => m.Level3Code == id);
            if (level3 == null)
            {
                return NotFound();
            }

            return PartialView(level3);
        }

        // POST: Level3/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.tbl_aim_level3 == null)
            {
                return Problem("Entity set 'AimDbContext.tbl_aim_level3'  is null.");
            }
            var level3 = await _context.tbl_aim_level3.FindAsync(id);
            if (level3 != null)
            {
                _context.tbl_aim_level3.Remove(level3);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool Level3Exists(int id)
        {
          return (_context.tbl_aim_level3?.Any(e => e.Level3Code == id)).GetValueOrDefault();
        }
    }
}
