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
    public class BrandsController : Controller
    {
        private readonly StoreProjectContext _context;

        public BrandsController(StoreProjectContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IActionResult> Index(BrandListViewModel brandListViewModel)
        {
            IQueryable<Brand> brands = from b in _context.Brand
                                       select b;

            if (!string.IsNullOrEmpty(brandListViewModel.SearchString))
            {
                brands = brands.Where(s => s.BrandName.Contains(brandListViewModel.SearchString));
            }

            int pageSize = 4;
            brandListViewModel.PageNumber = brandListViewModel.PageNumber <= 0 ? 1 : brandListViewModel.PageNumber;

            var count = await brands.CountAsync();
            var items = await brands.OrderBy(b => b.BrandId)
                  .Skip((brandListViewModel.PageNumber - 1) * pageSize)
                  .Take(pageSize).ToListAsync();

            brandListViewModel.Brands = new StaticPagedList<Brand>(items, brandListViewModel.PageNumber, pageSize, count);

            return View(brandListViewModel);
        }

        // GET: Brands/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var brand = await _context.Brand
                .FirstOrDefaultAsync(m => m.BrandId == id);
            if (brand == null)
            {
                return NotFound();
            }

            return View(brand);
        }

        // GET: Brands/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Brands/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Brand brand)
        {

            if (ModelState.IsValid)
            {

                var nameIsExist = _context.Brand.Any(b => b.BrandName == brand.BrandName);
                if (nameIsExist)
                {
                    ModelState.AddModelError("BrandName", "Cant Create Brand, This Brand Name is Already Exist ");
                    return View(brand);
                }
                _context.Brand.Add(brand);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(brand);
        }

        // GET: Brands/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var brand = await _context.Brand.FindAsync(id);
            if (brand == null)
            {
                return NotFound();
            }
            return View(brand);
        }

        // POST: Brands/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Brand brand)
        {
            if (ModelState.IsValid)
            {
                var nameIsExist = _context.Brand.Any(b => b.BrandName == brand.BrandName && b.BrandId != id);
                if (nameIsExist)
                {
                    ModelState.AddModelError("brandname", "cant update brand, this brand name is already exist ");
                    return View(brand);
                }

                var result = await _context.Brand.FindAsync(id);

                if (result == null)
                {
                    return NotFound();
                }

                result.BrandName = brand.BrandName;
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));

            }
            return View(brand);

        }

        // GET: Brands/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var brand = await _context.Brand
                .FirstOrDefaultAsync(m => m.BrandId == id);
            if (brand == null)
            {
                return NotFound();
            }

            return View(brand);
        }

        // POST: Brands/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var brand = await _context.Brand.FindAsync(id);
            _context.Brand.Remove(brand);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BrandExists(int id)
        {
            return _context.Brand.Any(e => e.BrandId == id);
        }
    }
}
