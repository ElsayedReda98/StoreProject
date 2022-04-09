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
using StoreProject.ViewModels.Stores;
using X.PagedList;

namespace StoreProject.Controllers
{
    public class StoresController : Controller
    {
        private readonly StoreProjectContext _context;

        public StoresController(StoreProjectContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(Index)); ;
        }

        // GET: Stores
        //public async Task<IActionResult> Index()
        //{
        //    return View(await _context.Store.ToListAsync());
        //}
        public async Task<IActionResult> Index(StoreIndexViewModel storeIndexViewModel)
        {
            IQueryable<Store> stores = from s in _context.Store
                                       orderby s.StoreId
                                       select s;

            if (!string.IsNullOrWhiteSpace(storeIndexViewModel.Name))
            {
                stores = stores.Where(s => s.StoreName.Contains(storeIndexViewModel.Name));
            }
            if (!string.IsNullOrWhiteSpace(storeIndexViewModel.Phone))
            {
                stores = stores.Where(s => s.Phone.Contains(storeIndexViewModel.Phone));
            }
            if (!string.IsNullOrWhiteSpace(storeIndexViewModel.Email))
            {
                stores = stores.Where(s => s.Email.Contains(storeIndexViewModel.Email));
            }

            int pageSize = 4;
            storeIndexViewModel.PageNumber = storeIndexViewModel.PageNumber <= 0 ? 1 : storeIndexViewModel.PageNumber;

            var count = await stores.CountAsync();
            var items = await stores.OrderBy(s => s.StoreId)
                .Skip((storeIndexViewModel.PageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();


            storeIndexViewModel.Stores = new StaticPagedList<Store>
                (items, storeIndexViewModel.PageNumber, pageSize, count);
            return View(storeIndexViewModel);
        }

        // GET: Stores/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var store = await _context.Store
                .FirstOrDefaultAsync(m => m.StoreId == id);
            if (store == null)
            {
                return NotFound();
            }

            return View(store);
        }

        // GET: Stores/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Stores/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StoreId,StoreName,Phone,Email,Street,City,State,ZipCodde")] Store store)
        {
            if (ModelState.IsValid)
            {
                _context.Add(store);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(store);
        }

        // GET: Stores/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var store = await _context.Store.FindAsync(id);
            if (store == null)
            {
                return NotFound();
            }
            return View(store);
        }

        // POST: Stores/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StoreId,StoreName,Phone,Email,Street,City,State,ZipCodde")] Store store)
        {
            if (id != store.StoreId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(store);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StoreExists(store.StoreId))
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
            return View(store);
        }

        // GET: Stores/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var store = await _context.Store
                .FirstOrDefaultAsync(m => m.StoreId == id);
            if (store == null)
            {
                return NotFound();
            }

            return View(store);
        }

        // POST: Stores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var store = await _context.Store.FindAsync(id);
            _context.Store.Remove(store);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StoreExists(int id)
        {
            return _context.Store.Any(e => e.StoreId == id);
        }
    }
}
