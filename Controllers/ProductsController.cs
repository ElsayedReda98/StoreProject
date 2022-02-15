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
        public async Task<IActionResult> Index()
        {
            return View(await _context.Product.ToListAsync());
        }
        //*******************************************

        // GET : Products
        //public async Task<IActionResult> Index(string searchString)
        //{
        //    //var products = from p in _context.Product
        //               select p;

        //if (!string.IsNullOrEmpty(searchString))
        //{
        //    products = products.Where(x => x.ProductName.Contains(searchString));
        //}
        //return View(await products.ToListAsync());
        //}

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

            var bM = new ProductViewModel();
            bM.BrandName = "5";
            bM.Brands = new List<SelectListItem>
            {
                new SelectListItem { Text = "brand1", Value = "1"},
                new SelectListItem { Text = "brand2", Value = "2"},
                new SelectListItem { Text = "brand3", Value = "3"},
                new SelectListItem { Text = "brand4", Value = "4"},
                new SelectListItem { Text = "brand5", Value = "5"},
            };


            return View(bM);
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
