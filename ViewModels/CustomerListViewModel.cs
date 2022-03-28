using StoreProject.Models;
using X.PagedList;

namespace StoreProject.ViewModels
{
    public class CustomerListViewModel
    {
        public string NameSearch { get; set; }
        public string EmailSearch { get; set; }
        public string PhoneSearch { get; set; }
        public int PageNumber { get; set; } = 1;
        public IPagedList<Customer> Customers { get; set; }

    }
}
