using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreProject.Models
{
    public class Product
    {
        [Display(Name ="Product Id")]
        public int ProductId { get; set; }
        
        [Display(Name ="Product Name")]
        public string ProductName { get; set; }
        [Display(Name ="Brand Id")]
        public int BrandId { get; set; }
        [Display(Name ="Category Id")]
        public int CategoryId { get; set; }
        [Display(Name ="Model Year")]
        public short ModelYear { get; set; }
        
        [Display(Name ="List Price")]
        
        [Column(TypeName ="decimal(18,2)")]
        public decimal ListPrice { get; set; }

    }
}
