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
    public class AppBusinessImpactsController : BaseController
    {
        private readonly AimDbContext _context;

        public AppBusinessImpactsController(AimDbContext context) : base(context)
        {
            _context = context;
        }

        // GET: AppBusinessImpacts
        public async Task<IActionResult> Index()
        {
            var aimDbContext = _context.tbl_aim_appbusimpact.Include(a => a.BusinessImpact).Include(a => a.CreatedBy).Include(a => a.RPortOsla);
            return PartialView(await aimDbContext.ToListAsync());
        }

        // GET: AppBusinessImpacts/Details/RPortOslaCode-ImpactCode
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Split the id parameter into parts
            var idParts = id.Split('-');
            if (idParts.Length != 2)
            {
                return NotFound();
            }

            string RPortOslaCode = idParts[0];
            if (!int.TryParse(idParts[1], out int ImpactCode))
            {
                return BadRequest("Invalid ImpactCode"); // Handle the case where the conversion fails.
            }

            var appBusinessImpact = await _context.tbl_aim_appbusimpact
                .Include(a => a.BusinessImpact)
                .Include(a => a.CreatedBy)
                .Include(a => a.RPortOsla)
                .FirstOrDefaultAsync(m => m.RPortOslaCode == RPortOslaCode && m.ImpactCode == ImpactCode);

            if (appBusinessImpact == null)
            {
                return NotFound();
            }

            return PartialView(appBusinessImpact);
        }



        // GET: AppBusinessImpacts/Create
        public IActionResult Create()
        {
            ViewData["ImpactCode"] = new SelectList(_context.tbl_aim_businessimpact, "ImpactCode", "ImpactName");
            ViewData["ImpactCreatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName");
            ViewData["RPortOslaCode"] = new SelectList(_context.tbl_aim_rportosla, "RPortOslaCode", "RPortOslaCode");
            return PartialView();
        }

        // POST: AppBusinessImpacts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RPortOslaCode,ImpactCode,ImpactCreatedBy,ImpactCreatedDt")] AppBusinessImpact appBusinessImpact)
        {
            appBusinessImpact.ImpactCreatedDt = DateTime.Now;

            if (ModelState.IsValid)
            {
                appBusinessImpact.ImpactCreatedBy = HttpContext.Session.GetString("UserName");
                _context.Add(appBusinessImpact);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ImpactCode"] = new SelectList(_context.tbl_aim_businessimpact, "ImpactCode", "ImpactName", appBusinessImpact.ImpactCode);
            ViewData["ImpactCreatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName", appBusinessImpact.ImpactCreatedBy);
            ViewData["RPortOslaCode"] = new SelectList(_context.tbl_aim_rportosla, "RPortOslaCode", "RPortOslaCode", appBusinessImpact.RPortOslaCode);
            return PartialView(appBusinessImpact);
        }

        // GET: AppBusinessImpacts/Edit/RPortOslaCode-ImpactCode
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Split the id parameter into parts
            var idParts = id.Split('-');
            if (idParts.Length != 2)
            {
                return NotFound();
            }

            string RPortOslaCode = idParts[0];
            if (!int.TryParse(idParts[1], out int ImpactCode))
            {
                return BadRequest("Invalid ImpactCode"); // Handle the case where the conversion fails.
            }

            var appBusinessImpact = await _context.tbl_aim_appbusimpact.FindAsync(RPortOslaCode, ImpactCode);
            if (appBusinessImpact == null)
            {
                return NotFound();
            }

            ViewData["ImpactCode"] = new SelectList(_context.tbl_aim_businessimpact, "ImpactCode", "ImpactCode", appBusinessImpact.ImpactCode);
            ViewData["ImpactCreatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserCode", appBusinessImpact.ImpactCreatedBy);
            ViewData["RPortOslaCode"] = new SelectList(_context.tbl_aim_rportosla, "RPortOslaCode", "RPortOslaCode", appBusinessImpact.RPortOslaCode);

            return PartialView(appBusinessImpact);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("RPortOslaCode, ImpactCode, ImpactCreatedBy, ImpactCreatedDt")] AppBusinessImpact updatedAppBusinessImpact)
        {
            if (id != updatedAppBusinessImpact.RPortOslaCode)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Find the existing record with the composite key (RPortOslaCode and ImpactCode)
                    var existingAppBusinessImpact = await _context.tbl_aim_appbusimpact
                        .Where(a => a.RPortOslaCode == updatedAppBusinessImpact.RPortOslaCode && a.ImpactCode == updatedAppBusinessImpact.ImpactCode)
                        .FirstOrDefaultAsync();

                    if (existingAppBusinessImpact == null)
                    {
                        // If not found, create a new entity with the specified key values
                        var newAppBusinessImpact = new AppBusinessImpact
                        {
                            RPortOslaCode = updatedAppBusinessImpact.RPortOslaCode,
                            ImpactCode = updatedAppBusinessImpact.ImpactCode,
                            ImpactCreatedBy = updatedAppBusinessImpact.ImpactCreatedBy,
                            ImpactCreatedDt = updatedAppBusinessImpact.ImpactCreatedDt
                            // Add other properties as needed
                        };

                        _context.tbl_aim_appbusimpact.Add(newAppBusinessImpact);
                        await _context.SaveChangesAsync();

                        return RedirectToAction(nameof(Index)); // Redirect to the index page on success
                    }
                    else
                    {
                        // If found, update the existing entity with the new data
                        existingAppBusinessImpact.ImpactCreatedBy = updatedAppBusinessImpact.ImpactCreatedBy;
                        existingAppBusinessImpact.ImpactCreatedDt = updatedAppBusinessImpact.ImpactCreatedDt;
                        existingAppBusinessImpact.RPortOslaCode = updatedAppBusinessImpact.RPortOslaCode;
                        existingAppBusinessImpact.ImpactCode = updatedAppBusinessImpact.ImpactCode;

                        // Update other properties as needed

                        await _context.SaveChangesAsync();

                        return RedirectToAction(nameof(Index)); // Redirect to the index page on success
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    // Handle concurrency conflict and return a view with an error message
                    ModelState.AddModelError(string.Empty, "Concurrency conflict occurred.");
                    ViewData["ImpactCode"] = new SelectList(_context.tbl_aim_businessimpact, "ImpactCode", "ImpactName", updatedAppBusinessImpact.ImpactCode);
                    ViewData["ImpactCreatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName", updatedAppBusinessImpact.ImpactCreatedBy);
                    ViewData["RPortOslaCode"] = new SelectList(_context.tbl_aim_rportosla, "RPortOslaCode", "RPortOslaCode", updatedAppBusinessImpact.RPortOslaCode);
                    return PartialView(updatedAppBusinessImpact);
                }
            }

            // If ModelState is not valid, return the view with validation errors
            ViewData["ImpactCode"] = new SelectList(_context.tbl_aim_businessimpact, "ImpactCode", "ImpactName", updatedAppBusinessImpact.ImpactCode);
            ViewData["ImpactCreatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName", updatedAppBusinessImpact.ImpactCreatedBy);
            ViewData["RPortOslaCode"] = new SelectList(_context.tbl_aim_rportosla, "RPortOslaCode", "RPortOslaCode", updatedAppBusinessImpact.RPortOslaCode);
            return PartialView(updatedAppBusinessImpact);
        }


        // GET: AppBusinessImpacts/Delete/RPortOslaCode-ImpactCode
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Split the id parameter into parts
            var idParts = id.Split('-');
            if (idParts.Length != 2)
            {
                return NotFound();
            }

            string RPortOslaCode = idParts[0];
            if (!int.TryParse(idParts[1], out int ImpactCode))
            {
                return BadRequest("Invalid ImpactCode"); // Handle the case where the conversion fails.
            }

            var appBusinessImpact = await _context.tbl_aim_appbusimpact
                .Include(a => a.BusinessImpact)
                .Include(a => a.CreatedBy)
                .Include(a => a.RPortOsla)
                .FirstOrDefaultAsync(m => m.RPortOslaCode == RPortOslaCode && m.ImpactCode == ImpactCode);

            if (appBusinessImpact == null)
            {
                return NotFound();
            }

            return PartialView(appBusinessImpact);
        }

        // POST: AppBusinessImpacts/Delete/RPortOslaCode-ImpactCode
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Split the id parameter into parts
            var idParts = id.Split('-');
            if (idParts.Length != 2)
            {
                return NotFound();
            }

            string RPortOslaCode = idParts[0];
            if (!int.TryParse(idParts[1], out int ImpactCode))
            {
                return BadRequest("Invalid ImpactCode"); // Handle the case where the conversion fails.
            }

            var appBusinessImpact = await _context.tbl_aim_appbusimpact.FindAsync(RPortOslaCode, ImpactCode);
            if (appBusinessImpact != null)
            {
                _context.tbl_aim_appbusimpact.Remove(appBusinessImpact);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return NotFound();
        }

        private bool AppBusinessImpactExists(string id)
        {
          return (_context.tbl_aim_appbusimpact?.Any(e => e.RPortOslaCode == id)).GetValueOrDefault();
        }
    }
}
