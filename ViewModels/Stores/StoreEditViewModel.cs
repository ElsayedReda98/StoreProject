using System.ComponentModel.DataAnnotations;

namespace StoreProject.ViewModels.Stores
{
    public class StoreEditViewModel
    {
        public int StoreId { get; set; }

        [Required]
        [Display(Name ="Store Name")]
        [StringLength(255)]
        public string StoreName { get; set; }
        
        [StringLength(25)]
        public string Phone { get; set; }
        
        [StringLength(255)]
        public string Email { get; set; }
        
        [StringLength(255)]
        public string Street { get; set; }
        
        [StringLength(255)]
        public string City { get; set; }

        [StringLength(10)]
        public string State { get; set; }
        
        [Display(Name ="Zip Code")]
        [StringLength (5),Required]
        public string ZipCode { get; set; }

    }
}
