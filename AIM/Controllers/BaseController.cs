using AIM.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AIM.Models;

namespace AIM.Controllers
{
    public class BaseController : Controller
    {
        private readonly AimDbContext _context;

        public BaseController(AimDbContext context)
        {
            _context = context;
        }

        protected async Task PopulateViewBag()
        {
            var userCode = HttpContext.Session.GetString("UserName");
            var fullname = HttpContext.Session.GetString("FullName");
            var profilename = HttpContext.Session.GetString("ProfileName");
            var userpass = HttpContext.Session.GetString("UserPass");
            var appcode = HttpContext.Session.GetString("AppCode");

            if (string.IsNullOrEmpty(userCode) || string.IsNullOrEmpty(fullname))
            {
                RedirectToAction("Login", "Users");
            }

            var profileId = Convert.ToInt32(HttpContext.Session.GetString("ProfileId"));
            var moduleIdsWithAccess = await _context.tbl_aim_profileaccess
                .Where(pa => pa.ProfileId == profileId && pa.OpenAccess == "Y")
                .Select(pa => pa.ModuleId)
                .ToListAsync();
            var modulesWithAccess = await _context.tbl_aim_modules
                .Where(m => moduleIdsWithAccess.Contains(m.ModuleId))
                .ToListAsync();
            var categories = await _context.tbl_aim_category.OrderBy(a => a.CategoryName).ToListAsync();
            var status = await _context.tbl_aim_status.ToListAsync();

            // Get applications expiring soon (one month before expiry)
            var applicationsExpiringSoon = await GetApplicationsExpiringSoon();

            ViewBag.userCode = userCode;
            ViewBag.userPass = userpass;
            ViewBag.fullname = fullname;
            ViewBag.profile = profilename;
            ViewBag.categories = categories;
            ViewBag.status = status;
            ViewBag.applicationsExpiringSoon = applicationsExpiringSoon;

            ViewBag.modules = modulesWithAccess;
        }

        private async Task<List<Application>> GetApplicationsExpiringSoon()
        {
            // Calculate the date one month from now
            DateTime oneMonthFromNow = DateTime.Now.AddMonths(1);

            // Query the applications that are one month before expiring
            return await _context.tbl_aim_applications
                .Include(a => a.AuthMethod)
                        .Include(a => a.Category)
                        .Include(a => a.CreatedBy)
                        .Include(a => a.CriticalLevel)
                        .Include(a => a.Group)
                        .Include(a => a.Location)
                        .Include(a => a.OS)
                        .Include(a => a.PrintSpooler)
                        .Include(a => a.SLALevel)
                        .Include(a => a.Server)
                        .Include(a => a.Status)
                        .Include(a => a.SystemClass)
                        .Include(a => a.UpdatedBy).Where(app => app.AppExpiryDate.HasValue && app.AppExpiryDate.Value <= oneMonthFromNow)
                .ToListAsync();
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            PopulateViewBag().Wait(); // Wait synchronously since OnActionExecuting cannot be async
            base.OnActionExecuting(context);
        }
    }
}
