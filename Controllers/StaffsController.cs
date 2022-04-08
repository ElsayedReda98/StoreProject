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
            IQueryable<Staff> staffList = _context.Staff
                                .Include(m => m.Manager)
                                .Include(s => s.Store);

            if (!string.IsNullOrWhiteSpace(staffListViewModel.Name))
            {
                staffList = staffList.Where(s => (s.FirstName + s.LastName).Contains(staffListViewModel.Name.Trim()));
            }
            if (!string.IsNullOrWhiteSpace(staffListViewModel.Email))
            {
                staffList = staffList.Where(s => s.Email.Contains(staffListViewModel.Email.Trim()));
            }
            if (!string.IsNullOrWhiteSpace(staffListViewModel.Phone))
            {
                staffList = staffList.Where(s => s.Phone.Contains(staffListViewModel.Phone.Trim()));
            }
            if (staffListViewModel.Active.HasValue)
            {
                byte x  = (byte)(staffListViewModel.Active.Value ? 1 : 0);
                staffList = staffList.Where(s => s.Active == x);
            }
            if (staffListViewModel.Manager > 0)
            {
                staffList = staffList.Where(s => s.ManagerId == staffListViewModel.Manager);
            }
            if (staffListViewModel.Store > 0)
            {
                staffList = staffList.Where(s => s.StoreId == staffListViewModel.Store);
            }
            int pageSize = 15;
            staffListViewModel.PageNumber = staffListViewModel.PageNumber <= 0 ? 1 : staffListViewModel.PageNumber;
            
            var count =await staffList.CountAsync();
            var  items = await staffList.OrderBy(s => s.StaffId)
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
                .Select(s => new SelectListItem(s.FullName,s.StaffId.ToString()))
                .Distinct()
                .ToListAsync();

            staffListViewModel.Stores = await _context.Store
                .Select(s => new SelectListItem(s.StoreName, s.StoreId.ToString()))
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
        public async Task<IActionResult> Create()
        {
            StaffCreateViewModel staffCreateViewModel = new StaffCreateViewModel() 
            {
                Stores = new SelectList(_context.Store.OrderBy(s => s.StoreId).Distinct().ToList(), "StoreId", "StoreName"),
                Managers = new SelectList(_context.Staff.OrderBy(s => s.StaffId).Distinct().ToList(), "StaffId", "FirstName")
            };
            
            return View(staffCreateViewModel);
        }

        // POST: Staffs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Createpost(StaffCreateViewModel staffCreateViewModel)
        {
            Staff staff = new Staff();
            staffCreateViewModel.Stores = await _context.Store.Select
                (r => new SelectListItem(r.StoreName, r.StoreId.ToString()))
                .ToListAsync();
            staffCreateViewModel.Managers = await _context.Staff.Select
                (m => new SelectListItem(m.FirstName, m.StaffId.ToString()))
                .Distinct()
                .ToListAsync();
                
            if (ModelState.IsValid)
            {
                var emailExist = _context.Staff.Any(e => e.Email == staffCreateViewModel.Email);
                var phoneExist = _context.Staff.Any(p => p.Phone == staffCreateViewModel.Phone);
                if (emailExist)
                {
                    ModelState.AddModelError("Email", "This Email is Already Exist");
                    return View();
                }
                else if (phoneExist)
                {
                    ModelState.AddModelError("Phone", "This Phone is Already Exist");
                    return View();
                }

                //Staff staff = new Staff();
                staff.FirstName = staffCreateViewModel.FirstName;
                staff.LastName = staffCreateViewModel.LastName;
                staff.Email = staffCreateViewModel.Email;
                staff.Phone = staffCreateViewModel.Phone;
                staff.StoreId = staffCreateViewModel.StoreId;
                staff.ManagerId = staffCreateViewModel.ManagerId;
                staff.Active = (byte)(staffCreateViewModel.Active ? 1 : 0);

                _context.Staff.Add(staff);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
                
            }

            staffCreateViewModel.Stores = new SelectList(_context.Store.OrderBy(s => s.StoreId).Distinct().ToList(), "StoreId", "StoreName");
            staffCreateViewModel.Managers = new SelectList(_context.Staff.OrderBy(s => s.StaffId).Distinct().ToList(), "StaffId", "FirstName");
            
            return View(staffCreateViewModel);

        }

        // GET: Staffs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {

            if (id == null)
            {
                return BadRequest();
            }
            
            //var staff = await _context.Staff.FindAsync(id);
            var staff = await _context.Staff.FirstOrDefaultAsync(s => s.StaffId == id);

            if (staff == null)
            {
                return NotFound();
            }
            StaffEditViewModel staffEditViewModel = new StaffEditViewModel();
            
            staffEditViewModel.StaffId = staff.StaffId;
            staffEditViewModel.FirstName = staff.FirstName;
            staffEditViewModel.LastName = staff.LastName;
            staffEditViewModel.Email = staff.Email;
            staffEditViewModel.Phone = staff.Phone;
            staffEditViewModel.StoreId = staff.StoreId;
            staffEditViewModel.ManagerId = staff?.ManagerId;
            staffEditViewModel.Active = Convert.ToBoolean(staff.Active);

            staffEditViewModel.Stores = new SelectList(_context.Store.OrderBy(s => s.StoreId).ToList(), "StoreId", "StoreName");
            staffEditViewModel.Managers = new SelectList(_context.Staff.OrderBy(s => s.StaffId).ToList(), "StaffId", "FullName");
            
            return View(staffEditViewModel);
        }

        // POST: Staffs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,StaffEditViewModel staffEditViewModel)
        {
            if (ModelState.IsValid)
            {
                var staff = await _context.Staff.FirstOrDefaultAsync(s => s.StaffId == id);

                if (staff == null)
                {
                    return NotFound();
                }

                var emailExist = _context.Staff.Any(e => e.Email == staffEditViewModel.Email && e.StaffId != id);
                var phoneExist = _context.Staff.Any(p => p.Phone == staffEditViewModel.Phone && p.StaffId != id);
                
                if (emailExist)
                {
                    ModelState.AddModelError("Email", "can't update, This Email is Already Exist");
                    
                }
                
                if (phoneExist)
                {
                    ModelState.AddModelError("Phone", "can't update, This Phone is Already Exist");
                    
                }

                if (ModelState.IsValid)
                {

                    staff.FirstName = staffEditViewModel.FirstName;
                    staff.LastName = staffEditViewModel.LastName;
                    staff.Email = staffEditViewModel.Email;
                    staff.Phone = staffEditViewModel.Phone;
                    staff.StoreId = staffEditViewModel.StoreId;
                    staff.ManagerId = staffEditViewModel.ManagerId;
                    staff.Active = (byte)(staffEditViewModel.Active ? 1 : 0);

                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
            }
            staffEditViewModel.Stores = new SelectList(_context.Store.OrderBy(s => s.StoreId).ToList(), "StoreId", "StoreName");
            staffEditViewModel.Managers = new SelectList(_context.Staff.OrderBy(s => s.StaffId).ToList(), "StaffId", "FullName");

            return View(staffEditViewModel);
   
            
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
