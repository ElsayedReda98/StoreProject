using StoreProject.Models;

namespace StoreProject.ViewModels
{
    public class CustomerListViewModel
    {
        public string NameSearch { get; set; }
        public string EmailSearch { get; set; }
        public string PhoneSearch { get; set; }
        public List<Customer> Customers { get; set; }

    }
}
