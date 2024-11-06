using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AIM.Data;
using AIM.Models;
using LSM_PN;
using Microsoft.Extensions.Options;
using System.DirectoryServices.AccountManagement;

namespace AIM.Controllers
{
    public class UsersController : BaseController
    {
        private readonly AimDbContext _context;
        private readonly LDAPSettings _ldapSettings;


        public UsersController(AimDbContext context, IOptions<LDAPSettings> ldapSettings) :base(context)
        {
            _context = context;
            _ldapSettings = ldapSettings.Value;
        }


		public IActionResult Login()
		{
			return View();
		}


        [HttpPost]
        public async Task<IActionResult> Login(string username, string password, bool useADAuthentication = false)
        {

            if (ModelState.IsValid)
            {
                if (useADAuthentication)
                {
                    using (var context = new PrincipalContext(ContextType.Domain, null, _ldapSettings.Path))
                    {
                        // Validate the credentials
                        try
                        {

                            var adEmail = username + "@princeretail.com";
                            if (context.ValidateCredentials(adEmail, password))
                            {


                                var user = await _context.tbl_aim_users.FirstOrDefaultAsync(u => u.UserADLogin == username);
                                if (user != null)
                                {
                                    HttpContext.Session.SetString("UserName", user.UserCode);
                                    HttpContext.Session.SetString("UserPass", user.UserPass);
                                    HttpContext.Session.SetString("FullName", user.UserFullName);
                                    HttpContext.Session.SetString("ProfileId", user.UserProfile.ToString());
                                    //HttpContext.Session.SetString("ProfileName", user.Profile.ProfileName);

                                    return Json(new { success = true, message = "Username and password matched." });
                                }
                                else
                                {
                                    return Json(new { success = false, message = $"User {username} is not registered to the System" });

                                }
                            }
                            else
                            {
                                return Json(new { success = false, message = "Incorrect password." });
                            }
                        }
                        catch (Exception e)
                        {
                            var errorMessage = $"LDAP authentication error: {e.Message}";
                            return Json(new { success = false, message = errorMessage });
                        }
                    }

                }
                else
                {
                    /// Find the user by username
                    var user = await _context.tbl_aim_users.FirstOrDefaultAsync(u => u.UserCode == username);

                    if (user != null)
                    {

                        
                        var profileName = await _context.tbl_aim_profiles.FirstOrDefaultAsync(p => p.ProfileId == user.UserProfile);
                        if (user != null)
                        {

                            // Check if the user code exists in the database
                            if (user.UserCode == username)
                            {
                                // Authenticate the user
                                var isPasswordCorrect = BCrypt.Net.BCrypt.Verify(password, user.UserPass);

                                if (isPasswordCorrect)
                                {
                                    HttpContext.Session.SetString("UserName", user.UserCode);
                                    HttpContext.Session.SetString("UserPass", user.UserPass);
                                    HttpContext.Session.SetString("FullName", user.UserFullName);
                                    HttpContext.Session.SetString("ProfileId", user.UserProfile.ToString());
                                    HttpContext.Session.SetString("ProfileName", user.Profile.ProfileName);


                                    return Json(new { success = true, message = "Username and password matched." });
                                }
                                else
                                {
                                    return Json(new { success = false, message = "Incorrect password." });
                                }
                            }
                        }
                    }
                }
            }

            return Json(new { success = false, message = "User does not exist." });
        }


        //For clearing the session stored
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction(nameof(Login));
        }


        // GET: Users
        public async Task<IActionResult> Index()
        {
            await PopulateViewBag();
            var aimDbContext = _context.tbl_aim_users.Include(u => u.Profile).Include(u => u.Status);
            return PartialView("Index",await aimDbContext.ToListAsync());
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.tbl_aim_users == null)
            {
                return NotFound();
            }

            var user = await _context.tbl_aim_users
                .Include(u => u.Profile)
                .Include(u => u.Status)
                .FirstOrDefaultAsync(m => m.UserCode == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            ViewData["UserProfile"] = new SelectList(_context.tbl_aim_profiles, "ProfileId", "ProfileName");
            ViewData["UserStatus"] = new SelectList(_context.tbl_aim_status, "StatusCode", "StatusCode");
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserCode,UserPass,UserADLogin,UserFullName,UserProfile,UserStatus,UserCreated,UserDtCreated,UserUpdated,UserDtUpdated")] User user)
        {
            var usercreator = HttpContext.Session.GetString("FullName");

            if (ModelState.IsValid)
            {

                // Hash the password using bcrypt
                user.UserPass = BCrypt.Net.BCrypt.HashPassword(user.UserPass);
                user.UserCreated = usercreator;
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserProfile"] = new SelectList(_context.tbl_aim_profiles, "ProfileId", "ProfileDescription", user.UserProfile);
            ViewData["UserStatus"] = new SelectList(_context.tbl_aim_status, "StatusCode", "StatusCode", user.UserStatus);
            return View(user);
        }

       
        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(string id)
        
        {
           

            var user = await _context.tbl_aim_users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
			ViewData["UserUpdated"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName");
			ViewData["UserProfile"] = new SelectList(_context.tbl_aim_profiles, "ProfileId", "ProfileName");
            ViewData["UserStatus"] = new SelectList(_context.tbl_aim_status, "StatusCode", "StatusName");
            return View(user);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(User user)
        {
            var LoginUser = HttpContext.Session.GetString("FullName");

            if (ModelState.IsValid)
            {
                try
                {
                    // Check if the UserPass is already hashed (assuming bcrypt format)
                    if (!user.UserPass.StartsWith("$2a$"))
                    {
                        // Hash the new password using bcrypt
                        user.UserPass = BCrypt.Net.BCrypt.HashPassword(user.UserPass);
                    }

                    user.UserUpdated = LoginUser;
                    user.UserDtUpdated = DateTime.Now;
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.UserCode))
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

            return View(user);
        }



        public IActionResult ResetPassword(string id)
        {
            // Check if the user exists with the given id
            var user = _context.tbl_aim_users.FirstOrDefault(u => u.UserCode == id);
            if (user == null)
            {
                return NotFound();
            }

            // Reset the user's password to a default value (e.g., "1234")
            user.UserPass = BCrypt.Net.BCrypt.HashPassword("1234");

            _context.SaveChanges();

            // Redirect back to the user list or any other desired page
            return RedirectToAction("Index");
        }
        // GET: Users/Edit/5
        public async Task<IActionResult> ChangePassword(string id)

        {

            var user = await _context.tbl_aim_users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            ViewData["UserUpdated"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName");
            ViewData["UserProfile"] = new SelectList(_context.tbl_aim_profiles, "ProfileId", "ProfileName");
            ViewData["UserStatus"] = new SelectList(_context.tbl_aim_status, "StatusCode", "StatusName");
            return View();
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]

        public async Task<IActionResult> ChangePassword(string CurrentPass, string newPassword, string confirmPassword)
		{

            var userName = HttpContext.Session.GetString("UserName");
            var user = _context.tbl_aim_users.FirstOrDefault(u => u.UserCode == userName);

            if (user == null)
            {
                return NotFound();
            }

            // Perform validation on old password

            if (!BCrypt.Net.BCrypt.Verify(CurrentPass, user.UserPass))
            {
                ModelState.AddModelError("CurrentPass", "Current password is incorrect");
                TempData["ErrorMessage"] = "Current password is incorrect";
                return RedirectToAction("ChangePassword");
            }

            // Perform validation on new password and confirm password
            if (newPassword != confirmPassword)
            {
                ModelState.AddModelError("confirmPassword", "New password and confirm password do not match");
                TempData["ErrorMessage"] = "New password and confirm password do not match";
                return RedirectToAction("ChangePassword");
            }
            // Hash the password using bcrypt

            // Hash the password using bcrypt
            user.UserPass = BCrypt.Net.BCrypt.HashPassword(newPassword);


            await _context.SaveChangesAsync();


            TempData["PasswordChangeSuccess"] = true;

            return RedirectToAction("Login");
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.tbl_aim_users == null)
            {
                return NotFound();
            }

            var user = await _context.tbl_aim_users
                .Include(u => u.Profile)
                .Include(u => u.Status)
                .FirstOrDefaultAsync(m => m.UserCode == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.tbl_aim_users == null)
            {
                return Problem("Entity set 'AimDbContext.tbl_aim_users'  is null.");
            }
            var user = await _context.tbl_aim_users.FindAsync(id);
            if (user != null)
            {
                _context.tbl_aim_users.Remove(user);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(string id)
        {
          return (_context.tbl_aim_users?.Any(e => e.UserCode == id)).GetValueOrDefault();
        }
    }
}
