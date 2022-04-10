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
            _context = context ?? throw new ArgumentNullException(nameof(context)); ;
        }

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
        public async Task<IActionResult> Create(StoreCreateViewModel storeCreateViewModel)
        {
            if (ModelState.IsValid)
            {
                var phoneExist = _context.Store.Any(p => p.Phone == storeCreateViewModel.Phone);
                if (phoneExist)
                {
                    ModelState.AddModelError("Phone", "Can't Create, This Phone is Already Exist");
                    return View();
                }

                var emailExist = _context.Store.Any(e => e.Email == storeCreateViewModel.Email);
                if (emailExist)
                {
                    ModelState.AddModelError("Email", "Can't Create, This Email is Already Exist");
                    return View();
                }

                Store store = new Store()
                {
                    StoreName = storeCreateViewModel.StoreName,
                    Phone = storeCreateViewModel.Phone,
                    Email = storeCreateViewModel.Email,
                    Street = storeCreateViewModel.Street,
                    City = storeCreateViewModel.City,
                    State = storeCreateViewModel.State,
                    ZipCode = storeCreateViewModel.ZipCode 
                };

                _context.Store.Add(store);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(storeCreateViewModel);
        }

        // GET: Stores/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var store = await _context.Store.FindAsync(id);
            if (store == null)
            {
                return NotFound();
            }
            StoreEditViewModel storeEditViewModel = new StoreEditViewModel()
            {
                StoreName = store.StoreName,
                Phone = store.Phone,
                Email = store.Email,
                Street = store.Street,
                City = store.City,
                State = store.State,
                ZipCode = store.ZipCode

            };

            return View(storeEditViewModel);
        }

        // POST: Stores/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, StoreEditViewModel storeEditViewModel )
        {
            if (ModelState.IsValid)
            {
                var store = await _context.Store.FindAsync(id);
                
                if (store == null)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    store.StoreName = storeEditViewModel.StoreName;
                    store.Phone = storeEditViewModel.Phone;
                    store.Email = storeEditViewModel.Email;
                    store.Street = storeEditViewModel.Street;
                    store.City = storeEditViewModel.City;
                    store.State = storeEditViewModel.State;
                    store.ZipCode = storeEditViewModel.ZipCode;

                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index)); 
                }

            }
            
            
            
            
            return View(storeEditViewModel);
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
