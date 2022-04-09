using StoreProject.Models;
using X.PagedList;

namespace StoreProject.ViewModels.Stores
{
    public class StoreIndexViewModel
    {
        public string  Name { get; set; }
        public string Phone { get; set; }
        public string  Email { get; set; }
        public int PageNumber { get; set; }

        public IPagedList<Store> Stores { get; set; }
    }
}
