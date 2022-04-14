using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StoreProject.Data;
using StoreProject.Models;
using StoreProject.ViewModels;
using X.PagedList;

namespace StoreProject.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly StoreProjectContext _context;

        public CategoriesController(StoreProjectContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        // GET: Categories
        public async Task<IActionResult> Index(CategoryListViewModel categoryListViewModel)
        {
            IQueryable<Category> categories = from c in _context.Category
                                              select c;

            if (!string.IsNullOrEmpty(categoryListViewModel.SearchString))
            {
                categories = categories.Where(c => c.CategoryName.Contains(categoryListViewModel.SearchString));
            }

            int pageSize = 5;
            categoryListViewModel.PageNumber = categoryListViewModel.PageNumber <= 0 ? 1 : categoryListViewModel.PageNumber;

            var count = await categories.CountAsync();
            var item = await categories.OrderBy(c => c.CategoryName)
                .Skip((categoryListViewModel.PageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();


            categoryListViewModel.Categories = categories.ToPagedList(categoryListViewModel.PageNumber, pageSize);
            return View(categoryListViewModel);
        }



        // GET: Categories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Category
                .FirstOrDefaultAsync(m => m.CategoryId == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Categories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            if (ModelState.IsValid)
            {
                var nameIsExist = _context.Category.Any(x => x.CategoryName == category.CategoryName);
                if (nameIsExist)
                {
                    ModelState.AddModelError("CategoryName", "Cant Creat ,This CategoryName is Already Exists");
                    return View(category);
                }
                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Category.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Category category)
        {
            if (ModelState.IsValid)
            {
                var result = await _context.Category.FindAsync(id);
                if (result is null)
                {
                    return NotFound();
                }
                var nameAlreadyExist = _context.Category.Any(x => x.CategoryName == category.CategoryName && x.CategoryId != id);
                if (nameAlreadyExist)
                {
                    ModelState.AddModelError("CategoryName", "Cant Update,This Category Name Already Exist ");
                    return View(category);
                }

                result.CategoryName = category.CategoryName;
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
             
            return View(category);
        }

        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Category
                .FirstOrDefaultAsync(m => m.CategoryId == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _context.Category.FindAsync(id);
            _context.Category.Remove(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
            return _context.Category.Any(e => e.CategoryId == id);
        }
    }
}
