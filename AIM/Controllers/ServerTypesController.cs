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
    public class ServerTypesController : Controller
    {
        private readonly AimDbContext _context;

        public ServerTypesController(AimDbContext context)
        {
            _context = context;
        }

        // GET: ServerTypes
        public async Task<IActionResult> Index()
        {
            var aimDbContext = _context.tbl_aim_servertype.Include(s => s.CreatedBy).Include(s => s.Status).Include(s => s.UpdatedBy);
            return View(await aimDbContext.ToListAsync());
        }

        // GET: ServerTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.tbl_aim_servertype == null)
            {
                return NotFound();
            }

            var serverType = await _context.tbl_aim_servertype
                .Include(s => s.CreatedBy)
                .Include(s => s.Status)
                .Include(s => s.UpdatedBy)
                .FirstOrDefaultAsync(m => m.TypeCode == id);
            if (serverType == null)
            {
                return NotFound();
            }

            return View(serverType);
        }

        // GET: ServerTypes/Create
        public IActionResult Create()
        {
            ViewData["TypeCreatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserCode");
            ViewData["TypeStatus"] = new SelectList(_context.tbl_aim_status, "StatusCode", "StatusCode");
            ViewData["TypeUpdatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserCode");
            return View();
        }

        // POST: ServerTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TypeCode,TypeName,TypeStatus,TypeCreatedBy,TypeCreatedDt,TypeUpdatedBy,TypeUpdatedDt")] ServerType serverType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(serverType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TypeCreatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserCode", serverType.TypeCreatedBy);
            ViewData["TypeStatus"] = new SelectList(_context.tbl_aim_status, "StatusCode", "StatusCode", serverType.TypeStatus);
            ViewData["TypeUpdatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserCode", serverType.TypeUpdatedBy);
            return View(serverType);
        }

        // GET: ServerTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.tbl_aim_servertype == null)
            {
                return NotFound();
            }

            var serverType = await _context.tbl_aim_servertype.FindAsync(id);
            if (serverType == null)
            {
                return NotFound();
            }
            ViewData["TypeCreatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserCode", serverType.TypeCreatedBy);
            ViewData["TypeStatus"] = new SelectList(_context.tbl_aim_status, "StatusCode", "StatusCode", serverType.TypeStatus);
            ViewData["TypeUpdatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserCode", serverType.TypeUpdatedBy);
            return View(serverType);
        }

        // POST: ServerTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TypeCode,TypeName,TypeStatus,TypeCreatedBy,TypeCreatedDt,TypeUpdatedBy,TypeUpdatedDt")] ServerType serverType)
        {
            if (id != serverType.TypeCode)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(serverType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServerTypeExists(serverType.TypeCode))
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
            ViewData["TypeCreatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserCode", serverType.TypeCreatedBy);
            ViewData["TypeStatus"] = new SelectList(_context.tbl_aim_status, "StatusCode", "StatusCode", serverType.TypeStatus);
            ViewData["TypeUpdatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserCode", serverType.TypeUpdatedBy);
            return View(serverType);
        }

        // GET: ServerTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.tbl_aim_servertype == null)
            {
                return NotFound();
            }

            var serverType = await _context.tbl_aim_servertype
                .Include(s => s.CreatedBy)
                .Include(s => s.Status)
                .Include(s => s.UpdatedBy)
                .FirstOrDefaultAsync(m => m.TypeCode == id);
            if (serverType == null)
            {
                return NotFound();
            }

            return View(serverType);
        }

        // POST: ServerTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.tbl_aim_servertype == null)
            {
                return Problem("Entity set 'AimDbContext.tbl_aim_servertype'  is null.");
            }
            var serverType = await _context.tbl_aim_servertype.FindAsync(id);
            if (serverType != null)
            {
                _context.tbl_aim_servertype.Remove(serverType);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ServerTypeExists(int id)
        {
          return (_context.tbl_aim_servertype?.Any(e => e.TypeCode == id)).GetValueOrDefault();
        }
    }
}
