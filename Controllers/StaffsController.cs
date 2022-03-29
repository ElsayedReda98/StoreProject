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
using StoreProject.ViewModels;
using X.PagedList;

namespace StoreProject.Controllers
{
    public class StaffsController : Controller
    {
        private readonly StoreProjectContext _context;

        public StaffsController(StoreProjectContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        // GET: Staffs
        //public async Task<IActionResult> Index()
        //{
        //    return View(await _context.Staff.ToListAsync());
        //}
        //**************************************************************

        public async Task<IActionResult> Index(StaffListViewModel staffListViewModel)
        {
            //var staffList = from s in _context.Staff
            //                select s;
            //*******************************************
            IQueryable<Staff> staffList = _context.Staff
                                .Include(s => s.Store);
                                       
            //*********************************************

            //if (!string.IsNullOrEmpty(staffListViewModel.NameSearch))
            //{
            //    staffList = staffList.Where(s => s.FirstName.Contains(staffListViewModel.NameSearch)
            //                                   || s.LastName.Contains(staffListViewModel.NameSearch));
            //}
            //******************************************************
            /* error as FullName prop not mapped to database
            if (!string.IsNullOrEmpty(staffListViewModel.NameSearch))
            {
                staffList = staffList.Where(s => s.FullName.Contains(staffListViewModel.NameSearch));
            }
            */
            if (!string.IsNullOrEmpty(staffListViewModel.NameSearch))
            {
                staffList = staffList.Where(s => (s.FirstName + s.LastName).Contains(staffListViewModel.NameSearch));
            }
            if (!string.IsNullOrEmpty(staffListViewModel.EmailSearch))
            {
                staffList = staffList.Where(s => s.Email.Contains(staffListViewModel.EmailSearch));
            }
            if (!string.IsNullOrEmpty(staffListViewModel.PhoneSearch))
            {
                staffList = staffList.Where(s => s.Phone.Contains(staffListViewModel.PhoneSearch));
            }
            if (staffListViewModel.ActiveSearch > 0)
            {
                staffList = staffList.Where(s => s.Active == staffListViewModel.ActiveSearch);
            }
            if (staffListViewModel.SelectedManager > 0)
            {
                staffList = staffList.Where(s => s.ManagerId == staffListViewModel.SelectedManager);
            }
            if (staffListViewModel.SelectedStore > 0)
            {
                staffList = staffList.Where(s => s.StoreId == staffListViewModel.SelectedStore);
            }
            int pageSize = 10;
            staffListViewModel.PageNumber = staffListViewModel.PageNumber <= 0 ? 1 : staffListViewModel.PageNumber; 
            var count =await staffList.CountAsync();
            var  items = await staffList.OrderBy(s => s.FirstName)
                .Skip((staffListViewModel.PageNumber -1 ) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            staffListViewModel.Staffs = new StaticPagedList<Staff>
                (items, staffListViewModel.PageNumber, pageSize, count); ;

            await FileLookUp(staffListViewModel);
            return View(staffListViewModel);
        }
        private async Task FileLookUp(StaffListViewModel staffListViewModel)
        {
            staffListViewModel.Managers = await _context.Staff
                .Select(s => new SelectListItem(s.FirstName, s.ManagerId.ToString()))
                .Distinct()
                .ToListAsync();

            staffListViewModel.Stores = await _context.Store
                .Select(s => new SelectListItem(s.StoreName, s.StoreId.ToString()))
                .Distinct()
                .ToListAsync();
            staffListViewModel.ActiveList = await _context.Staff
                .Select(s => new SelectListItem(s.Active.ToString(), s.Active.ToString()))
                .Distinct()
                .ToListAsync();
        }

        

        // GET: Staffs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var staff = await _context.Staff
                .FirstOrDefaultAsync(m => m.StaffId == id);
            if (staff == null)
            {
                return NotFound();
            }

            return View(staff);
        }

        // GET: Staffs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Staffs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StaffId,FirstName,LastName,Email,Phone,Active,StoreId,ManagerId")] Staff staff)
        {
            if (ModelState.IsValid)
            {
                _context.Add(staff);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(staff);
        }

        // GET: Staffs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var staff = await _context.Staff.FindAsync(id);
            if (staff == null)
            {
                return NotFound();
            }
            return View(staff);
        }

        // POST: Staffs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StaffId,FirstName,LastName,Email,Phone,Active,StoreId,ManagerId")] Staff staff)
        {
            if (id != staff.StaffId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(staff);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StaffExists(staff.StaffId))
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
            return View(staff);
        }

        // GET: Staffs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var staff = await _context.Staff
                .FirstOrDefaultAsync(m => m.StaffId == id);
            if (staff == null)
            {
                return NotFound();
            }

            return View(staff);
        }

        // POST: Staffs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var staff = await _context.Staff.FindAsync(id);
            _context.Staff.Remove(staff);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StaffExists(int id)
        {
            return _context.Staff.Any(e => e.StaffId == id);
        }
    }
}
