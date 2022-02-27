using Microsoft.AspNetCore.Mvc.Rendering;
using StoreProject.Models;

namespace StoreProject.ViewModels
{
    public class ProductListViewModel
    {
        public string SearchString { get; set; }
        public int SelectedBrand { get; set; }
        public int SelectedCategory { get; set; }
        public short SelectedYear { get; set; }
        public List<Product>? Products { get; set; }
        public IEnumerable<SelectListItem>? Brands { get; set; }
        public IEnumerable<SelectListItem>? Categories { get; set; }
        public IEnumerable<SelectListItem> ModelYears { get; set; }



    }
}
