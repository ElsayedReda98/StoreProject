#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StoreProject.Data;
using StoreProject.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace StoreProject.Controllers
{
    public class ProductsController : Controller
    {
        private readonly StoreProjectContext _context;

        public ProductsController(StoreProjectContext context)
        {
            _context = context;
        }
        //******************************************
        // Index before Search
        // GET: Products
        //public async Task<IActionResult> Index()
        //{
        //    return View(await _context.Product.ToListAsync());
        //}

        //*******************************************

        //GET : Products
        public async Task<IActionResult> Index(string searchString)
        {
            var products = from p in _context.Product
                           select p;

            if (!string.IsNullOrEmpty(searchString))
            {
                products = products.Where(x => x.ProductName.Contains(searchString));
            }
            return View(await products.ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public async Task<IActionResult> Create()
        {
            //var brands = await _context.Brand.ToListAsync();
            
            //or
            //var brands =  from b in _context.Brand
            //                   select new { b.BrandId, b.BrandName };
            
            //var categories = await _context.Category.ToListAsync();

            //or
            //var categories = from c in _context.Category
            //                 select new {c.CategoryId,c.CategoryName};

            //var modelYears = await _context.Product.ToListAsync();

            //or
            //var modelyears = from y in _context.Product
            //                 select y.ModelYear;

            //or
            //IQueryable<short> modelYears = from y in _context.Product
            //                       select y.ModelYear;

            var productVM = new ProductViewModel();
            //{
            //Brands = new SelectList(brands, "BrandId", "BrandName"),

            //Categories = new SelectList(categories,"CategoryId","CategoryName"),

            ///MYears = new SelectList(modelYears,"ModelYear","ModelYear",2017)

            //or
            //Brands = new SelectList(brands.Select(c => new SelectListItem( c.BrandName,c.BrandId.ToString()))),

            //or


            //Brands = brands.Select(b => new SelectListItem(b.BrandName, b.BrandId.ToString())),


            //Categories = categories.Select(c => new SelectListItem(c.CategoryName, c.CategoryId.ToString())),

            //MYears = modelYears.Select(y => new SelectListItem(y.ModelYear.ToString(), y.ModelYear.ToString())) 
            //};
            productVM.Brands =await _context.Brand.Select
                (a => new SelectListItem()
                {
                    Text = a.BrandName,
                    Value = a.BrandId.ToString()
                }).ToListAsync();

            productVM.Categories = await _context.Category.Select
                (c => new SelectListItem(c.CategoryName,c.CategoryId.ToString())).ToListAsync();

            productVM.MYears = await _context.Product.Select
                (y => new SelectListItem()
                {
                    Text = y.ModelYear.ToString(),
                    Value = y.ModelYear.ToString()
                }).Distinct().ToListAsync();

            productVM.BrandId = 281;

            productVM.CategoryId = 26;

            productVM.ModelYear = 2018;
           

            return View(productVM);
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,ProductName,BrandId,CategoryId,ModelYear,ListPrice")] Product product)
        {

            if (ModelState.IsValid)
            {
                var productNameExist = _context.Product.Any(p => p.ProductName == product.ProductName);
                if (productNameExist)
                {
                    ModelState.AddModelError("ProductName", "Can't Create Product, This Product Name is Already Exist ");
                    return View(product);
                }

                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,ProductName,BrandId,CategoryId,ModelYear,ListPrice")] Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var productNameExist = _context.Product.Any(p => p.ProductName == product.ProductName && p.ProductId != id);
                    if (productNameExist)
                    {
                        ModelState.AddModelError("ProductName", "cant update Product, This Product name is already exist ");
                        return View(product);
                    }


                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
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
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Product.FindAsync(id);
            _context.Product.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.ProductId == id);
        }
    }
}
