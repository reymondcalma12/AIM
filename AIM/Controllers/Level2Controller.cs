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
    public class Level2Controller : BaseController
    {
        private readonly AimDbContext _context;

        public Level2Controller(AimDbContext context):base(context)
        {
            _context = context;
        }

        // GET: Level2
        public async Task<IActionResult> Index()
        {
            var aimDbContext = _context.tbl_aim_level2.Include(l => l.CreatedBy).Include(l => l.Status).Include(l => l.UpdatedBy);
            return PartialView(await aimDbContext.ToListAsync());
        }

        // GET: Level2/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.tbl_aim_level2 == null)
            {
                return NotFound();
            }

            var level2 = await _context.tbl_aim_level2
                .Include(l => l.CreatedBy)
                .Include(l => l.Status)
                .Include(l => l.UpdatedBy)
                .FirstOrDefaultAsync(m => m.Level2Code == id);
            if (level2 == null)
            {
                return NotFound();
            }

            return PartialView(level2);
        }

        // GET: Level2/Create
        public IActionResult Create()
        {
            ViewData["Level2CreatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName");
            ViewData["Level2Status"] = new SelectList(_context.tbl_aim_status, "StatusCode", "StatusName");
            ViewData["Level2UpdatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName");
            return PartialView();
        }

        // POST: Level2/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Level2Code,Level2Name,Level2Status,Level2CreatedBy,Level2CreatedDt,Level2UpdatedBy,Level2UpdatedDt")] Level2 level2)
        {
            level2.Level2CreatedDt = DateTime.Now;
            if (ModelState.IsValid)
            {
                // Fetch the current level2_code from tbl_aim_parameters
                var level2CodeParameter = await _context.tbl_aim_parameters.FirstOrDefaultAsync(p => p.ParmCode == "level2_no");
                if (level2CodeParameter != null)
                {
                    // Increment the 'parm_value' in tbl_aim_parameters
                    level2CodeParameter.ParmValue++;

                    // Assign the current level2_code directly
                    level2.Level2Code = level2CodeParameter.ParmValue;

                    level2.Level2CreatedBy = HttpContext.Session.GetString("UserName");

                    // Save changes to the context
                    await _context.SaveChangesAsync();

                    // Continue with adding the 'level2' object to the context
                    _context.Add(level2);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }

            ViewData["Level2CreatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName", level2.Level2CreatedBy);
            ViewData["Level2Status"] = new SelectList(_context.tbl_aim_status, "StatusCode", "StatusName", level2.Level2Status);
            ViewData["Level2UpdatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName", level2.Level2UpdatedBy);
            return PartialView(level2);
        }


        // GET: Level2/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.tbl_aim_level2 == null)
            {
                return NotFound();
            }

            var level2 = await _context.tbl_aim_level2.FindAsync(id);
            if (level2 == null)
            {
                return NotFound();
            }
            ViewData["Level2CreatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName", level2.Level2CreatedBy);
            ViewData["Level2Status"] = new SelectList(_context.tbl_aim_status, "StatusCode", "StatusName", level2.Level2Status);
            ViewData["Level2UpdatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName", level2.Level2UpdatedBy);
            return PartialView(level2);
        }

        // POST: Level2/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Level2Code,Level2Name,Level2Status,Level2CreatedBy,Level2CreatedDt,Level2UpdatedBy,Level2UpdatedDt")] Level2 level2)
        {
            if (id != level2.Level2Code)
            {
                return NotFound();
            }
            level2.Level2UpdatedDt = DateTime.Now;
            if (ModelState.IsValid)
            {
                try
                {
                    level2.Level2CreatedBy = HttpContext.Session.GetString("UserName");
                    _context.Update(level2);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Level2Exists(level2.Level2Code))
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
            ViewData["Level2CreatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName", level2.Level2CreatedBy);
            ViewData["Level2Status"] = new SelectList(_context.tbl_aim_status, "StatusCode", "StatusName", level2.Level2Status);
            ViewData["Level2UpdatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName", level2.Level2UpdatedBy);
            return PartialView(level2);
        }

        // GET: Level2/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.tbl_aim_level2 == null)
            {
                return NotFound();
            }

            var level2 = await _context.tbl_aim_level2
                .Include(l => l.CreatedBy)
                .Include(l => l.Status)
                .Include(l => l.UpdatedBy)
                .FirstOrDefaultAsync(m => m.Level2Code == id);
            if (level2 == null)
            {
                return NotFound();
            }

            return PartialView(level2);
        }

        // POST: Level2/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.tbl_aim_level2 == null)
            {
                return Problem("Entity set 'AimDbContext.tbl_aim_level2'  is null.");
            }
            var level2 = await _context.tbl_aim_level2.FindAsync(id);
            if (level2 != null)
            {
                _context.tbl_aim_level2.Remove(level2);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool Level2Exists(int id)
        {
          return (_context.tbl_aim_level2?.Any(e => e.Level2Code == id)).GetValueOrDefault();
        }
    }
}
