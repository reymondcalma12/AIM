using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AIM.Data;
using AIM.Models;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;

namespace AIM.Controllers
{
    public class CategoriesController : BaseController
    {
        private readonly AimDbContext _context;

        public CategoriesController(AimDbContext context):base(context)
        {
            _context = context;
        }

        // GET: Categories
        public async Task<IActionResult> Index()
        {

              return _context.tbl_aim_category != null ? 
                          PartialView("Index",await _context.tbl_aim_category.Include(m=>m.Status).ToListAsync( )) :
                          Problem("Entity set 'AimDbContext.tbl_aim_category'  is null.");
        }

        // GET: Categories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.tbl_aim_category == null)
            {
                return NotFound();
            }

            var category = await _context.tbl_aim_category
                .FirstOrDefaultAsync(m => m.CategoryId == id);
            if (category == null)
            {
                return NotFound();
            }

            return PartialView(category);
        }

        // GET: Categories/Create
        public IActionResult Create()
        {
            ViewData["CategoryStatus"] = new SelectList(_context.tbl_aim_status, "StatusCode", "StatusName");
            return PartialView();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategoryId,CategoryName,CategoryStatus")] Category category)
        {
          
            if (ModelState.IsValid)
            {
                var categoryParameter = await _context.tbl_aim_parameters.FirstOrDefaultAsync(c => c.ParmCode == "category");

                if(categoryParameter !=null && categoryParameter.ParmValue != null)
                {
                    var currentCategoryCode = categoryParameter.ParmValue;

                    var newCategoryCode = currentCategoryCode + 1;

                    category.CategoryId = newCategoryCode;
                    category.CategoryName = category.CategoryName.ToUpper();
                    categoryParameter.ParmValue = newCategoryCode;

                    _context.Add(category);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }


            }
            return PartialView(category);
        }

        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.tbl_aim_category == null)
            {
                return NotFound();
            }

            var category = await _context.tbl_aim_category.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return PartialView(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CategoryId,CategoryName,CategoryStatus")] Category updatedCategory)
        {
            if (id != updatedCategory.CategoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Retrieve the existing entity with the same key
                    var existingCategory = await _context.tbl_aim_category.FindAsync(id);

                    if (existingCategory == null)
                    {
                        return NotFound();
                    }

                    // Update the properties of the existing entity
                    existingCategory.CategoryName = updatedCategory.CategoryName;
                    existingCategory.CategoryStatus = updatedCategory.CategoryStatus;
    
                    _context.Update(existingCategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(updatedCategory.CategoryId))
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
            return View(updatedCategory);
        }

        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.tbl_aim_category == null)
            {
                return NotFound();
            }

            var category = await _context.tbl_aim_category
                .FirstOrDefaultAsync(m => m.CategoryId == id);
            if (category == null)
            {
                return NotFound();
            }

            return PartialView(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.tbl_aim_category == null)
            {
                return Problem("Entity set 'AimDbContext.tbl_aim_category'  is null.");
            }
            var category = await _context.tbl_aim_category.FindAsync(id);
            if (category != null)
            {
                _context.tbl_aim_category.Remove(category);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
          return (_context.tbl_aim_category?.Any(e => e.CategoryId == id)).GetValueOrDefault();
        }
    }
}
