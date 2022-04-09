using System.ComponentModel.DataAnnotations;

namespace StoreProject.ViewModels.Stores
{
    public class StoreCreateViewModel
    {
        
        [StringLength(255),Required]
        [Display(Name ="Store Name")]
        public string StoreName { get; set;}
        
        [StringLength(25),Required]
        public string Phone { get; set; }
        
        [Required]
        [StringLength(255),EmailAddress]
        public string Email { get; set; }
        
        [StringLength (255)]
        public string Street { get; set; }
        
        [StringLength(255)]
        public string City { get; set; }

        [StringLength(10)]
        public string State { get; set; }

        [Required]
        [StringLength(5)]
        [Display(Name ="Zip Code")]
        public string ZipCode { get; set; }
    }
}
