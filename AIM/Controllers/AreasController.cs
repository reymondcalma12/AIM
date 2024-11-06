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
    public class AreasController : BaseController
    {
        private readonly AimDbContext _context;

        public AreasController(AimDbContext context) : base(context)
        {
            _context = context;
        }

        // GET: Areas
        public async Task<IActionResult> Index()
        {
            var aimDbContext = _context.tbl_aim_area.Include(a => a.CreatedBy).Include(a => a.Status).Include(a => a.UpdatedBy);
            return PartialView(await aimDbContext.ToListAsync());
        }

        // GET: Areas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.tbl_aim_area == null)
            {
                return NotFound();
            }

            var area = await _context.tbl_aim_area
                .Include(a => a.CreatedBy)
                .Include(a => a.Status)
                .Include(a => a.UpdatedBy)
                .FirstOrDefaultAsync(m => m.AreaCode == id);
            if (area == null)
            {
                return NotFound();
            }

            return PartialView(area);
        }

        // GET: Areas/Create
        public IActionResult Create()
        {
            ViewData["AreaCreatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName");
            ViewData["AreaStatus"] = new SelectList(_context.tbl_aim_status, "StatusCode", "StatusName");
            ViewData["AreaUpdatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName");
            return PartialView();
        }

        // POST: Areas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AreaCode,AreaName,AreaStatus,AreaCreatedBy,AreaCreatedDt,AreaUpdatedBy,AreaUpdatedDt")] Area area)
        {
            if (ModelState.IsValid)
            {
                // Fetch the current area_code from tbl_aim_parameters
                var areaCodeParameter = await _context.tbl_aim_parameters.FirstOrDefaultAsync(p => p.ParmCode == "area_code");
                if (areaCodeParameter != null && areaCodeParameter.ParmValue != null)
                {
                    // Assign the currentAreaCode directly
                    var currentAreaCode = areaCodeParameter.ParmValue;

                    // Increment the currentAreaCode by 1 to get the new area_code
                    var newAreaCode = currentAreaCode + 1;

                    // Update the area_code in the incoming 'area' object
                    area.AreaCode = newAreaCode;

                    // Update the 'parm_value' in tbl_aim_parameters
                    areaCodeParameter.ParmValue = newAreaCode;

                    area.AreaCreatedBy = HttpContext.Session.GetString("UserName");

                    // Save changes to the context
                    await _context.SaveChangesAsync();

                    // Continue with adding the 'area' object to the context
                    _context.Add(area);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }

            ViewData["AreaCreatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserCode", area.AreaCreatedBy);
            ViewData["AreaStatus"] = new SelectList(_context.tbl_aim_status, "StatusCode", "StatusCode", area.AreaStatus);
            ViewData["AreaUpdatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserCode", area.AreaUpdatedBy);
            return View(area);
        }



        // GET: Areas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.tbl_aim_area == null)
            {
                return NotFound();
            }

            var area = await _context.tbl_aim_area.FindAsync(id);
            if (area == null)
            {
                return NotFound();
            }
            ViewData["AreaCreatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserCode", area.AreaCreatedBy);
            ViewData["AreaStatus"] = new SelectList(_context.tbl_aim_status, "StatusCode", "StatusCode", area.AreaStatus);
            ViewData["AreaUpdatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserCode", area.AreaUpdatedBy);
            return PartialView(area);
        }

        // POST: Areas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AreaCode,AreaName,AreaStatus,AreaCreatedBy,AreaCreatedDt,AreaUpdatedBy,AreaUpdatedDt")] Area updatedArea)
        {
            if (id != updatedArea.AreaCode)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Retrieve the existing entity with the same key
                    var existingArea = await _context.tbl_aim_area.FindAsync(id);

                    if (existingArea == null)
                    {
                        return NotFound();
                    }

                    // Update the properties of the existing entity
                    existingArea.AreaName = updatedArea.AreaName;
                    existingArea.AreaStatus = updatedArea.AreaStatus;
                    existingArea.AreaCreatedBy = updatedArea.AreaCreatedBy;
                    existingArea.AreaCreatedDt = updatedArea.AreaCreatedDt;
                    existingArea.AreaUpdatedBy = HttpContext.Session.GetString("UserName");
                    existingArea.AreaUpdatedDt = DateTime.Now ;

                    _context.Update(existingArea);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AreaExists(updatedArea.AreaCode))
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
            ViewData["AreaCreatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserCode", updatedArea.AreaCreatedBy);
            ViewData["AreaStatus"] = new SelectList(_context.tbl_aim_status, "StatusCode", "StatusCode", updatedArea.AreaStatus);
            ViewData["AreaUpdatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserCode", updatedArea.AreaUpdatedBy);
            return PartialView(updatedArea);
        }


        // GET: Areas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.tbl_aim_area == null)
            {
                return NotFound();
            }

            var area = await _context.tbl_aim_area
                .Include(a => a.CreatedBy)
                .Include(a => a.Status)
                .Include(a => a.UpdatedBy)
                .FirstOrDefaultAsync(m => m.AreaCode == id);
            if (area == null)
            {
                return NotFound();
            }

            return PartialView(area);
        }

        // POST: Areas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.tbl_aim_area == null)
            {
                return Problem("Entity set 'AimDbContext.tbl_aim_area'  is null.");
            }
            var area = await _context.tbl_aim_area.FindAsync(id);
            if (area != null)
            {
                _context.tbl_aim_area.Remove(area);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AreaExists(int id)
        {
          return (_context.tbl_aim_area?.Any(e => e.AreaCode == id)).GetValueOrDefault();
        }
    }
}
