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
    public class Level3OwnerController : BaseController
    {
        private readonly AimDbContext _context;

        public Level3OwnerController(AimDbContext context) : base(context)
        {
            _context = context;
        }

        // GET: Level3Owner
        public async Task<IActionResult> Index()
        {
            var aimDbContext = _context.tbl_aim_level3owner.Include(l => l.CreatedBy).Include(l => l.Level3).Include(l => l.RPortOsla);
            return PartialView(await aimDbContext.ToListAsync());
        }

        // GET: Level3Owner/Details/RPortOslaCode-Level3Code
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.tbl_aim_level3owner == null)
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
            if (!int.TryParse(idParts[1], out int Level3Code))
            {
                return BadRequest("Invalid Level3Code"); // Handle the case where the conversion fails.
            }


            var level3Owner = await _context.tbl_aim_level3owner
                .Include(l => l.CreatedBy)
                .Include(l => l.Level3)
                .Include(l => l.RPortOsla)
                .FirstOrDefaultAsync(m => m.RPortOslaCode == RPortOslaCode && m.Level3Code == Level3Code);

            if (level3Owner == null)
            {
                return NotFound();
            }

            return PartialView(level3Owner);
        }


        // GET: Level3Owner/Create
        public IActionResult Create()
        {
            ViewData["Level3OwnerCreatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName");
            ViewData["Level3Code"] = new SelectList(_context.tbl_aim_level3, "Level3Code", "Level3Name");
            ViewData["RPortOslaCode"] = new SelectList(_context.tbl_aim_rportosla, "RPortOslaCode", "RPortOslaCode");
            return PartialView();
        }

        // POST: Level3Owner/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RPortOslaCode,Level3Code,Level3OwnerCreatedBy,Level3OwnerCreatedDt")] Level3Owner level3Owner)
        {
            if (ModelState.IsValid)
            {
                _context.Add(level3Owner);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Level3OwnerCreatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserCode", level3Owner.Level3OwnerCreatedBy);
            ViewData["Level3Code"] = new SelectList(_context.tbl_aim_level3, "Level3Code", "Level3Code", level3Owner.Level3Code);
            ViewData["RPortOslaCode"] = new SelectList(_context.tbl_aim_rportosla, "RPortOslaCode", "RPortOslaCode", level3Owner.RPortOslaCode);
            return View(level3Owner);
        }

        // GET: Level3Owner/Edit/RPortOslaCode-Level3Code
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.tbl_aim_level3owner == null)
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
            if (!int.TryParse(idParts[1], out int parsedLevel3Code))
            {
                return BadRequest("Invalid Level3Code"); // Handle the case where the conversion fails.
            }

            var level3Owner = await _context.tbl_aim_level3owner
                .Include(l => l.CreatedBy)
                .Include(l => l.Level3)
                .Include(l => l.RPortOsla)
                .FirstOrDefaultAsync(m => m.RPortOslaCode == RPortOslaCode && m.Level3Code == parsedLevel3Code);

            if (level3Owner == null)
            {
                return NotFound();
            }

            ViewData["Level3OwnerCreatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserCode", level3Owner.Level3OwnerCreatedBy);
            ViewData["Level3Code"] = new SelectList(_context.tbl_aim_level3, "Level3Code", "Level3Code", level3Owner.Level3Code);
            ViewData["RPortOslaCode"] = new SelectList(_context.tbl_aim_rportosla, "RPortOslaCode", "RPortOslaCode", level3Owner.RPortOslaCode);
            return PartialView(level3Owner);
        }


        // POST: Level3Owner/Edit/RPortOslaCode-Level3Code
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("RPortOslaCode,Level3Code,Level3OwnerCreatedBy,Level3OwnerCreatedDt")] Level3Owner updatedLevel3Owner)
        {
            // Split the id parameter into parts
            var idParts = id.Split('-');
            if (idParts.Length != 2)
            {
                return NotFound();
            }

            string RPortOslaCode = idParts[0];
            if (!int.TryParse(idParts[1], out int parsedLevel3Code))
            {
                return BadRequest("Invalid Level3Code"); // Handle the case where the conversion fails.
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Find the existing record with the composite key
                    var existingLevel3Owner = await _context.tbl_aim_level3owner
                        .Where(l => l.RPortOslaCode == RPortOslaCode && l.Level3Code == parsedLevel3Code)
                        .FirstOrDefaultAsync();

                    if (existingLevel3Owner == null)
                    {
                        // If not found, create a new entity with the specified key values
                        var newLevel3Owner = new Level3Owner
                        {
                            RPortOslaCode = RPortOslaCode,
                            Level3Code = parsedLevel3Code,
                            Level3OwnerCreatedBy = updatedLevel3Owner.Level3OwnerCreatedBy,
                            Level3OwnerCreatedDt = updatedLevel3Owner.Level3OwnerCreatedDt
                            // Add other properties as needed
                        };

                        _context.tbl_aim_level3owner.Add(newLevel3Owner);
                        await _context.SaveChangesAsync();

                        // Return a JSON response indicating success
                        return Json(new { success = true });
                    }
                    else
                    {
                        // If found, update the existing entity with the new data
                        existingLevel3Owner.Level3OwnerCreatedBy = updatedLevel3Owner.Level3OwnerCreatedBy;
                        existingLevel3Owner.Level3OwnerCreatedDt = updatedLevel3Owner.Level3OwnerCreatedDt;
                        // Update other properties as needed

                        await _context.SaveChangesAsync();

                        // Return a JSON response indicating success
                        return Json(new { success = true });
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


        // GET: Level3Owner/Delete/RPortOslaCode-Level3Code
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
            if (!int.TryParse(idParts[1], out int parsedLevel3Code))
            {
                return BadRequest("Invalid Level3Code"); // Handle the case where the conversion fails.
            }

            var level3Owner = await _context.tbl_aim_level3owner
                .Include(l => l.CreatedBy)
                .Include(l => l.Level3)
                .Include(l => l.RPortOsla)
                .FirstOrDefaultAsync(m => m.RPortOslaCode == RPortOslaCode && m.Level3Code == parsedLevel3Code);

            if (level3Owner == null)
            {
                return NotFound();
            }

            return PartialView(level3Owner);
        }

        // POST: Level3Owner/Delete/RPortOslaCode-Level3Code
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            // Split the id parameter into parts
            var idParts = id.Split('-');
            if (idParts.Length != 2)
            {
                return NotFound();
            }

            string RPortOslaCode = idParts[0];
            if (!int.TryParse(idParts[1], out int parsedLevel3Code))
            {
                return BadRequest("Invalid Level3Code"); // Handle the case where the conversion fails.
            }

            if (_context.tbl_aim_level3owner == null)
            {
                return Problem("Entity set 'AimDbContext.tbl_aim_level3owner' is null.");
            }

            var level3Owner = await _context.tbl_aim_level3owner.FindAsync(RPortOslaCode, parsedLevel3Code);
            if (level3Owner != null)
            {
                _context.tbl_aim_level3owner.Remove(level3Owner);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool Level3OwnerExists(string id)
        {
          return (_context.tbl_aim_level3owner?.Any(e => e.RPortOslaCode == id)).GetValueOrDefault();
        }
    }
}
