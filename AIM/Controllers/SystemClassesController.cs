using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AIM.Data;
using AIM.Models;
using Microsoft.DiaSymReader;

namespace AIM.Controllers
{
    public class SystemClassesController : BaseController
    {
        private readonly AimDbContext _context;

        public SystemClassesController(AimDbContext context):base(context)
        {
            _context = context;
        }

        // GET: SystemClasses
        public async Task<IActionResult> Index()
        {
            var aimDbContext = _context.tbl_aim_systemclass.Include(s => s.CreatedBy).Include(s => s.Status).Include(s => s.UpdatedBy);
            return PartialView(await aimDbContext.ToListAsync());
        }

        // GET: SystemClasses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.tbl_aim_systemclass == null)
            {
                return NotFound();
            }

            var systemClass = await _context.tbl_aim_systemclass
                .Include(s => s.CreatedBy)
                .Include(s => s.Status)
                .Include(s => s.UpdatedBy)
                .FirstOrDefaultAsync(m => m.ClassCode == id);
            if (systemClass == null)
            {
                return NotFound();
            }

            return PartialView(systemClass);
        }

        // GET: SystemClasses/Create
        public IActionResult Create()
        {
            ViewData["ClassCreatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName");
            ViewData["ClassStatus"] = new SelectList(_context.tbl_aim_status, "StatusCode", "StatusName");
            ViewData["ClassUpdatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName");
            return PartialView();
        }

        // POST: SystemClasses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ClassCode,ClassName,ClassStatus,ClassCreatedBy,ClassCreatedDt,ClassUpdatedBy,ClassUpdatedDt")] SystemClass systemClass)
        {
            systemClass.ClassCreatedDt = DateTime.Now;
            if (ModelState.IsValid)
            {
                // Fetch the current class_code from tbl_aim_parameters
                var classCodeParameter = await _context.tbl_aim_parameters.FirstOrDefaultAsync(p => p.ParmCode == "class_code");
                if (classCodeParameter != null)
                {
                    // Increment the 'parm_value' in tbl_aim_parameters
                    classCodeParameter.ParmValue++;

                    // Assign the current class_code directly
                    systemClass.ClassCode = classCodeParameter.ParmValue;

                    systemClass.ClassCreatedBy = HttpContext.Session.GetString("UserName");

                    // Save changes to the context
                    await _context.SaveChangesAsync();

                    // Continue with adding the 'systemClass' object to the context
                    _context.Add(systemClass);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }

            ViewData["ClassCreatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName", systemClass.ClassCreatedBy);
            ViewData["ClassStatus"] = new SelectList(_context.tbl_aim_status, "StatusCode", "StatusName", systemClass.ClassStatus);
            ViewData["ClassUpdatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName", systemClass.ClassUpdatedBy);
            return PartialView(systemClass);
        }


        // GET: SystemClasses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.tbl_aim_systemclass == null)
            {
                return NotFound();
            }

            var systemClass = await _context.tbl_aim_systemclass.FindAsync(id);
            if (systemClass == null)
            {
                return NotFound();
            }
            ViewData["ClassCreatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName", systemClass.ClassCreatedBy);
            ViewData["ClassStatus"] = new SelectList(_context.tbl_aim_status, "StatusCode", "StatusName", systemClass.ClassStatus);
            ViewData["ClassUpdatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName", systemClass.ClassUpdatedBy);
            return PartialView(systemClass);
        }

        // POST: SystemClasses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ClassCode,ClassName,ClassStatus,ClassCreatedBy,ClassCreatedDt,ClassUpdatedBy,ClassUpdatedDt")] SystemClass systemClass)
        {
            if (id != systemClass.ClassCode)
            {
                return NotFound();
            }
            systemClass.ClassUpdatedDt = DateTime.Now;
            if (ModelState.IsValid)
            {
                try
                {
                    systemClass.ClassUpdatedBy = HttpContext.Session.GetString("UserName");
                    _context.Update(systemClass);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SystemClassExists(systemClass.ClassCode))
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
            ViewData["ClassCreatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName", systemClass.ClassCreatedBy);
            ViewData["ClassStatus"] = new SelectList(_context.tbl_aim_status, "StatusCode", "StatusName", systemClass.ClassStatus);
            ViewData["ClassUpdatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName", systemClass.ClassUpdatedBy);
            return PartialView(systemClass);
        }

        // GET: SystemClasses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.tbl_aim_systemclass == null)
            {
                return NotFound();
            }

            var systemClass = await _context.tbl_aim_systemclass
                .Include(s => s.CreatedBy)
                .Include(s => s.Status)
                .Include(s => s.UpdatedBy)
                .FirstOrDefaultAsync(m => m.ClassCode == id);
            if (systemClass == null)
            {
                return NotFound();
            }

            return PartialView(systemClass);
        }

        // POST: SystemClasses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.tbl_aim_systemclass == null)
            {
                return Problem("Entity set 'AimDbContext.tbl_aim_systemclass'  is null.");
            }
            var systemClass = await _context.tbl_aim_systemclass.FindAsync(id);
            if (systemClass != null)
            {
                _context.tbl_aim_systemclass.Remove(systemClass);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SystemClassExists(int id)
        {
          return (_context.tbl_aim_systemclass?.Any(e => e.ClassCode == id)).GetValueOrDefault();
        }
    }
}
