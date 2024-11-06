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
    public class AuthMethodsController : BaseController
    {
        private readonly AimDbContext _context;

        public AuthMethodsController(AimDbContext context):base(context)
        {
            _context = context;
        }

        // GET: AuthMethods
        public async Task<IActionResult> Index()
        {
            var aimDbContext = _context.tbl_aim_authmethod.Include(a => a.CreatedBy).Include(a => a.Status).Include(a => a.UpdatedBy);
            return PartialView(await aimDbContext.ToListAsync());
        }

        // GET: AuthMethods/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.tbl_aim_authmethod == null)
            {
                return NotFound();
            }

            var authMethod = await _context.tbl_aim_authmethod
                .Include(a => a.CreatedBy)
                .Include(a => a.Status)
                .Include(a => a.UpdatedBy)
                .FirstOrDefaultAsync(m => m.AuthCode == id);
            if (authMethod == null)
            {
                return NotFound();
            }

            return PartialView(authMethod);
        }

        // GET: AuthMethods/Create
        public IActionResult Create()
        {
            ViewData["AuthCreatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName");
            ViewData["AuthStatus"] = new SelectList(_context.tbl_aim_status, "StatusCode", "StatusName");
            ViewData["AuthUpdatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName");
            return PartialView();
        }

        // POST: AuthMethods/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AuthCode,AuthName,AuthStatus,AuthCreatedBy,AuthCreatedDt,AuthUpdatedBy,AuthUpdatedDt")] AuthMethod authMethod)
        {
            authMethod.AuthCreatedDt = DateTime.Now;

            if (ModelState.IsValid)
            {
                // Fetch the current auth_no from tbl_aim_parameters
                var authNoParameter = await _context.tbl_aim_parameters.FirstOrDefaultAsync(p => p.ParmCode == "auth_no");
                if (authNoParameter != null)
                {
                    int currentAuthNo = authNoParameter.ParmValue;
                    // Increment the current auth_no by 1 to get the new auth_no
                    var newAuthNo = currentAuthNo + 1;

                    // Update the auth_no in the incoming 'authMethod' object
                    authMethod.AuthCode = newAuthNo;

                    // Update the 'parm_value' in tbl_aim_parameters
                    authNoParameter.ParmValue = newAuthNo;

                    authMethod.AuthCreatedBy = HttpContext.Session.GetString("UserName");
                    // Save changes to the context
                    await _context.SaveChangesAsync();

                    // Continue with adding the 'authMethod' object to the context
                    _context.Add(authMethod);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }

            ViewData["AuthCreatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserCode", authMethod.AuthCreatedBy);
            ViewData["AuthStatus"] = new SelectList(_context.tbl_aim_status, "StatusCode", "StatusCode", authMethod.AuthStatus);
            ViewData["AuthUpdatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserCode", authMethod.AuthUpdatedBy);
            return PartialView(authMethod);
        }


        // GET: AuthMethods/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.tbl_aim_authmethod == null)
            {
                return NotFound();
            }

            var authMethod = await _context.tbl_aim_authmethod.FindAsync(id);
            if (authMethod == null)
            {
                return NotFound();
            }
            ViewData["AuthCreatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName", authMethod.AuthCreatedBy);
            ViewData["AuthStatus"] = new SelectList(_context.tbl_aim_status, "StatusCode", "StatusName", authMethod.AuthStatus);
            ViewData["AuthUpdatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName", authMethod.AuthUpdatedBy);
            return PartialView(authMethod);
        }

        // POST: AuthMethods/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AuthCode,AuthName,AuthStatus,AuthCreatedBy,AuthCreatedDt,AuthUpdatedBy,AuthUpdatedDt")] AuthMethod authMethod)
        {
            if (id != authMethod.AuthCode)
            {
                return NotFound();
            }
            authMethod.AuthUpdatedDt = DateTime.Now; 
            if (ModelState.IsValid)
            {
                try
                {
                    authMethod.AuthUpdatedBy = HttpContext.Session.GetString("UserName");
                    _context.Update(authMethod);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AuthMethodExists(authMethod.AuthCode))
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
            ViewData["AuthCreatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName", authMethod.AuthCreatedBy);
            ViewData["AuthStatus"] = new SelectList(_context.tbl_aim_status, "StatusCode", "StatusName", authMethod.AuthStatus);
            ViewData["AuthUpdatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName", authMethod.AuthUpdatedBy);
            return PartialView(authMethod);
        }

        // GET: AuthMethods/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.tbl_aim_authmethod == null)
            {
                return NotFound();
            }

            var authMethod = await _context.tbl_aim_authmethod
                .Include(a => a.CreatedBy)
                .Include(a => a.Status)
                .Include(a => a.UpdatedBy)
                .FirstOrDefaultAsync(m => m.AuthCode == id);
            if (authMethod == null)
            {
                return NotFound();
            }

            return PartialView(authMethod);
        }

        // POST: AuthMethods/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.tbl_aim_authmethod == null)
            {
                return Problem("Entity set 'AimDbContext.tbl_aim_authmethod'  is null.");
            }
            var authMethod = await _context.tbl_aim_authmethod.FindAsync(id);
            if (authMethod != null)
            {
                _context.tbl_aim_authmethod.Remove(authMethod);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AuthMethodExists(int id)
        {
          return (_context.tbl_aim_authmethod?.Any(e => e.AuthCode == id)).GetValueOrDefault();
        }
    }
}
