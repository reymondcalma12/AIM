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
    public class StoresController : BaseController
    {
        private readonly AimDbContext _context;

        public StoresController(AimDbContext context):base(context)
        {
            _context = context;
        }

        // GET: Stores
        public async Task<IActionResult> Index()
        {
            await PopulateViewBag();
            var aimDbContext = _context.tbl_aim_stores.Include(s => s.Status);
            return PartialView("Index",await aimDbContext.ToListAsync());
        }

        // GET: Stores/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.tbl_aim_stores == null)
            {
                return NotFound();
            }

            var store = await _context.tbl_aim_stores
                .Include(s => s.Status)
                .FirstOrDefaultAsync(m => m.StoreCode == id);
            if (store == null)
            {
                return NotFound();
            }

            return View(store);
        }

        // GET: Stores/Create
        public IActionResult Create()
        {
            ViewData["StoreStatus"] = new SelectList(_context.tbl_aim_status, "StatusCode", "StatusName");
            return View();
        }

        // POST: Stores/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StoreCode,StoreName,StoreStatus")] Store store)
        {
            if (ModelState.IsValid)
            {
                _context.Add(store);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["StoreStatus"] = new SelectList(_context.tbl_aim_status, "StatusCode", "StatusCode", store.StoreStatus);
            return View(store);
        }

        // GET: Stores/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.tbl_aim_stores == null)
            {
                return NotFound();
            }

            var store = await _context.tbl_aim_stores.FindAsync(id);
            if (store == null)
            {
                return NotFound();
            }
            ViewData["StoreStatus"] = new SelectList(_context.tbl_aim_status, "StatusCode", "StatusCode", store.StoreStatus);
            return View(store);
        }

        // POST: Stores/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("StoreCode,StoreName,StoreStatus")] Store store)
        {
            if (id != store.StoreCode)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(store);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StoreExists(store.StoreCode))
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
            ViewData["StoreStatus"] = new SelectList(_context.tbl_aim_status, "StatusCode", "StatusCode", store.StoreStatus);
            return View(store);
        }

        // GET: Stores/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.tbl_aim_stores == null)
            {
                return NotFound();
            }

            var store = await _context.tbl_aim_stores
                .Include(s => s.Status)
                .FirstOrDefaultAsync(m => m.StoreCode == id);
            if (store == null)
            {
                return NotFound();
            }

            return View(store);
        }

        // POST: Stores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.tbl_aim_stores == null)
            {
                return Problem("Entity set 'AimDbContext.Store'  is null.");
            }
            var store = await _context.tbl_aim_stores.FindAsync(id);
            if (store != null)
            {
                _context.tbl_aim_stores.Remove(store);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
            
        private bool StoreExists(string id)
        {
          return (_context.tbl_aim_stores?.Any(e => e.StoreCode == id)).GetValueOrDefault();
        }
    }
}
