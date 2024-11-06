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
    public class RPortOslasController : BaseController
    {
        private readonly AimDbContext _context;

        public RPortOslasController(AimDbContext context) : base(context)
        {
            _context = context;
        }

        // GET: RPortOslas
        public async Task<IActionResult> Index()
        {
            var aimDbContext = _context.tbl_aim_rportosla.Include(r => r.Application).Include(r => r.CreatedBy).Include(r => r.Location).Include(r => r.SLALevel).Include(r => r.Status).Include(r => r.UpdatedBy);
            return PartialView(await aimDbContext.ToListAsync());
        }

        // GET: RPortOslas/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.tbl_aim_rportosla == null)
            {
                return NotFound();
            }

            var rPortOsla = await _context.tbl_aim_rportosla
                .Include(r => r.Application)
                .Include(r => r.CreatedBy)
                .Include(r => r.Location)
                .Include(r => r.SLALevel)
                .Include(r => r.Status)
                .Include(r => r.UpdatedBy)
                .FirstOrDefaultAsync(m => m.RPortOslaCode == id);
            if (rPortOsla == null)
            {
                return NotFound();
            }

            return PartialView(rPortOsla);
        }

        // GET: RPortOslas/Create
        public IActionResult Create()
        {
            ViewData["AppCode"] = new SelectList(_context.tbl_aim_applications, "AppCode", "AppName");
            ViewData["AppCreatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName");
            ViewData["AppLocation"] = new SelectList(_context.tbl_aim_apploc, "LocationCode", "LocationName");
            ViewData["AppSLALevel"] = new SelectList(_context.tbl_aim_slalevel, "SLALevelCode", "SLALevelName");
            ViewData["AppStatus"] = new SelectList(_context.tbl_aim_status, "StatusCode", "StatusName");
            ViewData["AppUpdatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName");
            return PartialView();
        }

        // POST: RPortOslas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RPortOslaCode,AppCode,AppLocation,AppCurrentRPO,AppProposedRPO,AppRTO,AppSLALevel,AppStatus,AppCreatedBy,AppCreatedDt,AppUpdatedBy,AppUpdatedDt")] RPortOsla rPortOsla)
        {
            if (ModelState.IsValid)
            {
                // Set the current date and time
                rPortOsla.AppCreatedDt = DateTime.Now;
                // Calculate the next RPO number
                string nextRPO = CalculateNextRPO();
                // Set the status to 1
                rPortOsla.AppStatus = "1";
                // Set the primary key
                rPortOsla.RPortOslaCode = nextRPO;
                _context.Add(rPortOsla);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AppCode"] = new SelectList(_context.tbl_aim_applications, "AppCode", "AppCode", rPortOsla.AppCode);
            ViewData["AppCreatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserCode", rPortOsla.AppCreatedBy);
            ViewData["AppLocation"] = new SelectList(_context.tbl_aim_apploc, "LocationCode", "LocationCode", rPortOsla.AppLocation);
            ViewData["AppSLALevel"] = new SelectList(_context.tbl_aim_slalevel, "SLALevelCode", "SLALevelCode", rPortOsla.AppSLALevel);
            ViewData["AppStatus"] = new SelectList(_context.tbl_aim_status, "StatusCode", "StatusCode", rPortOsla.AppStatus);
            ViewData["AppUpdatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserCode", rPortOsla.AppUpdatedBy);
            return View(rPortOsla);
        }
        // Helper method to calculate the next RPO number
        private string CalculateNextRPO()
        {
            // Check if there are existing records in the database
            if (!_context.tbl_aim_rportosla.Any())
            {
                // No existing records, set the initial RPO code
                return "RPO000000000001";
            }

            // Find the maximum RPO number from the existing records
            var maxRPO = _context.tbl_aim_rportosla
                .Select(r => r.RPortOslaCode)
                .Max();

            // Parse the numeric part of the RPO code and increment it
            int nextRPONumber = int.Parse(maxRPO.Substring(3)) + 1;

            // Format the next RPO number as "RPO000000000001"
            string nextRPO = $"RPO{nextRPONumber:D12}";

            return nextRPO;
        }

        // GET: RPortOslas/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.tbl_aim_rportosla == null)
            {
                return NotFound();
            }

            var rPortOsla = await _context.tbl_aim_rportosla.FindAsync(id);
            if (rPortOsla == null)
            {
                return NotFound();
            }
            ViewData["AppCode"] = new SelectList(_context.tbl_aim_applications, "AppCode", "AppName", rPortOsla.AppCode);
            ViewData["AppCreatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName", rPortOsla.AppCreatedBy);
            ViewData["AppLocation"] = new SelectList(_context.tbl_aim_apploc, "LocationCode", "LocationName", rPortOsla.AppLocation);
            ViewData["AppSLALevel"] = new SelectList(_context.tbl_aim_slalevel, "SLALevelCode", "SLALevelName", rPortOsla.AppSLALevel);
            ViewData["AppStatus"] = new SelectList(_context.tbl_aim_status, "StatusCode", "StatusName", rPortOsla.AppStatus);
            ViewData["AppUpdatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName", rPortOsla.AppUpdatedBy);
            return PartialView(rPortOsla);
        }

        // POST: RPortOslas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("RPortOslaCode,AppCode,AppLocation,AppCurrentRPO,AppProposedRPO,AppRTO,AppSLALevel,AppStatus,AppCreatedBy,AppCreatedDt,AppUpdatedBy,AppUpdatedDt")] RPortOsla rPortOsla)
        {
            if (id != rPortOsla.RPortOslaCode)
            {
                return NotFound();
            }
            var LoginUser = HttpContext.Session.GetString("UserName");

            if (ModelState.IsValid)
            {
                try
                {
                    rPortOsla.AppUpdatedBy = LoginUser;
                    rPortOsla.AppUpdatedDt = DateTime.Now;
                    _context.Update(rPortOsla);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RPortOslaExists(rPortOsla.RPortOslaCode))
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
            ViewData["AppCode"] = new SelectList(_context.tbl_aim_applications, "AppCode", "AppCode", rPortOsla.AppCode);
            ViewData["AppCreatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserCode", rPortOsla.AppCreatedBy);
            ViewData["AppLocation"] = new SelectList(_context.tbl_aim_apploc, "LocationCode", "LocationCode", rPortOsla.AppLocation);
            ViewData["AppSLALevel"] = new SelectList(_context.tbl_aim_slalevel, "SLALevelCode", "SLALevelCode", rPortOsla.AppSLALevel);
            ViewData["AppStatus"] = new SelectList(_context.tbl_aim_status, "StatusCode", "StatusCode", rPortOsla.AppStatus);
            ViewData["AppUpdatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserCode", rPortOsla.AppUpdatedBy);
            return View(rPortOsla);
        }

        // GET: RPortOslas/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.tbl_aim_rportosla == null)
            {
                return NotFound();
            }

            var rPortOsla = await _context.tbl_aim_rportosla
                .Include(r => r.Application)
                .Include(r => r.CreatedBy)
                .Include(r => r.Location)
                .Include(r => r.SLALevel)
                .Include(r => r.Status)
                .Include(r => r.UpdatedBy)
                .FirstOrDefaultAsync(m => m.RPortOslaCode == id);
            if (rPortOsla == null)
            {
                return NotFound();
            }

            return PartialView(rPortOsla);
        }

        // POST: RPortOslas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.tbl_aim_rportosla == null)
            {
                return Problem("Entity set 'AimDbContext.tbl_aim_rportosla'  is null.");
            }
            var rPortOsla = await _context.tbl_aim_rportosla.FindAsync(id);
            if (rPortOsla != null)
            {
                _context.tbl_aim_rportosla.Remove(rPortOsla);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RPortOslaExists(string id)
        {
          return (_context.tbl_aim_rportosla?.Any(e => e.RPortOslaCode == id)).GetValueOrDefault();
        }
    }
}
