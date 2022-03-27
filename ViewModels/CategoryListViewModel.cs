using StoreProject.Models;
using X.PagedList;

namespace StoreProject.ViewModels
{
    public class CategoryListViewModel
    {
        public string SearchString { get; set; }

        public int PageNumber { get; set; } = 1;
        public IPagedList<Category> Categories { get; set; }
    }
}
