using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace StoreProject.Models
{
    public class ProductViewModel
    {

        public int ProductId { get; set; }
        public string BrandName { get; set; }
        public IEnumerable<SelectListItem>? Brands { get; set; }

        public string CategoryName { get; set; }
        public IEnumerable<SelectListItem> Categories { get; set; }
        public IEnumerable<SelectListItem> MYears { get; set; }



        //******************
        public int BrandId { get; set; }
        public int CategoryId { get; set; }

        // These properties for other fields in View(Create.cshtml)
        public string ProductName { get; set; }
        public short ModelYear { get; set; }
        public decimal ListPrice { get; set; }





    }
}
