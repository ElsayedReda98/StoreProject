using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace StoreProject.Models
{
    public class ProductViewModel
    {
        
        
        public string BrandName { get; set; }
        public List<SelectListItem> Brands { get; set; }

        public string CategoryName { get; set; }
        public List<SelectListItem> Categories { get; set; }
        public SelectList MYears { get; set; }
        
        
        

        public string ProductName { get; set; }
        public short ModelYear { get; set; }
        public decimal ListPrice { get; set; }





    }
}
