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
    public class ProfilesController : BaseController
    {
        private readonly AimDbContext _context;

        public ProfilesController(AimDbContext context):base(context)
        {
            _context = context;
        }

        // GET: Profiles
        public async Task<IActionResult> Index()
        {
            await PopulateViewBag();
            var aimDbContext = _context.tbl_aim_profiles.Include(p => p.CreatedBy).Include(p => p.Status).Include(p => p.UpdatedBy);
            return PartialView("Index",await aimDbContext.ToListAsync());
        }

        // GET: Profiles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.tbl_aim_profiles == null)
            {
                return NotFound();
            }

            var profile = await _context.tbl_aim_profiles
                .Include(p => p.CreatedBy)
                .Include(p => p.Status)
                .Include(p => p.UpdatedBy)
                .FirstOrDefaultAsync(m => m.ProfileId == id);
            if (profile == null)
            {
                return NotFound();
            }

            return PartialView(profile);
        }

        // GET: Profiles/Create
        public IActionResult Create()
        {
            ViewData["ProfileCreated"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName");
            ViewData["ProfileStatus"] = new SelectList(_context.tbl_aim_status, "StatusCode", "StatusName");
            ViewData["ProfileUpdated"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName");
            return PartialView();
        }

        // POST: Profiles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProfileId,ProfileName,ProfileDescription,ProfileStatus,ProfileCreated,ProfileDtCreated,ProfileUpdated,ProfileDtUpdated")] Profile profile)
        {
            if (ModelState.IsValid)
            {
                // Fetch the current profile_no from tbl_aim_parameters
                var profileNoParameter = await _context.tbl_aim_parameters.FirstOrDefaultAsync(p => p.ParmCode == "profile_no");
                if (profileNoParameter != null && profileNoParameter.ParmValue != null)
                {
                    // Assign the currentProfileNo directly
                    var currentProfileNo = profileNoParameter.ParmValue + 1;

                    // Update the profile_no in the incoming 'profile' object
                    profile.ProfileId = currentProfileNo;
                    profile.ProfileCreated = HttpContext.Session.GetString("UserName");

                    // Increment the currentProfileNo by 1 for the next profile
                    profileNoParameter.ParmValue = currentProfileNo;

                    _context.Add(profile);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }

            ViewData["ProfileCreated"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserCode", profile.ProfileCreated);
            ViewData["ProfileStatus"] = new SelectList(_context.tbl_aim_status, "StatusCode", "StatusCode", profile.ProfileStatus);
            ViewData["ProfileUpdated"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserCode", profile.ProfileUpdated);
            return View(profile);
        }


        // GET: Profiles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.tbl_aim_profiles == null)
            {
                return NotFound();
            }

            var profile = await _context.tbl_aim_profiles.FindAsync(id);
            if (profile == null)
            {
                return NotFound();
            }
            ViewData["ProfileCreated"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserCode", profile.ProfileCreated);
            ViewData["ProfileStatus"] = new SelectList(_context.tbl_aim_status, "StatusCode", "StatusCode", profile.ProfileStatus);
            ViewData["ProfileUpdated"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserCode", profile.ProfileUpdated);
            return PartialView(profile);
        }

        // POST: Profiles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProfileId,ProfileName,ProfileDescription,ProfileStatus,ProfileCreated,ProfileDtCreated,ProfileUpdated,ProfileDtUpdated")] Profile updatedProfile)
        {
            if (id != updatedProfile.ProfileId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Retrieve the existing entity with the same key
                    var existingProfile = await _context.tbl_aim_profiles.FindAsync(id);

                    if (existingProfile == null)
                    {
                        return NotFound();
                    }

                    // Update the properties of the existing entity
                    existingProfile.ProfileName = updatedProfile.ProfileName;
                    existingProfile.ProfileDescription = updatedProfile.ProfileDescription;
                    existingProfile.ProfileStatus = updatedProfile.ProfileStatus;
                    existingProfile.ProfileCreated = HttpContext.Session.GetString("UserName");
                    existingProfile.ProfileDtCreated = updatedProfile.ProfileDtCreated;
                    existingProfile.ProfileUpdated = updatedProfile.ProfileUpdated;
                    existingProfile.ProfileDtUpdated = DateTime.Now ;

                    _context.Update(existingProfile);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProfileExists(updatedProfile.ProfileId))
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

            // Repopulate the ViewData for dropdowns (if needed)
            ViewData["ProfileCreated"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserCode", updatedProfile.ProfileCreated);
            ViewData["ProfileStatus"] = new SelectList(_context.tbl_aim_status, "StatusCode", "StatusCode", updatedProfile.ProfileStatus);
            ViewData["ProfileUpdated"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserCode", updatedProfile.ProfileUpdated);
            return View(updatedProfile);
        }

        // GET: Profiles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.tbl_aim_profiles == null)
            {
                return NotFound();
            }

            var profile = await _context.tbl_aim_profiles
                .Include(p => p.CreatedBy)
                .Include(p => p.Status)
                .Include(p => p.UpdatedBy)
                .FirstOrDefaultAsync(m => m.ProfileId == id);
            if (profile == null)
            {
                return NotFound();
            }

            return PartialView(profile);
        }

        // POST: Profiles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.tbl_aim_profiles == null)
            {
                return Problem("Entity set 'AimDbContext.tbl_aim_profiles'  is null.");
            }
            var profile = await _context.tbl_aim_profiles.FindAsync(id);
            if (profile != null)
            {
                _context.tbl_aim_profiles.Remove(profile);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProfileExists(int id)
        {
          return (_context.tbl_aim_profiles?.Any(e => e.ProfileId == id)).GetValueOrDefault();
        }
    }
}
