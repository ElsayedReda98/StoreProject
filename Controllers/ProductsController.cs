﻿#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ReflectionIT.Mvc.Paging;
using StoreProject.Data;
using StoreProject.Models;
using StoreProject.ViewModels;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace StoreProject.Controllers
{
    public class ProductsController : Controller
    {
        private readonly StoreProjectContext _context;

        public ProductsController(StoreProjectContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        //public async Task<IActionResult> Index(int pg = 1)
        //{
        //    List<Product> products = _context.Product.ToList();

        //    const int pageSize = 10;
        //    if (pg < 1)
        //    {
        //        pg = 1;
        //    }

        //    int rescCount = products.Count();

        //    var pager = new Pager(rescCount, pg, pageSize);

        //    int recSkip = (pg - 1) * pageSize;

        //    var data = products.Skip(recSkip).Take(pager.PageSize).ToList();

        //    ViewBag.pager = pager;

        //    return View(data);

        //}
        
        //GET : Products
        public async Task<IActionResult> Index(
            ProductListViewModel productListViewModel)
        {
            
            //use Linq to get list of Brands
            var brandQuery = from b in _context.Brand
                             select b;

            var catecgoryQuery = from c in _context.Category
                                 select c;

            IQueryable<Product> products = _context.Product
                            .Include(s => s.Brand)
                            .Include(c => c.Category);


            if (!string.IsNullOrEmpty(productListViewModel. SearchString))
            {
                products = products.Where(s => s.ProductName.Contains(productListViewModel.SearchString));
            }
            if (productListViewModel.SelectedBrand > 0 )
            {
                products = products.Where(x => x.BrandId == productListViewModel.SelectedBrand);
            }

            if (productListViewModel.SelectedCategory > 0 )
            {
                products = products.Where(x => x.CategoryId == productListViewModel.SelectedCategory);
            }

            if (productListViewModel.SelectedYear > 0 )
            {
                products = products.Where(z => z.ModelYear == productListViewModel.SelectedYear);
            }

            productListViewModel.Brands = await _context.Brand.Select
                        (a => new SelectListItem()
                        {
                            Text = a.BrandName,
                            Value = a.BrandId.ToString()
                        }).ToListAsync();

            productListViewModel.Categories = await _context.Category.Select
                    (c => new SelectListItem(c.CategoryName, c.CategoryId.ToString())).ToListAsync();

            productListViewModel.ModelYears = await _context.Product.Select
                    (y => new SelectListItem()
                    {
                        Text = y.ModelYear.ToString(),
                        Value = y.ModelYear.ToString()
                    }).Distinct().ToListAsync();

            //productListViewModel.Products = await products.ToListAsync();

            const int pageSize = 10;
            if (productListViewModel.PageNumber < 1)
            {
                productListViewModel.PageNumber = 1;
            }

            int rescCount = products.Count();

            var pager = new ProductListViewModel(rescCount, productListViewModel.PageNumber, pageSize);

            int recSkip = (productListViewModel.PageNumber - 1) * pageSize;

            
            productListViewModel.Products = products.Skip(recSkip).Take(pager.PageSize).ToList();

            ViewBag.pager = pager;

            return View(productListViewModel);

        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                //return NotFound();
                return BadRequest();
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
               
            var productVM = new ProductViewModel();
            productVM.Brands = await _context.Brand.Select
            (a => new SelectListItem()
            {
                Text = a.BrandName,
                Value = a.BrandId.ToString()
            }).ToListAsync();

            productVM.Categories = await _context.Category.Select
                (c => new SelectListItem(c.CategoryName, c.CategoryId.ToString())).ToListAsync();

            productVM.MYears = await _context.Product.Select
                (y => new SelectListItem()
                {
                    Text = y.ModelYear.ToString(),
                    Value = y.ModelYear.ToString()
                }).Distinct().ToListAsync();

            
            //productVM.BrandId = 281;

            //productVM.CategoryId = 26;

            //productVM.ModelYear = 2022;


            return View(productVM);
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,ProductName,BrandId,CategoryId,ModelYear,ListPrice")] Product product)
        {

            var productVM = new ProductViewModel();
            productVM.Brands = await _context.Brand.Select
                (a => new SelectListItem()
                {
                    Text = a.BrandName,
                    Value = a.BrandId.ToString()
                }).ToListAsync();

            productVM.Categories = await _context.Category.Select
                (c => new SelectListItem(c.CategoryName, c.CategoryId.ToString())).ToListAsync();

            productVM.MYears = await _context.Product.Select
                (y => new SelectListItem()
                {
                    Text = y.ModelYear.ToString(),
                    Value = y.ModelYear.ToString()
                }).Distinct().ToListAsync();
            if (ModelState.IsValid)
            {
                var productNameExist = _context.Product.Any(p => p.ProductName == product.ProductName);
                if (productNameExist)
                {
                    ModelState.AddModelError("ProductName", "Can't Create Product, This Product Name is Already Exist ");
                    return View(productVM);
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
        //    var productVM = new ProductViewModel();
        //    productVM.Brands = await _context.Brand.Select
        //        (a => new SelectListItem()
        //        {
        //            Text = a.BrandName,
        //            Value = a.BrandId.ToString()
        //        }).ToListAsync();

        //    productVM.Categories = await _context.Category.Select
        //        (c => new SelectListItem(c.CategoryName, c.CategoryId.ToString())).ToListAsync();

        //    productVM.MYears = await _context.Product.Select
        //        (y => new SelectListItem()
        //        {
        //            Text = y.ModelYear.ToString(),
        //            Value = y.ModelYear.ToString()
        //        }).Distinct().ToListAsync();

            if (id == null)
            {
                return BadRequest();
            }

             var product = await _context.Product.FindAsync(id);
            
            if (product == null)
            {
                return BadRequest();
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
                return BadRequest();
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
