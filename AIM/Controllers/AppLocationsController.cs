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
    public class AppLocationsController : BaseController
    {
        private readonly AimDbContext _context;

        public AppLocationsController(AimDbContext context) : base(context)
        {
            _context = context;
        }

        // GET: AppLocations
        public async Task<IActionResult> Index()
        {
            var aimDbContext = _context.tbl_aim_apploc.Include(a => a.CreatedBy).Include(a => a.Status).Include(a => a.UpdatedBy);
            return PartialView(await aimDbContext.ToListAsync());
        }

        // GET: AppLocations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.tbl_aim_apploc == null)
            {
                return NotFound();
            }

            var appLocation = await _context.tbl_aim_apploc
                .Include(a => a.CreatedBy)
                .Include(a => a.Status)
                .Include(a => a.UpdatedBy)
                .FirstOrDefaultAsync(m => m.LocationCode == id);
            if (appLocation == null)
            {
                return NotFound();
            }

            return PartialView(appLocation);
        }

        // GET: AppLocations/Create
        public IActionResult Create()
        {
            ViewData["LocationCreatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName");
            ViewData["LocationStatus"] = new SelectList(_context.tbl_aim_status, "StatusCode", "StatusName");
            ViewData["LocationUpdatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName");
            return PartialView();
        }

        // POST: AppLocations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LocationCode,LocationName,LocationStatus,LocationCreatedBy,LocationCreatedDt,LocationUpdatedBy,LocationUpdatedDt")] AppLocation appLocation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(appLocation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["LocationCreatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserCode", appLocation.LocationCreatedBy);
            ViewData["LocationStatus"] = new SelectList(_context.tbl_aim_status, "StatusCode", "StatusCode", appLocation.LocationStatus);
            ViewData["LocationUpdatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserCode", appLocation.LocationUpdatedBy);
            return PartialView(appLocation);
        }

        // GET: AppLocations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.tbl_aim_apploc == null)
            {
                return NotFound();
            }

            var appLocation = await _context.tbl_aim_apploc.FindAsync(id);
            if (appLocation == null)
            {
                return NotFound();
            }
            ViewData["LocationCreatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserCode", appLocation.LocationCreatedBy);
            ViewData["LocationStatus"] = new SelectList(_context.tbl_aim_status, "StatusCode", "StatusCode", appLocation.LocationStatus);
            ViewData["LocationUpdatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserCode", appLocation.LocationUpdatedBy);
            return PartialView(appLocation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LocationCode,LocationName,LocationStatus,LocationCreatedBy,LocationCreatedDt,LocationUpdatedBy,LocationUpdatedDt")] AppLocation updatedAppLocation)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Check if the Location with the given id exists
                    var existingAppLocation = await _context.tbl_aim_apploc.FindAsync(id);

                    if (existingAppLocation == null)
                    {
                        // If not found, create a new entity with the specified key values
                        var newAppLocation = new AppLocation
                        {
                            LocationCode = id,
                            LocationName = updatedAppLocation.LocationName,
                            LocationStatus = updatedAppLocation.LocationStatus,
                            LocationCreatedBy = updatedAppLocation.LocationCreatedBy,
                            LocationCreatedDt = updatedAppLocation.LocationCreatedDt,
                            LocationUpdatedBy = updatedAppLocation.LocationUpdatedBy,
                            LocationUpdatedDt = updatedAppLocation.LocationUpdatedDt
                            // Add other properties as needed
                        };

                        _context.tbl_aim_apploc.Add(newAppLocation);
                        await _context.SaveChangesAsync();

                        // Return a JSON response indicating success
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        // If found, update the existing entity with the new data
                        existingAppLocation.LocationName = updatedAppLocation.LocationName;
                        existingAppLocation.LocationStatus = updatedAppLocation.LocationStatus;
                        existingAppLocation.LocationCreatedBy = updatedAppLocation.LocationCreatedBy;
                        existingAppLocation.LocationCreatedDt = updatedAppLocation.LocationCreatedDt;
                        existingAppLocation.LocationUpdatedBy = updatedAppLocation.LocationUpdatedBy;
                        existingAppLocation.LocationUpdatedDt = updatedAppLocation.LocationUpdatedDt;
                        // Update other properties as needed

                        await _context.SaveChangesAsync();

                        // Return a JSON response indicating success
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    // Handle concurrency conflict and return a JSON response indicating an error
                    return Json(new { success = false, error = "Concurrency conflict occurred." });
                }
            }

            // If ModelState is not valid, return a JSON response indicating validation errors
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            return Json(new { success = false, errors });
        }


        // GET: AppLocations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.tbl_aim_apploc == null)
            {
                return NotFound();
            }

            var appLocation = await _context.tbl_aim_apploc
                .Include(a => a.CreatedBy)
                .Include(a => a.Status)
                .Include(a => a.UpdatedBy)
                .FirstOrDefaultAsync(m => m.LocationCode == id);
            if (appLocation == null)
            {
                return NotFound();
            }

            return PartialView(appLocation);
        }

        // POST: AppLocations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.tbl_aim_apploc == null)
            {
                return Problem("Entity set 'AimDbContext.tbl_aim_apploc'  is null.");
            }
            var appLocation = await _context.tbl_aim_apploc.FindAsync(id);
            if (appLocation != null)
            {
                _context.tbl_aim_apploc.Remove(appLocation);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AppLocationExists(int id)
        {
          return (_context.tbl_aim_apploc?.Any(e => e.LocationCode == id)).GetValueOrDefault();
        }
    }
}
