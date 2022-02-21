using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreProject.Models
{
    public class ProductViewModel
    {
        public List<Product>? Products { get; set; }
        
        public string? SearchString { get; set; }

        //****************************************
        public int ProductId { get; set; }
        public int? SelectedBrand { get; set; }
        public IEnumerable<SelectListItem>? Brands { get; set; }

        public int? SelectedCategory { get; set; }
        public IEnumerable<SelectListItem>? Categories { get; set; }
        public IEnumerable<SelectListItem> MYears { get; set; }

        public string BrandName { get; set; }
        public string CategoryName { get; set; }

        //******************
        [Display(Name ="Brand Name")]
        public int BrandId { get; set; }

        [Display(Name ="Category Name")]
        public int CategoryId { get; set; }

        // These properties for other fields in Views
        [Display(Name ="Product Name")]
        public string ProductName { get; set; }

        [Display(Name ="Model Year")]
        public short ModelYear { get; set; }
        
        [Display(Name ="List Price")]
        [Range(1,1000)]
        [Column(TypeName ="decimal(18,2)")]
        public decimal ListPrice { get; set; }





    }
}
