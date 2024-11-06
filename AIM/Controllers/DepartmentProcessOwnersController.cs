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
    public class DepartmentProcessOwnersController : BaseController
    {
        private readonly AimDbContext _context;

        public DepartmentProcessOwnersController(AimDbContext context) : base(context)
        {
            _context = context;
        }

        // GET: DepartmentProcessOwners
        public async Task<IActionResult> Index()
        {
            var aimDbContext = _context.tbl_aim_deptprocessowner.Include(d => d.CreatedBy).Include(d => d.Department).Include(d => d.RPortOsla);
            return PartialView(await aimDbContext.ToListAsync());
        }

        // GET: DepartmentProcessOwners/Details/RPortOslaCode-DepartmentCode
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
            string DepartmentCode = idParts[1];
            if (!int.TryParse(idParts[1], out int parsedDepartmentCode))
            {
                return BadRequest("Invalid DepartmentCode"); // Handle the case where the conversion fails.
            }

            var departmentProcessOwner = await _context.tbl_aim_deptprocessowner
                .Include(d => d.CreatedBy)
                .Include(d => d.Department)
                .Include(d => d.RPortOsla)
                .FirstOrDefaultAsync(m => m.RPortOslaCode == RPortOslaCode && m.DeptCode == parsedDepartmentCode);

            if (departmentProcessOwner == null)
            {
                return NotFound();
            }

            return PartialView(departmentProcessOwner);
        }


        // GET: DepartmentProcessOwners/Create
        public IActionResult Create()
        {
            ViewData["DeptProcessOwnerCreatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName");
            ViewData["DeptCode"] = new SelectList(_context.tbl_aim_department, "DeptCode", "DeptName");
            ViewData["RPortOslaCode"] = new SelectList(_context.tbl_aim_rportosla, "RPortOslaCode", "RPortOslaCode");
            return PartialView();
        }

        // POST: DepartmentProcessOwners/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RPortOslaCode,DeptCode,DeptProcessOwnerCreatedBy,DeptProcessOwnerCreatedDt")] DepartmentProcessOwner departmentProcessOwner)
        {
            if (ModelState.IsValid)
            {
                _context.Add(departmentProcessOwner);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DeptProcessOwnerCreatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserCode", departmentProcessOwner.DeptProcessOwnerCreatedBy);
            ViewData["DeptCode"] = new SelectList(_context.tbl_aim_department, "DeptCode", "DeptCode", departmentProcessOwner.DeptCode);
            ViewData["RPortOslaCode"] = new SelectList(_context.tbl_aim_rportosla, "RPortOslaCode", "RPortOslaCode", departmentProcessOwner.RPortOslaCode);
            return PartialView(departmentProcessOwner);
        }

        // GET: DepartmentProcessOwners/Edit/RPortOslaCode-DepartmentCode
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.tbl_aim_deptprocessowner == null)
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
            if (!int.TryParse(idParts[1], out int parsedDepartmentCode))
            {
                return BadRequest("Invalid DepartmentCode"); // Handle the case where the conversion fails.
            }

            var departmentProcessOwner = await _context.tbl_aim_deptprocessowner.FindAsync(RPortOslaCode, parsedDepartmentCode);
            if (departmentProcessOwner == null)
            {
                return NotFound();
            }

            ViewData["DeptProcessOwnerCreatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName", departmentProcessOwner.DeptProcessOwnerCreatedBy);
            ViewData["DeptCode"] = new SelectList(_context.tbl_aim_department, "DeptCode", "DeptName", departmentProcessOwner.DeptCode);
            ViewData["RPortOslaCode"] = new SelectList(_context.tbl_aim_rportosla, "RPortOslaCode", "RPortOslaCode", departmentProcessOwner.RPortOslaCode);
            return PartialView(departmentProcessOwner);
        }


        // POST: DepartmentProcessOwners/Edit/RPortOslaCode-DepartmentCode
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("RPortOslaCode,DeptCode,DeptProcessOwnerCreatedBy,DeptProcessOwnerCreatedDt")] DepartmentProcessOwner updatedDepartmentProcessOwner)
        {
            // Split the id parameter into parts
            var idParts = id.Split('-');
            if (idParts.Length != 2)
            {
                return NotFound();
            }

            string RPortOslaCode = idParts[0];
            if (!int.TryParse(idParts[1], out int parsedDepartmentCode))
            {
                return BadRequest("Invalid DepartmentCode"); // Handle the case where the conversion fails.
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Find the existing record with the composite key
                    var existingDepartmentProcessOwner = await _context.tbl_aim_deptprocessowner
                        .Where(d => d.RPortOslaCode == RPortOslaCode && d.DeptCode == parsedDepartmentCode)
                        .FirstOrDefaultAsync();

                    if (existingDepartmentProcessOwner == null)
                    {
                        // If not found, create a new entity with the specified key values
                        var newDepartmentProcessOwner = new DepartmentProcessOwner
                        {
                            RPortOslaCode = RPortOslaCode,
                            DeptCode = parsedDepartmentCode,
                            DeptProcessOwnerCreatedBy = updatedDepartmentProcessOwner.DeptProcessOwnerCreatedBy,
                            DeptProcessOwnerCreatedDt = updatedDepartmentProcessOwner.DeptProcessOwnerCreatedDt
                            // Add other properties as needed
                        };

                        _context.tbl_aim_deptprocessowner.Add(newDepartmentProcessOwner);
                        await _context.SaveChangesAsync();

                        // Return a JSON response indicating success
                        return Json(new { success = true });
                    }
                    else
                    {
                        // If found, update the existing entity with the new data
                        existingDepartmentProcessOwner.DeptProcessOwnerCreatedBy = updatedDepartmentProcessOwner.DeptProcessOwnerCreatedBy;
                        existingDepartmentProcessOwner.DeptProcessOwnerCreatedDt = updatedDepartmentProcessOwner.DeptProcessOwnerCreatedDt;
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


        // GET: DepartmentProcessOwners/Delete/RPortOslaCode-DepartmentCode
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
            if (!int.TryParse(idParts[1], out int parsedDepartmentCode))
            {
                return BadRequest("Invalid DepartmentCode"); // Handle the case where the conversion fails.
            }

            var departmentProcessOwner = await _context.tbl_aim_deptprocessowner
                .Include(d => d.CreatedBy)
                .Include(d => d.Department)
                .Include(d => d.RPortOsla)
                .FirstOrDefaultAsync(m => m.RPortOslaCode == RPortOslaCode && m.DeptCode == parsedDepartmentCode);

            if (departmentProcessOwner == null)
            {
                return NotFound();
            }

            return PartialView(departmentProcessOwner);
        }

        // POST: DepartmentProcessOwners/Delete/RPortOslaCode-DepartmentCode
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
            if (!int.TryParse(idParts[1], out int parsedDepartmentCode))
            {
                return BadRequest("Invalid DepartmentCode"); // Handle the case where the conversion fails.
            }

            if (_context.tbl_aim_deptprocessowner == null)
            {
                return Problem("Entity set 'AimDbContext.tbl_aim_deptprocessowner' is null.");
            }

            var departmentProcessOwner = await _context.tbl_aim_deptprocessowner.FindAsync(RPortOslaCode, parsedDepartmentCode);
            if (departmentProcessOwner != null)
            {
                _context.tbl_aim_deptprocessowner.Remove(departmentProcessOwner);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DepartmentProcessOwnerExists(string id)
        {
          return (_context.tbl_aim_deptprocessowner?.Any(e => e.RPortOslaCode == id)).GetValueOrDefault();
        }
    }
}
