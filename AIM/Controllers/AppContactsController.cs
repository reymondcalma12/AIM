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
    public class AppContactsController : BaseController
    {
        private readonly AimDbContext _context;

        public AppContactsController(AimDbContext context):base(context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appContacts = await _context.tbl_aim_app_contacts
                .Include(a => a.Application)
                .Include(a => a.SupportType)
                .Include(a => a.User)
                .Where(m => m.AppCode == id)
                .ToListAsync();

            // Store the "id" as "AppCode" in the session
            HttpContext.Session.SetString("AppCode", id);

            return PartialView(appContacts);
        }


        // GET: AppContacts/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Split the composite ID into its components
            var idComponents = id.Split('-');
            if (idComponents.Length != 3)
            {
                return NotFound();
            }

            string appCode = idComponents[0];
            string contactNo = idComponents[1];
            int supportTypeCode;

            if ( !int.TryParse(idComponents[2], out supportTypeCode))
            {
                return NotFound();
            }

            var appContact = await _context.tbl_aim_app_contacts
                .Include(a => a.Application)
                .Include(a => a.SupportType)
                .Include(a => a.User)
                .FirstOrDefaultAsync(m => m.AppCode == appCode && m.ContactNo.Equals(contactNo) && m.SupportTypeCode == supportTypeCode);

            if (appContact == null)
            {
                return NotFound();
            }

            return PartialView(appContact);
        }


        // GET: AppContacts/Create
        public IActionResult Create()
        {
            ViewData["AppCode"] = new SelectList(_context.tbl_aim_applications, "AppCode", "AppName", HttpContext.Session.GetString("AppCode"));
            ViewData["SupportTypeCode"] = new SelectList(_context.tbl_aim_supporttype, "SupportTypeCode", "SupportTypeName");
            ViewData["CreatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName");
            return PartialView();
        }

        // POST: AppContacts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AppCode,ContactNo,SupportTypeCode,CreatedBy,CreatedDt")] AppContact appContact)
        {
            if (ModelState.IsValid)
            {
                appContact.CreatedBy = HttpContext.Session.GetString("UserName");
                appContact.CreatedDt = DateTime.Now;
                _context.Add(appContact);
                await _context.SaveChangesAsync();
                // Redirect to the "Index" action of the "APPLICATIONS" controller with the "AppCode" from the session
                return RedirectToAction("Details", "APPLICATIONS", new { id = HttpContext.Session.GetString("AppCode") });
            }
            ViewData["AppCode"] = new SelectList(_context.tbl_aim_applications, "AppCode", "AppName", appContact.AppCode);
            ViewData["SupportTypeCode"] = new SelectList(_context.tbl_aim_supporttype, "SupportTypeCode", "SupportTypeName", appContact.SupportTypeCode);
            ViewData["CreatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName", appContact.CreatedBy);
            return PartialView(appContact);
        }

        // GET: AppContacts/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Split the composite ID into its components
            var idComponents = id.Split('-');
            if (idComponents.Length != 3)
            {
                return NotFound();
            }

            string appCode = idComponents[0];
            string contactNo = idComponents[1];
            int supportTypeCode;

            if (!int.TryParse(idComponents[2], out supportTypeCode))
            {
                return NotFound();
            }

            var appContact = await _context.tbl_aim_app_contacts.FindAsync(appCode, contactNo, supportTypeCode);
            if (appContact == null)
            {
                return NotFound();
            }

            ViewData["AppCode"] = new SelectList(_context.tbl_aim_applications, "AppCode", "AppName", appContact.AppCode);
            ViewData["SupportTypeCode"] = new SelectList(_context.tbl_aim_supporttype, "SupportTypeCode", "SupportTypeName", appContact.SupportTypeCode);
            ViewData["CreatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName", appContact.CreatedBy);
            return PartialView(appContact);
        }


        // POST: AppContacts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("AppCode,ContactNo,SupportTypeCode,CreatedBy,CreatedDt")] AppContact appContact)
        {
            if (id != appContact.AppCode)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(appContact);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppContactExists(appContact.AppCode))
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
            ViewData["AppCode"] = new SelectList(_context.tbl_aim_applications, "AppCode", "AppName", appContact.AppCode);
            ViewData["SupportTypeCode"] = new SelectList(_context.tbl_aim_supporttype, "SupportTypeCode", "SupportTypeName", appContact.SupportTypeCode);
            ViewData["CreatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserName", appContact.CreatedBy);
            return PartialView(appContact);
        }

        // GET: AppContacts/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Split the composite ID into its components
            var idComponents = id.Split('-');
            if (idComponents.Length != 3)
            {
                return NotFound();
            }

            string appCode = idComponents[0];
            string contactNo = idComponents[1];
            int supportTypeCode;

            if ( !int.TryParse(idComponents[2], out supportTypeCode))
            {
                return NotFound();
            }

            var appContact = await _context.tbl_aim_app_contacts
                .Include(a => a.Application)
                .Include(a => a.SupportType)
                .Include(a => a.User)
                .FirstOrDefaultAsync(m => m.AppCode == appCode && m.ContactNo.Equals(contactNo) && m.SupportTypeCode == supportTypeCode);

            if (appContact == null)
            {
                return NotFound();
            }

            return PartialView(appContact);
        }


        // POST: AppContacts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Split the composite ID into its components
            var idComponents = id.Split('-');
            if (idComponents.Length != 3)
            {
                return NotFound();
            }

            string appCode = idComponents[0];
            string contactNo = idComponents[1];
            int supportTypeCode;

            if ( !int.TryParse(idComponents[2], out supportTypeCode))
            {
                return NotFound();
            }

            var appContact = await _context.tbl_aim_app_contacts.FindAsync(appCode, contactNo, supportTypeCode);
            if (appContact == null)
            {
                return NotFound();
            }

            _context.tbl_aim_app_contacts.Remove(appContact);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        private bool AppContactExists(string id)
        {
          return (_context.tbl_aim_app_contacts?.Any(e => e.AppCode == id)).GetValueOrDefault();
        }
    }
}
