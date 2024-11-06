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
    public class GroupsController : BaseController
    {
        private readonly AimDbContext _context;

        public GroupsController(AimDbContext context):base(context)
        {
            _context = context;
        }

        // GET: Groups
        public async Task<IActionResult> Index()
        {
            var aimDbContext = _context.tbl_aim_group.Include(u => u.CreatedBy).Include(s => s.Status).Include(u => u.UpdatedBy);
            return PartialView(await aimDbContext.ToListAsync());
        }

        // GET: Groups/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.tbl_aim_group == null)
            {
                return NotFound();
            }

            var @group = await _context.tbl_aim_group
                .Include(u => u.CreatedBy)
                .Include(u => u.Status)
                .Include(u => u.UpdatedBy)
                .FirstOrDefaultAsync(m => m.GroupCode == id);
            if (@group == null)
            {
                return NotFound();
            }

            return PartialView(@group);
        }

        // GET: Groups/Create
        public IActionResult Create()
        {
            ViewData["GroupCreatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName");
            ViewData["GroupStatus"] = new SelectList(_context.tbl_aim_status, "StatusCode", "StatusName");
            ViewData["GroupUpdatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName");
            return PartialView();
        }

        // POST: Groups/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GroupCode,GroupName,GroupStatus,GroupCreatedBy,GroupCreatedDt,GroupUpdatedBy,GroupUpdatedDt")] Group @group)
        {
            if (ModelState.IsValid)
            {
                // Fetch the current group_code from tbl_aim_parameters
                var groupCodeParameter = await _context.tbl_aim_parameters.FirstOrDefaultAsync(p => p.ParmCode == "group_no");
                if (groupCodeParameter != null && groupCodeParameter.ParmValue != null)
                {
                    // Assign the currentGroupCode directly
                    var currentGroupCode = groupCodeParameter.ParmValue;

                    // Increment the currentGroupCode by 1 to get the new group_code
                    var newGroupCode = currentGroupCode + 1;

                    // Update the group_code in the incoming 'group' object
                    @group.GroupCode = newGroupCode;

                    // Update the 'parm_value' in tbl_aim_parameters
                    groupCodeParameter.ParmValue = newGroupCode;

                    @group.GroupCreatedBy = HttpContext.Session.GetString("UserName");
                    @group.GroupCreatedDt = DateTime.Now;

                    // Save changes to the context
                    await _context.SaveChangesAsync();

                    // Continue with adding the 'group' object to the context
                    _context.Add(@group);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }

            ViewData["GroupCreatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserCode", @group.GroupCreatedBy);
            ViewData["GroupStatus"] = new SelectList(_context.tbl_aim_status, "StatusCode", "StatusCode", @group.GroupStatus);
            ViewData["GroupUpdatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserCode", @group.GroupUpdatedBy);
            return PartialView(@group);
        }

        // GET: Groups/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.tbl_aim_group == null)
            {
                return NotFound();
            }

            var @group = await _context.tbl_aim_group.FindAsync(id);
            if (@group == null)
            {
                return NotFound();
            }
            ViewData["GroupCreatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName", @group.GroupCreatedBy);
            ViewData["GroupStatus"] = new SelectList(_context.tbl_aim_status, "StatusCode", "StatusName", @group.GroupStatus);
            ViewData["GroupUpdatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName", @group.GroupUpdatedBy);
            return PartialView(@group);
        }

        // POST: Groups/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("GroupCode,GroupName,GroupStatus,GroupCreatedBy,GroupCreatedDt,GroupUpdatedBy,GroupUpdatedDt")] Group @group)
        {
            if (id != @group.GroupCode)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    @group.GroupCreatedBy = HttpContext.Session.GetString("UserName");
                    @group.GroupCreatedDt = DateTime.Now;
                    _context.Update(@group);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GroupExists(@group.GroupCode))
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
            ViewData["GroupCreatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullname", @group.GroupCreatedBy);
            ViewData["GroupStatus"] = new SelectList(_context.tbl_aim_status, "StatusCode", "StatusName", @group.GroupStatus);
            ViewData["GroupUpdatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName", @group.GroupUpdatedBy);
            return PartialView(@group);
        }

        // GET: Groups/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.tbl_aim_group == null)
            {
                return NotFound();
            }

            var @group = await _context.tbl_aim_group
                .Include(u => u.CreatedBy)
                .Include(u => u.Status)
                .Include(u => u.UpdatedBy)
                .FirstOrDefaultAsync(m => m.GroupCode == id);
            if (@group == null)
            {
                return NotFound();
            }

            return PartialView(@group);
        }

        // POST: Groups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.tbl_aim_group == null)
            {
                return Problem("Entity set 'AimDbContext.tbl_aim_group'  is null.");
            }
            var @group = await _context.tbl_aim_group.FindAsync(id);
            if (@group != null)
            {
                _context.tbl_aim_group.Remove(@group);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GroupExists(int id)
        {
          return (_context.tbl_aim_group?.Any(e => e.GroupCode == id)).GetValueOrDefault();
        }
    }
}
