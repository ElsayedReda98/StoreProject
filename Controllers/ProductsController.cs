#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StoreProject.Data;
using StoreProject.Models;
using StoreProject.ViewModels;
using X.PagedList;

namespace StoreProject.Controllers
{
    public class ProductsController : Controller
    {
        private readonly StoreProjectContext _context;

        public ProductsController(StoreProjectContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        //GET : Products
        public async Task<IActionResult> Index(
            ProductListViewModel productListViewModel)
        {
            
            IQueryable<Product> products = _context.Product
                            .Include("Brand")
                            .Include("Category");

            if (!string.IsNullOrEmpty(productListViewModel.SearchString))
            {
                products = products.Where(s => s.ProductName.Contains(productListViewModel.SearchString));
            }
            if (productListViewModel.SelectedBrand > 0)
            {
                products = products.Where(x => x.BrandId == productListViewModel.SelectedBrand);
            }

            if (productListViewModel.SelectedCategory > 0)
            {
                products = products.Where(y => y.CategoryId == productListViewModel.SelectedCategory);
            }

            if (productListViewModel.SelectedYear > 0)
            {
                products = products.Where(z => z.ModelYear == productListViewModel.SelectedYear);
            }

            int pageSize = 10;
            productListViewModel.PageNumber = productListViewModel.PageNumber <= 0 ? 1 : productListViewModel.PageNumber;

            var count = await products.CountAsync();
            var items = await products.OrderBy(c => c.ProductName)
                .Skip((productListViewModel.PageNumber - 1) * pageSize) // 1 skip 0, 2 => 1 * 10 = 10, 3 => 2 * 10 = 20
                .Take(pageSize).ToListAsync();

            productListViewModel.Products = new StaticPagedList<Product>
                (items, productListViewModel.PageNumber, pageSize, count);

            await FillLookup(productListViewModel);
            return View(productListViewModel);
        }

        private async Task FillLookup(ProductListViewModel productListViewModel)
        {
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
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var product = await _context.Product
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return BadRequest();
            }

            return View(product);
        }

        // GET: Products/Create
        public async Task<IActionResult> Create(ProductViewModel productViewModel)
        {


            //productViewModel.Brands = await _context.Brand.Select
            //(a => new SelectListItem()
            //{
            //    Text = a.BrandName,
            //    Value = a.BrandId.ToString()
            //}).ToListAsync();

            //productViewModel.Categories = await _context.Category.Select
            //    (c => new SelectListItem(c.CategoryName, c.CategoryId.ToString())).ToListAsync();

            //productViewModel.MYears = await _context.Product.Select
            //    (y => new SelectListItem()
            //    {
            //        Text = y.ModelYear.ToString(),
            //        Value = y.ModelYear.ToString()
            //    }).Distinct().ToListAsync();


            //productVM.BrandId = 281;

            //productVM.CategoryId = 26;

            //productVM.ModelYear = 2022;

            ViewBag.Brands = _context.Brand.OrderBy(b => b.BrandId).ToList();

            ViewBag.Categories = _context.Category.OrderBy(b => b.CategoryId).ToList();
            
            ViewBag.Years = await ( _context.Product).Distinct().ToListAsync();

            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,ProductName,BrandId,CategoryId,ModelYear,ListPrice")] Product product)
        {
            //var productVM = new ProductViewModel();
            //productVM.Brands = await _context.Brand.Select
            //        (a => new SelectListItem()
            //        {
            //            Text = a.BrandName,
            //            Value = a.BrandId.ToString()
            //        }).ToListAsync();

            //productVM.Categories = await _context.Category.Select
            //    (c => new SelectListItem(c.CategoryName, c.CategoryId.ToString())).ToListAsync();

            //productVM.MYears = await _context.Product.Select
            //    (y => new SelectListItem()
            //    {
            //        Text = y.ModelYear.ToString(),
            //        Value = y.ModelYear.ToString()
            //    }).Distinct().ToListAsync();
            
            ViewBag.Brands = _context.Brand.OrderBy(b => b.BrandId).ToList();

            ViewBag.Categories = _context.Category.OrderBy(b => b.CategoryId).ToList();

            ViewBag.Years = _context.Product.OrderBy(b => b.ModelYear).Distinct().ToList();


            if (ModelState.IsValid)
            {
                var Name_Year_Exist = _context.Product.Any(p => p.ProductName == product.ProductName && p.ModelYear == product.ModelYear);
                
                if (Name_Year_Exist)
                {
                    
                    ModelState.AddModelError("ProductName", "Can't Create Product, This Product Name is Already Exist in This Model Year ");
                    return View(product);
                    
                }
                //var name_Brand_Exist = _context.Product.Any(b => b.ProductName == product.ProductName && b.BrandId == product.BrandId);
                //if (name_Brand_Exist)
                //{
                //    ModelState.AddModelError("ProductName" ," Can't Create Product, This Product Name is Already Exist in This Brand");
                //    return View(product);
                //}

                //var Name_Category_Exist = _context.Product.Any(c => c.ProductName == product.ProductName && c.CategoryId == product.CategoryId);
                //if (Name_Category_Exist)
                //{
                //    ModelState.AddModelError("ProductName", " Can't Create Product, This Product Name is Already Exist in This Category");
                //    return View(product);
                //}


                _context.Product.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Brands = _context.Brand.OrderBy(b => b.BrandId).ToList();

            ViewBag.Categories = _context.Category.OrderBy(b => b.CategoryId).ToList();

            ViewBag.Years = _context.Product.OrderBy(b => b.ModelYear).Distinct().ToList();

            return View();
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            //var productVM = new ProductViewModel();
            //productVM.Brands = await _context.Brand.Select
            //    (a => new SelectListItem()
            //    {
            //        Text = a.BrandName,
            //        Value = a.BrandId.ToString()
            //    }).ToListAsync();

            //productVM.Categories = await _context.Category.Select
            //    (c => new SelectListItem(c.CategoryName, c.CategoryId.ToString())).ToListAsync();

            //productVM.MYears = await _context.Product.Select
            //    (y => new SelectListItem()
            //    {
            //        Text = y.ModelYear.ToString(),
            //        Value = y.ModelYear.ToString()
            //    }).Distinct().ToListAsync();

            ViewBag.Brands = _context.Brand.OrderBy(b => b.BrandId).ToList();

            ViewBag.Categories = _context.Category.OrderBy(b => b.CategoryId).ToList();

            ViewBag.Years = _context.Product.OrderBy(b => b.ModelYear).ToList();


            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product.FindAsync(id);
            
            if (product == null)
            {
                return NotFound();
            }
            
            return View( product);
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

                    _context.Product.Update(product);
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
                        Console.WriteLine("Oops, Error");
                        //throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Brands = _context.Brand.OrderBy(b => b.BrandId).ToList();

            ViewBag.Categories = _context.Category.OrderBy(b => b.CategoryId).ToList();

            ViewBag.Years = _context.Product.OrderBy(b => b.ModelYear).Distinct().ToList();


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
