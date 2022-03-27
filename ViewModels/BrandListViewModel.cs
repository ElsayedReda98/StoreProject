using StoreProject.Models;
using X.PagedList;

namespace StoreProject.ViewModels
{
    public class BrandListViewModel
    {
        public string SearchString { get; set; }

        public int PageNumber { get; set; } = 1;
        public IPagedList<Brand> Brands { get; set; }

    }
}
