using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StoreProject.Models;

namespace StoreProject.ViewModels
{
    public class ProductListViewModel
    {
        // for paging
        public string SortOrder { get; set; }
        public string CurrentFilter { get; set; }
        public int? PageNumber { get; set; }

        //
        public string SearchString { get; set; }

        public int SelectedBrand { get; set; }
        public int SelectedCategory { get; set; }
        public short SelectedYear { get; set; }
        public List<Product> Products { get; set; }
        public IEnumerable<SelectListItem> Brands { get; set; }
        public IEnumerable<SelectListItem> Categories { get; set; }
        public IEnumerable<SelectListItem> ModelYears { get; set; }

        //*******************************************************
    }

}

