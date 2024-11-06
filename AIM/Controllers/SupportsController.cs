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
    public class SupportsController : BaseController
    {
        private readonly AimDbContext _context;

        public SupportsController(AimDbContext context):base(context)
        {
            _context = context;
        }

        // GET: Supports
        public async Task<IActionResult> Index()
        {
            var aimDbContext = _context.tbl_aim_supports.Include(s => s.CreatedBy).Include(s => s.Status).Include(s => s.UpdatedBy);
            return PartialView(await aimDbContext.ToListAsync());
        }

        // GET: Supports/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.tbl_aim_supports == null)
            {
                return NotFound();
            }

            var support = await _context.tbl_aim_supports
                .Include(s => s.CreatedBy)
                .Include(s => s.Status) 
                .Include(s => s.UpdatedBy)
                .FirstOrDefaultAsync(m => m.SupportCode == id);
            if (support == null)
            {
                return NotFound();
            }

            return PartialView(support);
        }

        // GET: Supports/Create
        public IActionResult Create()
        {
            ViewData["SupportCreatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName");
            ViewData["SupportStatus"] = new SelectList(_context.tbl_aim_status, "StatusCode", "StatusName");
            ViewData["SupportUpdatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName");
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SupportCode,SupportName,SupportStatus,SupportCreatedBy,SupportCreatedDt,SupportUpdatedBy,SupportUpdatedDt")] Support support)
        {
            if (ModelState.IsValid)
            {
                // Fetch the current supp_no from tbl_aim_parameters
                var suppNoParameter = await _context.tbl_aim_parameters.FirstOrDefaultAsync(p => p.ParmCode == "supp_no");

                if (suppNoParameter != null && suppNoParameter.ParmValue != null)
                {
                    // Assign the currentSuppNo directly
                    var currentSuppNo = suppNoParameter.ParmValue + 1;

                    // Update the SupportCode property of the incoming 'support' object
                    support.SupportCode = currentSuppNo;
                    support.SupportCreatedBy = HttpContext.Session.GetString("UserName");
                    support.SupportCreatedDt = DateTime.Now;

                    // Increment the currentSuppNo by 1 for the next support
                    suppNoParameter.ParmValue = currentSuppNo;

                    _context.Add(support);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }

            // If ModelState is not valid or there was an issue with the parameter, return to the create view
            ViewData["SupportCreatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName", support.SupportCreatedBy);
            ViewData["SupportStatus"] = new SelectList(_context.tbl_aim_status, "StatusCode", "StatusName", support.SupportStatus);
            ViewData["SupportUpdatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName", support.SupportUpdatedBy);
            return PartialView(support);
        }

        // GET: Supports/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.tbl_aim_supports == null)
            {
                return NotFound();
            }

            var support = await _context.tbl_aim_supports.FindAsync(id);
            if (support == null)
            {
                return NotFound();
            }
            ViewData["SupportCreatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName", support.SupportCreatedBy);
            ViewData["SupportStatus"] = new SelectList(_context.tbl_aim_status, "StatusCode", "StatusName", support.SupportStatus);
            ViewData["SupportUpdatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName", support.SupportUpdatedBy);
            return PartialView(support);
        }

        // POST: Supports/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SupportCode,SupportName,SupportStatus,SupportCreatedBy,SupportCreatedDt,SupportUpdatedBy,SupportUpdatedDt")] Support support)
        {
            if (id != support.SupportCode)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    support.SupportUpdatedDt = DateTime.Now;
                    support.SupportUpdatedBy = HttpContext.Session.GetString("UserName");
                    _context.Update(support);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SupportExists(support.SupportCode))
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
            ViewData["SupportCreatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName", support.SupportCreatedBy);
            ViewData["SupportStatus"] = new SelectList(_context.tbl_aim_status, "StatusCode", "StatusName", support.SupportStatus);
            ViewData["SupportUpdatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName", support.SupportUpdatedBy);
            return PartialView(support);
        }

        // GET: Supports/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.tbl_aim_supports == null)
            {
                return NotFound();
            }

            var support = await _context.tbl_aim_supports
                .Include(s => s.CreatedBy)
                .Include(s => s.Status)
                .Include(s => s.UpdatedBy)
                .FirstOrDefaultAsync(m => m.SupportCode == id);
            if (support == null)
            {
                return NotFound();
            }

            return PartialView(support);
        }

        // POST: Supports/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.tbl_aim_supports == null)
            {
                return Problem("Entity set 'AimDbContext.tbl_aim_supports'  is null.");
            }
            var support = await _context.tbl_aim_supports.FindAsync(id);
            if (support != null)
            {
                _context.tbl_aim_supports.Remove(support);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SupportExists(int id)
        {
          return (_context.tbl_aim_supports?.Any(e => e.SupportCode == id)).GetValueOrDefault();
        }
    }
}
