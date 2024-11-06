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
    public class BusinessImpactsController : BaseController
    {
        private readonly AimDbContext _context;

        public BusinessImpactsController(AimDbContext context) : base(context)
        {
            _context = context;
        }

        // GET: BusinessImpacts
        public async Task<IActionResult> Index()
        {
            var aimDbContext = _context.tbl_aim_businessimpact.Include(b => b.CreatedBy).Include(b => b.Status).Include(b => b.UpdatedBy);
            return PartialView(await aimDbContext.ToListAsync());
        }

        // GET: BusinessImpacts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.tbl_aim_businessimpact == null)
            {
                return NotFound();
            }

            var businessImpact = await _context.tbl_aim_businessimpact
                .Include(b => b.CreatedBy)
                .Include(b => b.Status)
                .Include(b => b.UpdatedBy)
                .FirstOrDefaultAsync(m => m.ImpactCode == id);
            if (businessImpact == null)
            {
                return NotFound();
            }

            return PartialView(businessImpact);
        }

        // GET: BusinessImpacts/Create
        public IActionResult Create()
        {
            ViewData["ImpactCreatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName");
            ViewData["ImpactStatus"] = new SelectList(_context.tbl_aim_status, "StatusCode", "StatusName");
            ViewData["ImpactUpdatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName");
            return PartialView();
        }

        // POST: BusinessImpacts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ImpactCode,ImpactName,ImpactStatus,ImpactCreatedBy,ImpactCreatedDt,ImpactUpdatedBy,ImpactUpdatedDt")] BusinessImpact businessImpact)
        {
            if (ModelState.IsValid)
            {
                // Fetch the current impact_code from tbl_aim_parameters
                var impactCodeParameter = await _context.tbl_aim_parameters.FirstOrDefaultAsync(p => p.ParmCode == "impact_no");
                if (impactCodeParameter != null && impactCodeParameter.ParmValue != null)
                {
                    // Assign the current impact_code directly
                    var currentImpactCode = impactCodeParameter.ParmValue;

                    // Update the impact_code in the incoming 'businessImpact' object
                    businessImpact.ImpactCode = currentImpactCode + 1;

                    // Increment the current impact_code by 1 for the next impact
                    impactCodeParameter.ParmValue = currentImpactCode + 1;
                    businessImpact.ImpactCreatedBy = HttpContext.Session.GetString("UserName");
                    _context.Add(businessImpact);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }

            ViewData["ImpactCreatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserCode", businessImpact.ImpactCreatedBy);
            ViewData["ImpactStatus"] = new SelectList(_context.tbl_aim_status, "StatusCode", "StatusCode", businessImpact.ImpactStatus);
            ViewData["ImpactUpdatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserCode", businessImpact.ImpactUpdatedBy);
            return View(businessImpact);
        }


        // GET: BusinessImpacts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.tbl_aim_businessimpact == null)
            {
                return NotFound();
            }

            var businessImpact = await _context.tbl_aim_businessimpact.FindAsync(id);
            if (businessImpact == null)
            {
                return NotFound();
            }
            ViewData["ImpactCreatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserCode", businessImpact.ImpactCreatedBy);
            ViewData["ImpactStatus"] = new SelectList(_context.tbl_aim_status, "StatusCode", "StatusCode", businessImpact.ImpactStatus);
            ViewData["ImpactUpdatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserCode", businessImpact.ImpactUpdatedBy);
            return PartialView(businessImpact);
        }

        // POST: BusinessImpacts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ImpactCode,ImpactName,ImpactStatus,ImpactCreatedBy,ImpactCreatedDt,ImpactUpdatedBy,ImpactUpdatedDt")] BusinessImpact businessImpact)
        {
            if (id != businessImpact.ImpactCode)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    businessImpact.ImpactUpdatedBy = HttpContext.Session.GetString("UserName");
                    businessImpact.ImpactUpdatedDt = DateTime.Now;
                    _context.Update(businessImpact);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BusinessImpactExists(businessImpact.ImpactCode))
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
            ViewData["ImpactCreatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserCode", businessImpact.ImpactCreatedBy);
            ViewData["ImpactStatus"] = new SelectList(_context.tbl_aim_status, "StatusCode", "StatusCode", businessImpact.ImpactStatus);
            ViewData["ImpactUpdatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserCode", businessImpact.ImpactUpdatedBy);
            return View(businessImpact);
        }

        // GET: BusinessImpacts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.tbl_aim_businessimpact == null)
            {
                return NotFound();
            }

            var businessImpact = await _context.tbl_aim_businessimpact
                .Include(b => b.CreatedBy)
                .Include(b => b.Status)
                .Include(b => b.UpdatedBy)
                .FirstOrDefaultAsync(m => m.ImpactCode == id);
            if (businessImpact == null)
            {
                return NotFound();
            }

            return PartialView(businessImpact);
        }

        // POST: BusinessImpacts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.tbl_aim_businessimpact == null)
            {
                return Problem("Entity set 'AimDbContext.tbl_aim_businessimpact'  is null.");
            }
            var businessImpact = await _context.tbl_aim_businessimpact.FindAsync(id);
            if (businessImpact != null)
            {
                _context.tbl_aim_businessimpact.Remove(businessImpact);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BusinessImpactExists(int id)
        {
          return (_context.tbl_aim_businessimpact?.Any(e => e.ImpactCode == id)).GetValueOrDefault();
        }
    }
}
