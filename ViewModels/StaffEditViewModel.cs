using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace StoreProject.ViewModels
{
    public class StaffEditViewModel
    {
        public int StaffId { get; set; }
        
        [Required]
        [StringLength(255)]
        [Display(Name ="First Name")]
        public string FirstName { get; set; }
        
        [Required]
        [StringLength(255)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        
        
        [Required,EmailAddress]
        [StringLength(255)]
        public string Email { get; set; }
        
        [StringLength(25)]
        public string Phone { get; set; }
        
        public bool Active { get; set; }
        
        [Display(Name = "Store")]
        [Required]
        public int StoreId { get; set; }
        [Display(Name = "Manager")]
        public int? ManagerId { get; set; }
        public IEnumerable<SelectListItem>? Stores { get; set; }
        public IEnumerable<SelectListItem> Managers { get; set; }

    }
}
