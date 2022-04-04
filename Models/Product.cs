using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreProject.Models
{
    public class Product
    {
        public int ProductId { get; set; }

        [Required]
        [Display(Name ="Product Name")]
        [StringLength(50, MinimumLength = 3)]
        public string ProductName { get; set; }
        
        [Display(Name ="Model Year")]
        public short ModelYear { get; set; }
        
        [Display(Name ="List Price")]
        [Range(0.1,1000)]
        public decimal ListPrice { get; set; }

        // RelationShips
        
        [Display(Name = "Brand")]
        public int BrandId { get; set; }
        public Brand Brand { get; set; }


        [Display(Name = "Category")]
        public int CategoryId { get; set; }
        public Category Category { get; set; }


        // rshp between orderitem and products
        // orderitem is many
        // product is one
        public ICollection<OrderItem> OrderItems { get; set; }

        // rshp between stocks and products
        // stock is one
        // product is one
        public Stock Stock { get; set; }
    }
}
