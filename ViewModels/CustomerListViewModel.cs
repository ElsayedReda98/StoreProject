using StoreProject.Models;
using X.PagedList;

namespace StoreProject.ViewModels
{
    public class CustomerListViewModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int PageNumber { get; set; } = 1;
        public IPagedList<Customer> Customers { get; set; }

    }
}
