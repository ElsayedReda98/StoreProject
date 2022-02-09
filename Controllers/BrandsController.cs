using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StoreProject.Data;
using StoreProject.Models;

namespace StoreProject.Controllers
{
    public class BrandsController : Controller
    {
        private readonly StoreProjectContext _context;

        public BrandsController(StoreProjectContext context)
        {
            _context = context;
        }

        // GET: Brands
        //public async Task<IActionResult> Index()
        //{
        //    return View(await _context.Brand.ToListAsync());
        //}
        //************************************
        public async Task<IActionResult> Index(string searchString)
        {
            var brands = from b in _context.Brand
                         select b;

            if (!string.IsNullOrEmpty(searchString))
            {
                brands = brands.Where(s => s.BrandName.Contains(searchString));
            }
            return View(await brands.ToListAsync());
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
        public async Task<IActionResult> Create([Bind("BrandId,BrandName")] Brand brand)
        {
            if (ModelState.IsValid)
            {
                var nameIsExist = _context.Brand.Any(b => b.BrandName == brand.BrandName);
                if (nameIsExist)
                {
                    ModelState.AddModelError("BrandName", "Cant Create Brand, This Brand Name is Already Exist ");
                    return View(brand);
                }
                _context.Add(brand);
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
                return NotFound();
            }

            //var brand = await _context.Brand.FindAsync(id);
            var brand = await _context.Brand.FirstOrDefaultAsync(s => s.BrandId == id);
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
        public async Task<IActionResult> Edit(int id, [Bind("BrandId,BrandName")] Brand brand)
        {
            if (id != brand.BrandId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var nameIsExist = _context.Brand.Any(b => b.BrandName == brand.BrandName && brand.BrandId != id );

                    if (nameIsExist)
                    {

                        
                            ModelState.AddModelError("brandname", "cant update brand, this brand name is already exist ");
                            return View(brand);

                            


                    }
                    _context.Update(brand);
                    await _context.SaveChangesAsync();
                    

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BrandExists(brand.BrandId))
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
