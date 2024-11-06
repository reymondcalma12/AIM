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
    public class DepartmentsController : BaseController
    {
        private readonly AimDbContext _context;

        public DepartmentsController(AimDbContext context) : base(context)
        {
            _context = context;
        }

        // GET: Departments
        public async Task<IActionResult> Index()
        {
            var aimDbContext = _context.tbl_aim_department.Include(d => d.CreatedBy).Include(d => d.Status).Include(d => d.UpdatedBy);
            return PartialView(await aimDbContext.ToListAsync());
        }

        // GET: Departments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.tbl_aim_department == null)
            {
                return NotFound();
            }

            var department = await _context.tbl_aim_department
                .Include(d => d.CreatedBy)
                .Include(d => d.Status)
                .Include(d => d.UpdatedBy)
                .FirstOrDefaultAsync(m => m.DeptCode == id);
            if (department == null)
            {
                return NotFound();
            }

            return PartialView(department);
        }

        // GET: Departments/Create
        public IActionResult Create()
        {
            ViewData["DeptCreatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName");
            ViewData["DeptStatus"] = new SelectList(_context.tbl_aim_status, "StatusCode", "StatusName");
            ViewData["DeptUpdatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName");
            return PartialView();
        }

        // POST: Departments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DeptCode,DeptName,DeptStatus,DeptCreatedBy,DeptCreatedDt,DeptUpdatedBy,DeptUpdatedDt")] Department department)
        {
            if (ModelState.IsValid)
            {
                // Fetch the current dept_code from tbl_aim_parameters
                var deptCodeParameter = await _context.tbl_aim_parameters.FirstOrDefaultAsync(p => p.ParmCode == "dept_code");
                if (deptCodeParameter != null && deptCodeParameter.ParmValue != null)
                {
                    // Assign the current dept_code directly
                    var currentDeptCode = deptCodeParameter.ParmValue;

                    // Update the dept_code in the incoming 'department' object
                    department.DeptCode = currentDeptCode + 1;

                    // Increment the current dept_code by 1 for the next department
                    deptCodeParameter.ParmValue = currentDeptCode + 1;
                    department.DeptCreatedBy = HttpContext.Session.GetString("UserName");
                    _context.Add(department);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }

            ViewData["DeptCreatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserCode", department.DeptCreatedBy);
            ViewData["DeptStatus"] = new SelectList(_context.tbl_aim_status, "StatusCode", "StatusCode", department.DeptStatus);
            ViewData["DeptUpdatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserCode", department.DeptUpdatedBy);
            return View(department);
        }

        // GET: Departments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.tbl_aim_department == null)
            {
                return NotFound();
            }

            var department = await _context.tbl_aim_department.FindAsync(id);
            if (department == null)
            {
                return NotFound();
            }
            ViewData["DeptCreatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserCode", department.DeptCreatedBy);
            ViewData["DeptStatus"] = new SelectList(_context.tbl_aim_status, "StatusCode", "StatusCode", department.DeptStatus);
            ViewData["DeptUpdatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserCode", department.DeptUpdatedBy);
            return PartialView(department);
        }

        // POST: Departments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DeptCode,DeptName,DeptStatus,DeptCreatedBy,DeptCreatedDt,DeptUpdatedBy,DeptUpdatedDt")] Department department)
        {
            if (id != department.DeptCode)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    department.DeptUpdatedBy = HttpContext.Session.GetString("UserName");
                    department.DeptUpdatedDt = DateTime.Now;
                    _context.Update(department);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DepartmentExists(department.DeptCode))
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
            ViewData["DeptCreatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserCode", department.DeptCreatedBy);
            ViewData["DeptStatus"] = new SelectList(_context.tbl_aim_status, "StatusCode", "StatusCode", department.DeptStatus);
            ViewData["DeptUpdatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserCode", department.DeptUpdatedBy);
            return View(department);
        }

        // GET: Departments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.tbl_aim_department == null)
            {
                return NotFound();
            }

            var department = await _context.tbl_aim_department
                .Include(d => d.CreatedBy)
                .Include(d => d.Status)
                .Include(d => d.UpdatedBy)
                .FirstOrDefaultAsync(m => m.DeptCode == id);
            if (department == null)
            {
                return NotFound();
            }

            return PartialView(department);
        }

        // POST: Departments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.tbl_aim_department == null)
            {
                return Problem("Entity set 'AimDbContext.tbl_aim_department'  is null.");
            }
            var department = await _context.tbl_aim_department.FindAsync(id);
            if (department != null)
            {
                _context.tbl_aim_department.Remove(department);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DepartmentExists(int id)
        {
          return (_context.tbl_aim_department?.Any(e => e.DeptCode == id)).GetValueOrDefault();
        }
    }
}
