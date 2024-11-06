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
    public class PrintSpoolersController : BaseController
    {
        private readonly AimDbContext _context;

        public PrintSpoolersController(AimDbContext context):base(context)
        {
            _context = context;
        }

        // GET: PrintSpoolers
        public async Task<IActionResult> Index()
        {
            var aimDbContext = _context.tbl_aim_printspooler.Include(p => p.CreatedBy).Include(p => p.Status).Include(p => p.UpdatedBy);
            return PartialView(await aimDbContext.ToListAsync());
        }

        // GET: PrintSpoolers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.tbl_aim_printspooler == null)
            {
                return NotFound();
            }

            var printSpooler = await _context.tbl_aim_printspooler
                .Include(p => p.CreatedBy)
                .Include(p => p.Status)
                .Include(p => p.UpdatedBy)
                .FirstOrDefaultAsync(m => m.SpoolerCode == id);
            if (printSpooler == null)
            {
                return NotFound();
            }

            return PartialView(printSpooler);
        }

        // GET: PrintSpoolers/Create
        public IActionResult Create()
        {
            ViewData["SpoolerCreatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName");
            ViewData["SpoolerStatus"] = new SelectList(_context.tbl_aim_status, "StatusCode", "StatusName");
            ViewData["SpoolerUpdatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName");
            return PartialView();
        }

        // POST: PrintSpoolers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SpoolerCode,SpoolerName,SpoolerStatus,SpoolerCreatedBy,SpoolerCreatedDt,SpoolerUpdatedBy,SpoolerUpdatedDt")] PrintSpooler printSpooler)
        {
            printSpooler.SpoolerCreatedDt = DateTime.Now;
            if (ModelState.IsValid)
            {
                // Fetch the current spooler_code from tbl_aim_parameters
                var spoolerCodeParameter = await _context.tbl_aim_parameters.FirstOrDefaultAsync(p => p.ParmCode == "spooler_no");
                if (spoolerCodeParameter != null)
                {
                    // Increment the 'parm_value' in tbl_aim_parameters
                    spoolerCodeParameter.ParmValue++;

                    // Assign the current spooler_code directly
                    printSpooler.SpoolerCode = spoolerCodeParameter.ParmValue;

                    printSpooler.SpoolerCreatedBy = HttpContext.Session.GetString("UserName");

                    // Save changes to the context
                    await _context.SaveChangesAsync();

                    // Continue with adding the 'printSpooler' object to the context
                    _context.Add(printSpooler);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }

            ViewData["SpoolerCreatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName", printSpooler.SpoolerCreatedBy);
            ViewData["SpoolerStatus"] = new SelectList(_context.tbl_aim_status, "StatusCode", "StatusName", printSpooler.SpoolerStatus);
            ViewData["SpoolerUpdatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName", printSpooler.SpoolerUpdatedBy);
            return PartialView(printSpooler);
        }


        // GET: PrintSpoolers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.tbl_aim_printspooler == null)
            {
                return NotFound();
            }

            var printSpooler = await _context.tbl_aim_printspooler.FindAsync(id);
            if (printSpooler == null)
            {
                return NotFound();
            }
            ViewData["SpoolerCreatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName", printSpooler.SpoolerCreatedBy);
            ViewData["SpoolerStatus"] = new SelectList(_context.tbl_aim_status, "StatusCode", "StatusName", printSpooler.SpoolerStatus);
            ViewData["SpoolerUpdatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName", printSpooler.SpoolerUpdatedBy);
            return PartialView(printSpooler);
        }

        // POST: PrintSpoolers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SpoolerCode,SpoolerName,SpoolerStatus,SpoolerCreatedBy,SpoolerCreatedDt,SpoolerUpdatedBy,SpoolerUpdatedDt")] PrintSpooler printSpooler)
        {
            if (id != printSpooler.SpoolerCode)
            {
                return NotFound();
            }
            printSpooler.SpoolerUpdatedDt = DateTime.Now;
                
            if (ModelState.IsValid)
            {
                try
                {
                    printSpooler.SpoolerUpdatedBy = HttpContext.Session.GetString("UserName");
                    _context.Update(printSpooler);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PrintSpoolerExists(printSpooler.SpoolerCode))
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
            ViewData["SpoolerCreatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName", printSpooler.SpoolerCreatedBy);
            ViewData["SpoolerStatus"] = new SelectList(_context.tbl_aim_status, "StatusCode", "StatusName", printSpooler.SpoolerStatus);
            ViewData["SpoolerUpdatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName", printSpooler.SpoolerUpdatedBy);
            return PartialView(printSpooler);
        }

        // GET: PrintSpoolers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.tbl_aim_printspooler == null)
            {
                return NotFound();
            }

            var printSpooler = await _context.tbl_aim_printspooler
                .Include(p => p.CreatedBy)
                .Include(p => p.Status)
                .Include(p => p.UpdatedBy)
                .FirstOrDefaultAsync(m => m.SpoolerCode == id);
            if (printSpooler == null)
            {
                return NotFound();
            }

            return PartialView(printSpooler);
        }

        // POST: PrintSpoolers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.tbl_aim_printspooler == null)
            {
                return Problem("Entity set 'AimDbContext.tbl_aim_printspooler'  is null.");
            }
            var printSpooler = await _context.tbl_aim_printspooler.FindAsync(id);
            if (printSpooler != null)
            {
                _context.tbl_aim_printspooler.Remove(printSpooler);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PrintSpoolerExists(int id)
        {
          return (_context.tbl_aim_printspooler?.Any(e => e.SpoolerCode == id)).GetValueOrDefault();
        }
    }
}
