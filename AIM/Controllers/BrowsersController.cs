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
    public class BrowsersController : BaseController
    {
        private readonly AimDbContext _context;

        public BrowsersController(AimDbContext context):base(context)
        {
            _context = context;
        }

        // GET: Browsers
        public async Task<IActionResult> Index()
        {
            var aimDbContext = _context.tbl_aim_browser.Include(b => b.CreatedBy).Include(b => b.Status).Include(b => b.UpdatedBy);
            return PartialView(await aimDbContext.ToListAsync());
        }

        // GET: Browsers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.tbl_aim_browser == null)
            {
                return NotFound();
            }

            var browser = await _context.tbl_aim_browser
                .Include(b => b.CreatedBy)
                .Include(b => b.Status)
                .Include(b => b.UpdatedBy)
                .FirstOrDefaultAsync(m => m.BrowserCode == id);
            if (browser == null)
            {
                return NotFound();
            }

            return PartialView(browser);
        }

        // GET: Browsers/Create
        public IActionResult Create()
        {
            ViewData["BrowserCreatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName");
            ViewData["BrowserStatus"] = new SelectList(_context.tbl_aim_status, "StatusCode", "StatusName");
            ViewData["BrowserUpdatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName");
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BrowserCode,BrowserName,BrowserStatus,BrowserCreatedBy,BrowserCreatedDt,BrowserUpdatedBy,BrowserUpdatedDt")] Browser browser)
        {
            if (ModelState.IsValid)
            {
                // Check the browser_no parameter
                var browserNoParameter = await _context.tbl_aim_parameters.FirstOrDefaultAsync(c => c.ParmCode == "browser_no");

                if (browserNoParameter != null && browserNoParameter.ParmValue != null)
                {
                    var currentBrowserNo = browserNoParameter.ParmValue;
                    var newBrowserNo = currentBrowserNo + 1;
                    browser.BrowserCode = newBrowserNo;

                    // Update the browser_no parameter
                    browserNoParameter.ParmValue = newBrowserNo;
                }

                browser.BrowserCreatedBy = HttpContext.Session.GetString("UserName");
                browser.BrowserCreatedDt = DateTime.Now;

                _context.Add(browser);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["BrowserCreatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName", browser.BrowserCreatedBy);
            ViewData["BrowserStatus"] = new SelectList(_context.tbl_aim_status, "StatusCode", "StatusName", browser.BrowserStatus);
            ViewData["BrowserUpdatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName", browser.BrowserUpdatedBy);
            return PartialView(browser);
        }

        // GET: Browsers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.tbl_aim_browser == null)
            {
                return NotFound();
            }

            var browser = await _context.tbl_aim_browser.FindAsync(id);
            if (browser == null)
            {
                return NotFound();
            }
            ViewData["BrowserCreatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName", browser.BrowserCreatedBy);
            ViewData["BrowserStatus"] = new SelectList(_context.tbl_aim_status, "StatusCode", "StatusName", browser.BrowserStatus);
            ViewData["BrowserUpdatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName", browser.BrowserUpdatedBy);
            return PartialView(browser);
        }

        // POST: Browsers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BrowserCode,BrowserName,BrowserStatus,BrowserCreatedBy,BrowserCreatedDt,BrowserUpdatedBy,BrowserUpdatedDt")] Browser browser)
        {
            if (id != browser.BrowserCode)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    browser.BrowserUpdatedBy = HttpContext.Session.GetString("UserName");
                    browser.BrowserUpdatedDt = DateTime.Now;
                    _context.Update(browser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BrowserExists(browser.BrowserCode))
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
            ViewData["BrowserCreatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName", browser.BrowserCreatedBy);
            ViewData["BrowserStatus"] = new SelectList(_context.tbl_aim_status, "StatusCode", "StatusName", browser.BrowserStatus);
            ViewData["BrowserUpdatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName", browser.BrowserUpdatedBy);
            return PartialView(browser);
        }

        // GET: Browsers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.tbl_aim_browser == null)
            {
                return NotFound();
            }

            var browser = await _context.tbl_aim_browser
                .Include(b => b.CreatedBy)
                .Include(b => b.Status)
                .Include(b => b.UpdatedBy)
                .FirstOrDefaultAsync(m => m.BrowserCode == id);
            if (browser == null)
            {
                return NotFound();
            }

            return PartialView(browser);
        }

        // POST: Browsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.tbl_aim_browser == null)
            {
                return Problem("Entity set 'AimDbContext.tbl_aim_browser'  is null.");
            }
            var browser = await _context.tbl_aim_browser.FindAsync(id);
            if (browser != null)
            {
                _context.tbl_aim_browser.Remove(browser);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BrowserExists(int id)
        {
          return (_context.tbl_aim_browser?.Any(e => e.BrowserCode == id)).GetValueOrDefault();
        }
    }
}
