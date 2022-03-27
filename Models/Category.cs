using System.ComponentModel.DataAnnotations;

namespace StoreProject.Models
{
    public class Category
    {
        public int CategoryId { get; set; }

        [Required]
        [Display(Name = "Category Name")]
        [StringLength(50, MinimumLength = 3)]
        public string CategoryName { get; set; }

    }
}
