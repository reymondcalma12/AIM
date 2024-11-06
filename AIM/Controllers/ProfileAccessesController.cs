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
    public class ProfileAccessesController : BaseController
    {
        private readonly AimDbContext _context;

        public ProfileAccessesController(AimDbContext context):base(context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult SetProfileNameSession(string profileName)
        {
            HttpContext.Session.SetString("profileName", profileName);
            return Ok();
        }

        // GET: ProfileAccesses
        public async Task<IActionResult> Index(int? id)
        {
            // Retrieve the ProfileName from session
            string profileName = HttpContext.Session.GetString("profileName");
            // Pass the ProfileName to the view using ViewData
            ViewData["ProfileName"] = profileName;


            var aimDbContext = _context.tbl_aim_profileaccess.Include(p => p.CreatedBy).Include(p => p.Module).Include(p => p.Profile).Include(p => p.UpdatedBy);
            return View(await aimDbContext.ToListAsync());
        }


       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeAccess(int id, int id2)
        {
            var userrr = HttpContext.Session.GetString("UserName");

            if (_context.tbl_aim_profileaccess == null)
            {
                return Problem("Entity set 'LSM_PNContext.tbl_lsm_profileaccess'  is null.");
            }
            var profileAccess = await _context.tbl_aim_profileaccess.FindAsync(id, id2);
            if (profileAccess != null)
            {
                if (profileAccess.OpenAccess == "N")
                {
                    profileAccess.OpenAccess = "Y"; // Update to "Y" if it was previously "N"
                }
                else if (profileAccess.OpenAccess == "Y")
                {
                    profileAccess.OpenAccess = "N"; // Update to "N" if it was previously "Y" or any other value
                }

                profileAccess.UserUpdated = userrr;
                profileAccess.UserDtUpdated = DateTime.Now;

                _context.tbl_aim_profileaccess.Update(profileAccess);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index", new { id = id });

        }

        // GET: ProfileAccesses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.tbl_aim_profileaccess == null)
            {
                return NotFound();
            }

            var profileAccess = await _context.tbl_aim_profileaccess
                .Include(p => p.CreatedBy)
                .Include(p => p.Module)
                .Include(p => p.Profile)
                .Include(p => p.UpdatedBy)
                .FirstOrDefaultAsync(m => m.ProfileId == id);
            if (profileAccess == null)
            {
                return NotFound();
            }

            return View(profileAccess);
        }

        // GET: ProfileAccesses/Create
        public IActionResult Create()
        {
            ViewData["UserCreated"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserCode");
            ViewData["ModuleId"] = new SelectList(_context.tbl_aim_modules, "ModuleId", "ModuleCreated");
            ViewData["ProfileId"] = new SelectList(_context.tbl_aim_profiles, "ProfileId", "ProfileCreated");
            ViewData["UserUpdated"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserCode");
            return View();
        }

        // POST: ProfileAccesses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProfileId,ModuleId,OpenAccess,UserCreated,UserDtCreated,UserUpdated,UserDtUpdated")] ProfileAccess profileAccess)
        {
            if (ModelState.IsValid)
            {
                _context.Add(profileAccess);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserCreated"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserCode", profileAccess.UserCreated);
            ViewData["ModuleId"] = new SelectList(_context.tbl_aim_modules, "ModuleId", "ModuleCreated", profileAccess.ModuleId);
            ViewData["ProfileId"] = new SelectList(_context.tbl_aim_profiles, "ProfileId", "ProfileCreated", profileAccess.ProfileId);
            ViewData["UserUpdated"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserCode", profileAccess.UserUpdated);
            return View(profileAccess);
        }

        // GET: ProfileAccesses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.tbl_aim_profileaccess == null)
            {
                return NotFound();
            }

            var profileAccess = await _context.tbl_aim_profileaccess.FindAsync(id);
            if (profileAccess == null)
            {
                return NotFound();
            }
            ViewData["UserCreated"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserCode", profileAccess.UserCreated);
            ViewData["ModuleId"] = new SelectList(_context.tbl_aim_modules, "ModuleId", "ModuleCreated", profileAccess.ModuleId);
            ViewData["ProfileId"] = new SelectList(_context.tbl_aim_profiles, "ProfileId", "ProfileCreated", profileAccess.ProfileId);
            ViewData["UserUpdated"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserCode", profileAccess.UserUpdated);
            return View(profileAccess);
        }

        // POST: ProfileAccesses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProfileId,ModuleId,OpenAccess,UserCreated,UserDtCreated,UserUpdated,UserDtUpdated")] ProfileAccess profileAccess)
        {
            if (id != profileAccess.ProfileId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(profileAccess);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProfileAccessExists(profileAccess.ProfileId))
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
            ViewData["UserCreated"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserCode", profileAccess.UserCreated);
            ViewData["ModuleId"] = new SelectList(_context.tbl_aim_modules, "ModuleId", "ModuleCreated", profileAccess.ModuleId);
            ViewData["ProfileId"] = new SelectList(_context.tbl_aim_profiles, "ProfileId", "ProfileCreated", profileAccess.ProfileId);
            ViewData["UserUpdated"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserCode", profileAccess.UserUpdated);
            return View(profileAccess);
        }

        // GET: ProfileAccesses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.tbl_aim_profileaccess == null)
            {
                return NotFound();
            }

            var profileAccess = await _context.tbl_aim_profileaccess
                .Include(p => p.CreatedBy)
                .Include(p => p.Module)
                .Include(p => p.Profile)
                .Include(p => p.UpdatedBy)
                .FirstOrDefaultAsync(m => m.ProfileId == id);
            if (profileAccess == null)
            {
                return NotFound();
            }

            return View(profileAccess);
        }

        // POST: ProfileAccesses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.tbl_aim_profileaccess == null)
            {
                return Problem("Entity set 'AimDbContext.tbl_aim_profileaccess'  is null.");
            }
            var profileAccess = await _context.tbl_aim_profileaccess.FindAsync(id);
            if (profileAccess != null)
            {
                _context.tbl_aim_profileaccess.Remove(profileAccess);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProfileAccessExists(int id)
        {
          return (_context.tbl_aim_profileaccess?.Any(e => e.ProfileId == id)).GetValueOrDefault();
        }
    }
}
