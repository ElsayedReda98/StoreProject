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
    public class CustomersController : Controller
    {
        private readonly StoreProjectContext _context;

        public CustomersController(StoreProjectContext context)
        {
            _context = context;
        }

        // GET: Customers
        // before search 
        //public async Task<IActionResult> Index()
        //{
        //    return View(await _context.Customer.ToListAsync());
        //}
        //************************************************
        public async Task<IActionResult> Index(CustomerListViewModel customerListViewModel)
        {
            var customers = from c in _context.Customer
                            select c;
            if (!string.IsNullOrWhiteSpace(customerListViewModel.NameSearch))
            {
                customers = customers.Where(s => (s.FirstName + " " + s.LastName).Contains(customerListViewModel.NameSearch.Trim()));
                                               
            }
            if (!string.IsNullOrEmpty(customerListViewModel.EmailSearch))
            {
                customers = customers.Where(c => c.Email.Contains(customerListViewModel.EmailSearch)); 
            }
            if (!string.IsNullOrEmpty(customerListViewModel.PhoneSearch))
            {
                customers = customers.Where(c => c.Phone.Contains(customerListViewModel.PhoneSearch));
            }

            int pageSize = 10;
            customerListViewModel.PageNumber = customerListViewModel.PageNumber <= 0 ? 1 : customerListViewModel.PageNumber;

            var count = await customers.CountAsync();
            var items = await customers.OrderBy(c => c.CustomerId)
                .Skip((customerListViewModel.PageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // manual pagination
            customerListViewModel.Customers = new StaticPagedList<Customer>
                (items, customerListViewModel.PageNumber, pageSize, count);

            //implicit pagination
            //customerListViewModel.Customers = customers
              //  .ToPagedList(customerListViewModel.PageNumber, pageSize);

            //customerListViewModel.Customers =await customers.ToListAsync();
            return View(customerListViewModel);
        }
        // GET: Customers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customer
                .FirstOrDefaultAsync(m => m.CustomerId == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // GET: Customers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CustomerId,FirstName,LastName,Phone,Email,Street,City,State,ZipCode")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                var fNameExist = _context.Customer.Any(c => c.FirstName == customer.FirstName);
                var lNameExist = _context.Customer.Any(l => l.LastName == customer.LastName);
                //var nameExist = _context.Customer.Any(n => n.FullName == customer.FullName);
                var emailExist = _context.Customer.Any(l => l.Email == customer.Email);
                if (fNameExist & lNameExist )
                {
                    ModelState.AddModelError("FirstName", "Can not create customer, This Customer is Already Exist ");
                    ModelState.AddModelError("LastName", "Can not create customer, This Customer is Already Exist ");
                    return View(customer);
                }
                else if (emailExist)
                {
                    ModelState.AddModelError("Email", "Can't create Customer, This Email is Already Exist ");
                    return View(customer);
                }
                _context.Add(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customer.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CustomerId,FirstName,LastName,Phone,Email,Street,City,State,ZipCode")] Customer customer)
        {
            if (id != customer.CustomerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var fNameExist = _context.Customer.Any(f => f.FirstName == customer.FirstName && f.CustomerId != id);
                    var lNameExist = _context.Customer.Any(l => l.LastName == customer.LastName && l.CustomerId != id);
                    var emailExist = _context.Customer.Any(e => e.Email == customer.Email && e.CustomerId != id);
                    if (fNameExist && lNameExist)
                    {
                        ModelState.AddModelError("FirstName", "Can't update customer, This Name is Already Exist ");
                        ModelState.AddModelError("LastName", "Can't update customer, This Name is Already Exist  ");
                        return View(customer);
                    }
                    else if (emailExist)
                    {
                        ModelState.AddModelError("Email", "Can't update customer, This Email is Already Exist");
                        return View(customer);
                    }
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.CustomerId))
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
            return View(customer);
        }

        // GET: Customers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customer
                .FirstOrDefaultAsync(m => m.CustomerId == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customer = await _context.Customer.FindAsync(id);
            _context.Customer.Remove(customer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(int id)
        {
            return _context.Customer.Any(e => e.CustomerId == id);
        }
    }
}
