using Microsoft.AspNetCore.Mvc.Rendering;

namespace StoreProject.Models
{
    public class ProductViewModel
    {
        public List<Product>? Products { get; set; }
        public SelectList? Brands { get; set; }
        public SelectList Categories { get; set; }
        public SelectList MYears { get; set; }
        public string? ProductName { get; set; }
        public int BrandId { get; set; }
        public int CategoryId { get; set; }
        public short ModelYear { get; set; }
        public decimal ListPrice { get; set; }



    }
}
