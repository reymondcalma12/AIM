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
    public class SLALevelsController : BaseController
    {
        private readonly AimDbContext _context;

        public SLALevelsController(AimDbContext context) : base(context)
        {
            _context = context;
        }

        // GET: SLALevels
        public async Task<IActionResult> Index()
        {
            var aimDbContext = _context.tbl_aim_slalevel.Include(s => s.CreatedBy).Include(s => s.Status).Include(s => s.UpdatedBy);
            return PartialView(await aimDbContext.ToListAsync());
        }

        // GET: SLALevels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.tbl_aim_slalevel == null)
            {
                return NotFound();
            }

            var sLALevel = await _context.tbl_aim_slalevel
                .Include(s => s.CreatedBy)
                .Include(s => s.Status)
                .Include(s => s.UpdatedBy)
                .FirstOrDefaultAsync(m => m.SLALevelCode == id);
            if (sLALevel == null)
            {
                return NotFound();
            }

            return PartialView(sLALevel);
        }

        // GET: SLALevels/Create
        public IActionResult Create()
        {
            ViewData["SLALevelCreatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName");
            ViewData["SLALevelStatus"] = new SelectList(_context.tbl_aim_status, "StatusCode", "StatusName");
            ViewData["SLALevelUpdatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName");
            return PartialView();
        }

        // POST: SLALevels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SLALevelCode,SLALevelName,SLALevelStatus,SLALevelCreatedBy,SLALevelCreatedDt,SLALevelUpdatedBy,SLALevelUpdatedDt")] SLALevel sLALevel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sLALevel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SLALevelCreatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserCode", sLALevel.SLALevelCreatedBy);
            ViewData["SLALevelStatus"] = new SelectList(_context.tbl_aim_status, "StatusCode", "StatusCode", sLALevel.SLALevelStatus);
            ViewData["SLALevelUpdatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserCode", sLALevel.SLALevelUpdatedBy);
            return View(sLALevel);
        }

        // GET: SLALevels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.tbl_aim_slalevel == null)
            {
                return NotFound();
            }

            var sLALevel = await _context.tbl_aim_slalevel.FindAsync(id);
            if (sLALevel == null)
            {
                return NotFound();
            }
            ViewData["SLALevelCreatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserCode", sLALevel.SLALevelCreatedBy);
            ViewData["SLALevelStatus"] = new SelectList(_context.tbl_aim_status, "StatusCode", "StatusCode", sLALevel.SLALevelStatus);
            ViewData["SLALevelUpdatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserCode", sLALevel.SLALevelUpdatedBy);
            return PartialView(sLALevel);
        }

        // POST: SLALevels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SLALevelCode,SLALevelName,SLALevelStatus,SLALevelCreatedBy,SLALevelCreatedDt,SLALevelUpdatedBy,SLALevelUpdatedDt")] SLALevel sLALevel)
        {
            if (id != sLALevel.SLALevelCode)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sLALevel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SLALevelExists(sLALevel.SLALevelCode))
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
            ViewData["SLALevelCreatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserCode", sLALevel.SLALevelCreatedBy);
            ViewData["SLALevelStatus"] = new SelectList(_context.tbl_aim_status, "StatusCode", "StatusCode", sLALevel.SLALevelStatus);
            ViewData["SLALevelUpdatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserCode", sLALevel.SLALevelUpdatedBy);
            return View(sLALevel);
        }

        // GET: SLALevels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.tbl_aim_slalevel == null)
            {
                return NotFound();
            }

            var sLALevel = await _context.tbl_aim_slalevel
                .Include(s => s.CreatedBy)
                .Include(s => s.Status)
                .Include(s => s.UpdatedBy)
                .FirstOrDefaultAsync(m => m.SLALevelCode == id);
            if (sLALevel == null)
            {
                return NotFound();
            }

            return PartialView(sLALevel);
        }

        // POST: SLALevels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.tbl_aim_slalevel == null)
            {
                return Problem("Entity set 'AimDbContext.tbl_aim_slalevel'  is null.");
            }
            var sLALevel = await _context.tbl_aim_slalevel.FindAsync(id);
            if (sLALevel != null)
            {
                _context.tbl_aim_slalevel.Remove(sLALevel);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SLALevelExists(int id)
        {
          return (_context.tbl_aim_slalevel?.Any(e => e.SLALevelCode == id)).GetValueOrDefault();
        }
    }
}
