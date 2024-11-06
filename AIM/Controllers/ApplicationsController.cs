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
    public class ApplicationsController : BaseController
    {
        private readonly AimDbContext _context;

        public ApplicationsController(AimDbContext context):base(context)
        {
            _context = context;
        }

        // GET: Applications
        public async Task<IActionResult> Index()
        {
            var aimDbContext = _context.tbl_aim_applications.Include(a => a.AuthMethod).Include(a => a.Category).Include(a => a.CreatedBy).Include(a => a.CriticalLevel).Include(a => a.Group).Include(a => a.Location).Include(a => a.OS).Include(a => a.PrintSpooler).Include(a => a.SLALevel).Include(a => a.Server).Include(a => a.Status).Include(a => a.SystemClass).Include(a => a.UpdatedBy);
            return PartialView(await aimDbContext.ToListAsync());
        }


        // GET: Applications/ByServer/5
        public async Task<IActionResult> ByServer(int? serverCode)
        {
            if (serverCode == null)
            {
                return NotFound();
            }

            var serverApplications = await _context.tbl_aim_applications
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
                .Include(a => a.UpdatedBy)
                .Include(a => a.tbl_aim_subscriptionType)
                .Where(a => a.Server != null && a.Server.ServerCode == serverCode)
                .ToListAsync();

            if (serverApplications == null || serverApplications.Count == 0)
            {
                return NotFound();
            }

            return PartialView(serverApplications);
        }

        // GET: Applications/Details/5
        public async Task<IActionResult> Details(string id)
                {
                    if (id == null || _context.tbl_aim_applications == null)
                    {
                        return NotFound();
                    }

                    var application = await _context.tbl_aim_applications
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
                        .Include(a => a.UpdatedBy)
                        .Include(a => a.tbl_aim_subscriptionType)
                        .FirstOrDefaultAsync(m => m.AppCode == id);
                    if (application == null)
                    {
                        return NotFound();
                    }

                    return PartialView(application);
                }

        // GET: Applications/Create
        public IActionResult Create()
        {
            ViewData["AppAuthMethod"] = new SelectList(_context.tbl_aim_authmethod, "AuthCode", "AuthName");
            ViewData["AppCategory"] = new SelectList(_context.tbl_aim_appcategory, "CatCode", "CatName");
            ViewData["AppCreatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName");
            ViewData["AppCritLevel"] = new SelectList(_context.tbl_aim_criticallevel, "CriticalLevelCode", "CriticalLevelName");
            ViewData["AppGroupName"] = new SelectList(_context.tbl_aim_group, "GroupCode", "GroupName");
            ViewData["AppLocation"] = new SelectList(_context.tbl_aim_apploc, "LocationCode", "LocationName");
            ViewData["AppOS"] = new SelectList(_context.tbl_aim_operatingsystem, "OsCode", "OsName");
            ViewData["AppPrintSpool"] = new SelectList(_context.tbl_aim_printspooler, "SpoolerCode", "SpoolerName");
            ViewData["AppSLALevel"] = new SelectList(_context.tbl_aim_slalevel, "SLALevelCode", "SLALevelName");
            ViewData["AppServerName"] = new SelectList(_context.tbl_aim_server, "ServerCode", "ServerName");
            ViewData["AppStatus"] = new SelectList(_context.tbl_aim_status, "StatusCode", "StatusName");
            ViewData["AppSystemClass"] = new SelectList(_context.tbl_aim_systemclass, "ClassCode", "ClassName");
            ViewData["AppUpdatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName");
            ViewData["SubscriptionType"] = new SelectList(_context.tbl_aim_subscriptionType, "subscriptionType_code", "subscriptionType_name");
            return PartialView();
        }

        // POST: Applications/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AppCode,AppName,AppCategory,AppCritLevel,AppSLALevel,AppVersion,AppLocation,AppDescription,AppProgramApiEndpoint,AppApiUrlEndpoint,AppPortUsed,AppSupportsSD,AppServerName,AppOS,AppAuthMethod,AppPrintSpool,AppGroupName,AppSystemClass,AppStatus,AppCreatedBy,AppCreatedDt,AppUpdatedBy,AppUpdatedDt,subscriptionType_code,AppExpiryDate")] Application application)
        {
            if (ModelState.IsValid)
            {
                // Count the number of records in the Applications table
                int appCount = await _context.tbl_aim_applications.CountAsync();

                // Generate the next AppCode based on the count
                string nextAppCode = "APP" + (appCount + 1).ToString().PadLeft(12, '0'); // 12 for the count part

                application.AppCode = nextAppCode;
                application.AppCreatedBy = HttpContext.Session.GetString("UserName");
                application.AppCreatedDt = DateTime.Now;

                _context.Add(application);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AppAuthMethod"] = new SelectList(_context.tbl_aim_authmethod, "AuthCode", "AuthName", application.AppAuthMethod);
            ViewData["AppCategory"] = new SelectList(_context.tbl_aim_category, "CategoryId", "CategoryName", application.AppCategory);
            ViewData["AppCreatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName", application.AppCreatedBy);
            ViewData["AppCritLevel"] = new SelectList(_context.tbl_aim_criticallevel, "CriticalLevelCode", "CriticalLevelName", application.AppCritLevel);
            ViewData["AppGroupName"] = new SelectList(_context.tbl_aim_group, "GroupCode", "GroupName", application.AppGroupName);
            ViewData["AppLocation"] = new SelectList(_context.tbl_aim_apploc, "LocationCode", "LocationName", application.AppLocation);
            ViewData["AppOS"] = new SelectList(_context.tbl_aim_operatingsystem, "OsCode", "OsName", application.AppOS);
            ViewData["AppPrintSpool"] = new SelectList(_context.tbl_aim_printspooler, "SpoolerCode", "SpoolerName", application.AppPrintSpool);
            ViewData["AppSLALevel"] = new SelectList(_context.tbl_aim_slalevel, "SLALevelCode", "SLALevelName", application.AppSLALevel);
            ViewData["AppServerName"] = new SelectList(_context.tbl_aim_server, "ServerCode", "ServerName", application.AppServerName);
            ViewData["AppStatus"] = new SelectList(_context.tbl_aim_status, "StatusCode", "StatusName", application.AppStatus);
            ViewData["AppSystemClass"] = new SelectList(_context.tbl_aim_systemclass, "ClassCode", "ClassName", application.AppSystemClass);
            ViewData["AppUpdatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName", application.AppUpdatedBy);
            ViewData["SubscriptionType"] = new SelectList(_context.tbl_aim_subscriptionType, "subscriptionType_code", "subscriptionType_name", application.subscriptionType_code);

            return PartialView(application);
        }

        // GET: Applications/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.tbl_aim_applications == null)
            {
                return NotFound();
            }

            var application = await _context.tbl_aim_applications.FindAsync(id);

            if (application == null)
            {
                return NotFound();
            }
            ViewData["AppAuthMethod"] = new SelectList(_context.tbl_aim_authmethod, "AuthCode", "AuthName", application.AppAuthMethod);
            ViewData["AppCategory"] = new SelectList(_context.tbl_aim_category, "CategoryId", "CategoryName", application.AppCategory);
            ViewData["AppCreatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName", application.AppCreatedBy);
            ViewData["AppCritLevel"] = new SelectList(_context.tbl_aim_criticallevel, "CriticalLevelCode", "CriticalLevelName", application.AppCritLevel);
            ViewData["AppGroupName"] = new SelectList(_context.tbl_aim_group, "GroupCode", "GroupName", application.AppGroupName);
            ViewData["AppLocation"] = new SelectList(_context.tbl_aim_apploc, "LocationCode", "LocationName", application.AppLocation);
            ViewData["AppOS"] = new SelectList(_context.tbl_aim_operatingsystem, "OsCode", "OsName", application.AppOS);
            ViewData["AppPrintSpool"] = new SelectList(_context.tbl_aim_printspooler, "SpoolerCode", "SpoolerName", application.AppPrintSpool);
            ViewData["AppSLALevel"] = new SelectList(_context.tbl_aim_slalevel, "SLALevelCode", "SLALevelName", application.AppSLALevel);
            ViewData["AppServerName"] = new SelectList(_context.tbl_aim_server, "ServerCode", "ServerName", application.AppServerName);
            ViewData["AppStatus"] = new SelectList(_context.tbl_aim_status, "StatusCode", "StatusName", application.AppStatus);
            ViewData["AppSystemClass"] = new SelectList(_context.tbl_aim_systemclass, "ClassCode", "ClassName", application.AppSystemClass);
            ViewData["AppUpdatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName", application.AppUpdatedBy);
            ViewData["SubscriptionType"] = new SelectList(_context.tbl_aim_subscriptionType, "subscriptionType_code", "subscriptionType_name", application.subscriptionType_code);

            return PartialView(application);
        }

        // POST: Applications/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("AppCode,AppName,AppCategory,AppCritLevel,AppSLALevel,AppVersion,AppLocation,AppDescription,AppProgramApiEndpoint,AppApiUrlEndpoint,AppPortUsed,AppSupportsSD,AppServerName,AppOS,AppAuthMethod,AppPrintSpool,AppGroupName,AppSystemClass,AppStatus,AppCreatedBy,AppCreatedDt,AppUpdatedBy,AppUpdatedDt,AppExpiryDate,subscriptionType_code")] Application application)
        {
            if (id != application.AppCode)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Retrieve the existing entity from the database
                    var existingApplication = await _context.tbl_aim_applications.FindAsync(application.AppCode);
                    if (existingApplication == null)
                    {
                        return NotFound();
                    }

                    // Update properties
                    existingApplication.AppName = application.AppName;
                    existingApplication.AppCategory = application.AppCategory;
                    existingApplication.AppCritLevel = application.AppCritLevel;
                    existingApplication.AppSLALevel = application.AppSLALevel;
                    existingApplication.AppVersion = application.AppVersion;
                    existingApplication.AppLocation = application.AppLocation;
                    existingApplication.AppDescription = application.AppDescription;
                    existingApplication.AppProgramApiEndpoint = application.AppProgramApiEndpoint;
                    existingApplication.AppApiUrlEndpoint = application.AppApiUrlEndpoint;
                    existingApplication.AppPortUsed = application.AppPortUsed;
                    existingApplication.AppSupportsSD = application.AppSupportsSD;
                    existingApplication.AppServerName = application.AppServerName;
                    existingApplication.AppOS = application.AppOS;
                    existingApplication.AppAuthMethod = application.AppAuthMethod;
                    existingApplication.AppPrintSpool = application.AppPrintSpool;
                    existingApplication.AppGroupName = application.AppGroupName;
                    existingApplication.AppSystemClass = application.AppSystemClass;
                    existingApplication.AppStatus = application.AppStatus;
                    existingApplication.AppCreatedBy = application.AppCreatedBy; // Consider whether this should be updated
                    existingApplication.AppUpdatedBy = HttpContext.Session.GetString("UserName");
                    existingApplication.AppUpdatedDt = DateTime.Now;
                    existingApplication.subscriptionType_code = application.subscriptionType_code;
                    existingApplication.AppExpiryDate = application.AppExpiryDate;

                    // Save changes
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApplicationExists(application.AppCode))
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

            // Populate ViewData for the view in case of invalid model state

            return View(application);
        }

        // GET: Applications/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.tbl_aim_applications == null)
            {
                return NotFound();
            }

            var application = await _context.tbl_aim_applications
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
                .Include(a => a.UpdatedBy)
                .FirstOrDefaultAsync(m => m.AppCode == id);
            if (application == null)
            {
                return NotFound();
            }

            return PartialView(application);
        }

        // POST: Applications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.tbl_aim_applications == null)
            {
                return Problem("Entity set 'AimDbContext.tbl_aim_applications'  is null.");
            }
            var application = await _context.tbl_aim_applications.FindAsync(id);
            if (application != null)
            {
                _context.tbl_aim_applications.Remove(application);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ApplicationExists(string id)
        {
          return (_context.tbl_aim_applications?.Any(e => e.AppCode == id)).GetValueOrDefault();
        }
    }
}
