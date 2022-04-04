using System.ComponentModel.DataAnnotations;

namespace StoreProject.Models
{
    public class Brand
    {
        public int BrandId { get; set; }
        
        [Required]
        [Display(Name ="Brand Name" )]
        [StringLength(50, MinimumLength = 3)]
        public string BrandName { get; set; }

        // relationship
        public ICollection<Product> Products { get; set; }

    }
}
