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
    public class FunctionalAreasController : BaseController
    {
        private readonly AimDbContext _context;

        public FunctionalAreasController(AimDbContext context):base(context)
        {
            _context = context;
        }

        // GET: FunctionalAreas
        public async Task<IActionResult> Index()
        {
            var aimDbContext = _context.tbl_aim_functionalarea.Include(f => f.CreatedBy).Include(f => f.Status).Include(f => f.UpdatedBy);
            return PartialView(await aimDbContext.ToListAsync());
        }

        // GET: FunctionalAreas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.tbl_aim_functionalarea == null)
            {
                return NotFound();
            }

            var functionalArea = await _context.tbl_aim_functionalarea
                .Include(f => f.CreatedBy)
                .Include(f => f.Status)
                .Include(f => f.UpdatedBy)
                .FirstOrDefaultAsync(m => m.FunctionalCode == id);
            if (functionalArea == null)
            {
                return NotFound();
            }

            return PartialView(functionalArea);
        }

        // GET: FunctionalAreas/Create
        public IActionResult Create()
        {
            ViewData["FunctionalCreatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName");
            ViewData["FunctionalStatus"] = new SelectList(_context.tbl_aim_status, "StatusCode", "StatusName");
            ViewData["FunctionalUpdatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName");
            return PartialView();
        }

        // POST: FunctionalAreas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FunctionalCode,FunctionalName,FunctionalStatus,FunctionalCreatedBy,FunctionalCreatedDt,FunctionalUpdatedBy,FunctionalUpdatedDt")] FunctionalArea functionalArea)
        {
            functionalArea.FunctionalCreatedDt = DateTime.Now;
            if (ModelState.IsValid)
            {
                // Fetch the current functional_code from tbl_aim_parameters
                var functionalCodeParameter = await _context.tbl_aim_parameters.FirstOrDefaultAsync(p => p.ParmCode == "func_no");
                if (functionalCodeParameter != null)
                {
                    // Increment the 'parm_value' in tbl_aim_parameters
                    functionalCodeParameter.ParmValue++;

                    // Assign the current functional_code directly
                    functionalArea.FunctionalCode = functionalCodeParameter.ParmValue;

                    functionalArea.FunctionalCreatedBy = HttpContext.Session.GetString("UserName");

                    // Save changes to the context
                    await _context.SaveChangesAsync();

                    // Continue with adding the 'functionalArea' object to the context
                    _context.Add(functionalArea);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }

            ViewData["FunctionalCreatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName", functionalArea.FunctionalCreatedBy);
            ViewData["FunctionalStatus"] = new SelectList(_context.tbl_aim_status, "StatusCode", "StatusName", functionalArea.FunctionalStatus);
            ViewData["FunctionalUpdatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName", functionalArea.FunctionalUpdatedBy);
            return PartialView(functionalArea);
        }


        // GET: FunctionalAreas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.tbl_aim_functionalarea == null)
            {
                return NotFound();
            }

            var functionalArea = await _context.tbl_aim_functionalarea.FindAsync(id);
            if (functionalArea == null)
            {
                return NotFound();
            }
            ViewData["FunctionalCreatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName", functionalArea.FunctionalCreatedBy);
            ViewData["FunctionalStatus"] = new SelectList(_context.tbl_aim_status, "StatusCode", "StatusName", functionalArea.FunctionalStatus);
            ViewData["FunctionalUpdatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName", functionalArea.FunctionalUpdatedBy);
            return PartialView(functionalArea);
        }

        // POST: FunctionalAreas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FunctionalCode,FunctionalName,FunctionalStatus,FunctionalCreatedBy,FunctionalCreatedDt,FunctionalUpdatedBy,FunctionalUpdatedDt")] FunctionalArea functionalArea)
        {
            if (id != functionalArea.FunctionalCode)
            {
                return NotFound();
            }
            functionalArea.FunctionalCreatedDt = DateTime.Now;
            if (ModelState.IsValid)
            {
                try
                {
                    functionalArea.FunctionalCreatedBy = HttpContext.Session.GetString("UserName");
                    _context.Update(functionalArea);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FunctionalAreaExists(functionalArea.FunctionalCode))
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
            ViewData["FunctionalCreatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName", functionalArea.FunctionalCreatedBy);
            ViewData["FunctionalStatus"] = new SelectList(_context.tbl_aim_status, "StatusCode", "StatusName", functionalArea.FunctionalStatus);
            ViewData["FunctionalUpdatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName", functionalArea.FunctionalUpdatedBy);
            return PartialView(functionalArea);
        }

        // GET: FunctionalAreas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.tbl_aim_functionalarea == null)
            {
                return NotFound();
            }

            var functionalArea = await _context.tbl_aim_functionalarea
                .Include(f => f.CreatedBy)
                .Include(f => f.Status)
                .Include(f => f.UpdatedBy)
                .FirstOrDefaultAsync(m => m.FunctionalCode == id);
            if (functionalArea == null)
            {
                return NotFound();
            }

            return PartialView(functionalArea);
        }

        // POST: FunctionalAreas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.tbl_aim_functionalarea == null)
            {
                return Problem("Entity set 'AimDbContext.tbl_aim_functionalarea'  is null.");
            }
            var functionalArea = await _context.tbl_aim_functionalarea.FindAsync(id);
            if (functionalArea != null)
            {
                _context.tbl_aim_functionalarea.Remove(functionalArea);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FunctionalAreaExists(int id)
        {
          return (_context.tbl_aim_functionalarea?.Any(e => e.FunctionalCode == id)).GetValueOrDefault();
        }
    }
}
