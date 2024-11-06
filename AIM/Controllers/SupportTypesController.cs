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
    public class SupportTypesController : BaseController
    {
        private readonly AimDbContext _context;

        public SupportTypesController(AimDbContext context):base(context)
        {
            _context = context;
        }

        // GET: SupportTypes
        public async Task<IActionResult> Index()
        {
            var aimDbContext = _context.tbl_aim_supporttype.Include(s => s.CreatedBy).Include(s => s.Status).Include(s => s.UpdatedBy);
            return PartialView(await aimDbContext.ToListAsync());
        }

        // GET: SupportTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.tbl_aim_supporttype == null)
            {
                return NotFound();
            }

            var supportType = await _context.tbl_aim_supporttype
                .Include(s => s.CreatedBy)
                .Include(s => s.Status)
                .Include(s => s.UpdatedBy)
                .FirstOrDefaultAsync(m => m.SupportTypeCode == id);
            if (supportType == null)
            {
                return NotFound();
            }

            return PartialView(supportType);
        }

        // GET: SupportTypes/Create
        public IActionResult Create()
        {
            ViewData["SupportTypeCreatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName");
            ViewData["SupportTypeStatus"] = new SelectList(_context.tbl_aim_status, "StatusCode", "StatusName");
            ViewData["SupportTypeUpdatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName");
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SupportTypeCode,SupportTypeName,SupportTypeStatus,SupportTypeCreatedBy,SupportTypeCreatedDt,SupportTypeUpdatedBy,SupportTypeUpdatedDt")] SupportType supportType)
        {
            if (ModelState.IsValid)
            {
                // Fetch the current suptype_no from tbl_aim_parameters
                var suptypeNoParameter = await _context.tbl_aim_parameters.FirstOrDefaultAsync(p => p.ParmCode == "suptype_no");

                if (suptypeNoParameter != null && suptypeNoParameter.ParmValue != null)
                {
                    // Assign the currentSuptypeNo directly
                    var currentSuptypeNo = suptypeNoParameter.ParmValue + 1;

                    // Update the SupportTypeCode property of the incoming 'supportType' object
                    supportType.SupportTypeCode = currentSuptypeNo;
                    supportType.SupportTypeCreatedBy = HttpContext.Session.GetString("UserName");
                    supportType.SupportTypeCreatedDt = DateTime.Now;

                    // Increment the currentSuptypeNo by 1 for the next supportType
                    suptypeNoParameter.ParmValue = currentSuptypeNo;

                    _context.Add(supportType);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }

            // If ModelState is not valid or there was an issue with the parameter, return to the create view
            ViewData["SupportTypeCreatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName", supportType.SupportTypeCreatedBy);
            ViewData["SupportTypeStatus"] = new SelectList(_context.tbl_aim_status, "StatusCode", "StatusName", supportType.SupportTypeStatus);
            ViewData["SupportTypeUpdatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName", supportType.SupportTypeUpdatedBy);
            return PartialView(supportType);
        }


        // GET: SupportTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.tbl_aim_supporttype == null)
            {
                return NotFound();
            }

            var supportType = await _context.tbl_aim_supporttype.FindAsync(id);
            if (supportType == null)
            {
                return NotFound();
            }
            ViewData["SupportTypeCreatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName", supportType.SupportTypeCreatedBy);
            ViewData["SupportTypeStatus"] = new SelectList(_context.tbl_aim_status, "StatusCode", "StatusName", supportType.SupportTypeStatus);
            ViewData["SupportTypeUpdatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName", supportType.SupportTypeUpdatedBy);
            return PartialView(supportType);
        }

        // POST: SupportTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SupportTypeCode,SupportTypeName,SupportTypeStatus,SupportTypeCreatedBy,SupportTypeCreatedDt,SupportTypeUpdatedBy,SupportTypeUpdatedDt")] SupportType supportType)
        {
            if (id != supportType.SupportTypeCode)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    supportType.SupportTypeUpdatedBy = HttpContext.Session.GetString("UserName");
                    supportType.SupportTypeUpdatedDt = DateTime.Now;
                    _context.Update(supportType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SupportTypeExists(supportType.SupportTypeCode))
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
            ViewData["SupportTypeCreatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullname", supportType.SupportTypeCreatedBy);
            ViewData["SupportTypeStatus"] = new SelectList(_context.tbl_aim_status, "StatusCode", "StatusName", supportType.SupportTypeStatus);
            ViewData["SupportTypeUpdatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName", supportType.SupportTypeUpdatedBy);
            return PartialView(supportType);
        }

        // GET: SupportTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.tbl_aim_supporttype == null)
            {
                return NotFound();
            }

            var supportType = await _context.tbl_aim_supporttype
                .Include(s => s.CreatedBy)
                .Include(s => s.Status)
                .Include(s => s.UpdatedBy)
                .FirstOrDefaultAsync(m => m.SupportTypeCode == id);
            if (supportType == null)
            {
                return NotFound();
            }

            return PartialView(supportType);
        }

        // POST: SupportTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.tbl_aim_supporttype == null)
            {
                return Problem("Entity set 'AimDbContext.tbl_aim_supporttype'  is null.");
            }
            var supportType = await _context.tbl_aim_supporttype.FindAsync(id);
            if (supportType != null)
            {
                _context.tbl_aim_supporttype.Remove(supportType);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SupportTypeExists(int id)
        {
          return (_context.tbl_aim_supporttype?.Any(e => e.SupportTypeCode == id)).GetValueOrDefault();
        }
    }
}
