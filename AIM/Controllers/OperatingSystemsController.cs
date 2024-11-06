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
    public class OperatingSystemsController : BaseController
    {
        private readonly AimDbContext _context;

        public OperatingSystemsController(AimDbContext context):base(context)
        {
            _context = context;
        }

        // GET: OperatingSystems
        public async Task<IActionResult> Index()
        {
            var aimDbContext = _context.tbl_aim_operatingsystem.Include(o => o.CreatedBy).Include(o => o.Status).Include(o => o.UpdatedBy);
            return PartialView(await aimDbContext.ToListAsync());
        }

        // GET: OperatingSystems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.tbl_aim_operatingsystem == null)
            {
                return NotFound();
            }

            var operatingSystem = await _context.tbl_aim_operatingsystem
                .Include(o => o.CreatedBy)
                .Include(o => o.Status)
                .Include(o => o.UpdatedBy)
                .FirstOrDefaultAsync(m => m.OsCode == id);
            if (operatingSystem == null)
            {
                return NotFound();
            }

            return PartialView(operatingSystem);
        }

        // GET: OperatingSystems/Create
        public IActionResult Create()
        {
            ViewData["OsCreatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName");
            ViewData["OsStatus"] = new SelectList(_context.tbl_aim_status, "StatusCode", "StatusName");
            ViewData["OsUpdatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName");
            return PartialView();
        }
        // POST: OperatingSystems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OsCode,OsName,OsStatus,OsCreatedBy,OsCreatedDt,OsUpdatedBy,OsUpdatedDt")] Models.OperatingSystem operatingSystem)
        {
            operatingSystem.OsCreatedDt = DateTime.Now;
            if (ModelState.IsValid)
            {
                // Fetch the current os_code from tbl_aim_parameters
                var osCodeParameter = await _context.tbl_aim_parameters.FirstOrDefaultAsync(p => p.ParmCode == "os_code");
                if (osCodeParameter != null)
                {
                    // Increment the 'parm_value' in tbl_aim_parameters
                    osCodeParameter.ParmValue++;

                    // Assign the current os_code directly
                    operatingSystem.OsCode = osCodeParameter.ParmValue;

                    operatingSystem.OsCreatedBy = HttpContext.Session.GetString("UserName");

                    // Save changes to the context
                    await _context.SaveChangesAsync();

                    // Continue with adding the 'operatingSystem' object to the context
                    _context.Add(operatingSystem);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }

            ViewData["OsCreatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName", operatingSystem.OsCreatedBy);
            ViewData["OsStatus"] = new SelectList(_context.tbl_aim_status, "StatusCode", "StatusName", operatingSystem.OsStatus);
            ViewData["OsUpdatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName", operatingSystem.OsUpdatedBy);
            return PartialView(operatingSystem);
        }

        // GET: OperatingSystems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.tbl_aim_operatingsystem == null)
            {
                return NotFound();
            }

            var operatingSystem = await _context.tbl_aim_operatingsystem.FindAsync(id);
            if (operatingSystem == null)
            {
                return NotFound();
            }
            ViewData["OsCreatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName", operatingSystem.OsCreatedBy);
            ViewData["OsStatus"] = new SelectList(_context.tbl_aim_status, "StatusCode", "StatusName", operatingSystem.OsStatus);
            ViewData["OsUpdatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName", operatingSystem.OsUpdatedBy);
            return PartialView(operatingSystem);
        }

        // POST: OperatingSystems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OsCode,OsName,OsStatus,OsCreatedBy,OsCreatedDt,OsUpdatedBy,OsUpdatedDt")] Models.OperatingSystem operatingSystem)
        {
            if (id != operatingSystem.OsCode)
            {
                return NotFound();
            }
            operatingSystem.OsUpdatedDt = DateTime.Now;
            if (ModelState.IsValid)
            {
                try
                {
                    operatingSystem.OsUpdatedBy = HttpContext.Session.GetString("UserName");
                    _context.Update(operatingSystem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OperatingSystemExists(operatingSystem.OsCode))
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
            ViewData["OsCreatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName", operatingSystem.OsCreatedBy);
            ViewData["OsStatus"] = new SelectList(_context.tbl_aim_status, "StatusCode", "StatusName", operatingSystem.OsStatus);
            ViewData["OsUpdatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName", operatingSystem.OsUpdatedBy);
            return PartialView(operatingSystem);
        }

        // GET: OperatingSystems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.tbl_aim_operatingsystem == null)
            {
                return NotFound();
            }

            var operatingSystem = await _context.tbl_aim_operatingsystem
                .Include(o => o.CreatedBy)
                .Include(o => o.Status)
                .Include(o => o.UpdatedBy)
                .FirstOrDefaultAsync(m => m.OsCode == id);
            if (operatingSystem == null)
            {
                return NotFound();
            }

            return PartialView(operatingSystem);
        }

        // POST: OperatingSystems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.tbl_aim_operatingsystem == null)
            {
                return Problem("Entity set 'AimDbContext.tbl_aim_operatingsystem'  is null.");
            }
            var operatingSystem = await _context.tbl_aim_operatingsystem.FindAsync(id);
            if (operatingSystem != null)
            {
                _context.tbl_aim_operatingsystem.Remove(operatingSystem);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OperatingSystemExists(int id)
        {
          return (_context.tbl_aim_operatingsystem?.Any(e => e.OsCode == id)).GetValueOrDefault();
        }
    }
}
