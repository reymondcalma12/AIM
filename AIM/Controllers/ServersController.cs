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
    public class ServersController : BaseController
    {
        private readonly AimDbContext _context;

        public ServersController(AimDbContext context):base(context)
        {
            _context = context;
        }

        // GET: Servers
        public async Task<IActionResult> Index()
        {
            var aimDbContext = _context.tbl_aim_server.Include(s => s.CreatedBy).Include(s => s.Status).Include(s => s.UpdatedBy).Include(s => s.Type);
            return PartialView(await aimDbContext.ToListAsync());
        }

        // GET: Servers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.tbl_aim_server == null)
            {
                return NotFound();
            }

            var server = await _context.tbl_aim_server
                .Include(s => s.CreatedBy)
                .Include(s => s.Status)
                .Include(s => s.UpdatedBy)
                .FirstOrDefaultAsync(m => m.ServerCode == id);
            if (server == null)
            {
                return NotFound();
            }

            return PartialView(server);
        }

        // GET: Servers/Create
        public IActionResult Create()
        {
            ViewData["ServerCreatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName");
            ViewData["ServerStatus"] = new SelectList(_context.tbl_aim_status, "StatusCode", "StatusName");
            ViewData["ServerUpdatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName");
            ViewData["ServerType"] = new SelectList(_context.tbl_aim_servertype, "TypeCode", "TypeName"); // Add ServerType selection

            return PartialView();
        }

        // POST: Servers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ServerCode,ServerName,ServerIPAddress,ServerType,ServerHostName,ServerHostIPAddress,ServerStatus,ServerCreatedBy,ServerCreatedDt,ServerUpdatedBy,ServerUpdatedDt")] Server server)
        {
            if (ModelState.IsValid)
            {
                // Fetch the current server_no from tbl_aim_parameters
                var serverNoParameter = await _context.tbl_aim_parameters.FirstOrDefaultAsync(p => p.ParmCode == "server_no");

                if (serverNoParameter != null && serverNoParameter.ParmValue != null)
                {
                    // Assign the currentServerNo directly
                    var currentServerNo = serverNoParameter.ParmValue + 1;

                    // Update the server_no in the incoming 'server' object
                    server.ServerCode = currentServerNo;
                    server.ServerCreatedBy = HttpContext.Session.GetString("UserName");
                    server.ServerCreatedDt = DateTime.Now;

                    // Increment the currentServerNo by 1 for the next server
                    serverNoParameter.ParmValue = currentServerNo;

                    _context.Add(server);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }

            ViewData["ServerCreatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName", server.ServerCreatedBy);
            ViewData["ServerStatus"] = new SelectList(_context.tbl_aim_status, "StatusCode", "StatusName", server.ServerStatus);
            ViewData["ServerUpdatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName", server.ServerUpdatedBy);
            ViewData["ServerType"] = new SelectList(_context.tbl_aim_servertype, "TypeCode", "TypeName", server.ServerType); // Add ServerType selection

            return PartialView(server);
        }

        // GET: Servers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var server = await _context.tbl_aim_server.FindAsync(id);
            if (server == null)
            {
                return NotFound();
            }
            ViewData["ServerCreatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName", server.ServerCreatedBy);
            ViewData["ServerStatus"] = new SelectList(_context.tbl_aim_status, "StatusCode", "StatusName", server.ServerStatus);
            ViewData["ServerUpdatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName", server.ServerUpdatedBy);
            ViewData["ServerType"] = new SelectList(_context.tbl_aim_servertype, "TypeCode", "TypeName", server.ServerType); // Add ServerType selection

            return PartialView(server);
        }

        // POST: Servers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ServerCode,ServerName,ServerIPAddress,ServerType,ServerHostName,ServerHostIPAddress,ServerStatus,ServerCreatedBy,ServerCreatedDt,ServerUpdatedBy,ServerUpdatedDt")] Server server)
        {
            if (id != server.ServerCode)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    server.ServerUpdatedBy = HttpContext.Session.GetString("UserName");
                    server.ServerUpdatedDt = DateTime.Now;
                    _context.Update(server);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServerExists(server.ServerCode))
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
            ViewData["ServerCreatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName", server.ServerCreatedBy);
            ViewData["ServerStatus"] = new SelectList(_context.tbl_aim_status, "StatusCode", "StatusName", server.ServerStatus);
            ViewData["ServerUpdatedBy"] = new SelectList(_context.tbl_aim_users, "UserCode", "UserFullName", server.ServerUpdatedBy);
            ViewData["ServerType"] = new SelectList(_context.tbl_aim_servertype, "TypeCode", "TypeName", server.ServerType); // Add ServerType selection

            return PartialView(server);
        }


        // GET: Servers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.tbl_aim_server == null)
            {
                return NotFound();
            }

            var server = await _context.tbl_aim_server
                .Include(s => s.CreatedBy)
                .Include(s => s.Status)
                .Include(s => s.UpdatedBy)
                .Include(s => s.Type)
                .FirstOrDefaultAsync(m => m.ServerCode == id);
            if (server == null)
            {
                return NotFound();
            }

            return PartialView(server);
        }

        // POST: Servers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.tbl_aim_server == null)
            {
                return Problem("Entity set 'AimDbContext.tbl_aim_server'  is null.");
            }
            var server = await _context.tbl_aim_server.FindAsync(id);
            if (server != null)
            {
                _context.tbl_aim_server.Remove(server);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ServerExists(int id)
        {
          return (_context.tbl_aim_server?.Any(e => e.ServerCode == id)).GetValueOrDefault();
        }
    }
}
