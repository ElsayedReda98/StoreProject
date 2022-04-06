using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace StoreProject.ViewModels
{
    public class StaffCreateViewModel
    {
        [Required]
        [Display(Name = "First Name")]
        [StringLength(50, MinimumLength = 3)]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [StringLength(50, MinimumLength = 3)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(50, MinimumLength = 3)]
        public string Email { get; set; }

        //[StringLength(14, MinimumLength = 11)]
        [DataType(DataType.PhoneNumber)]
        
        public string Phone { get; set; }

        [Display(Name = "Store ")]
        public int Store { get; set; }
        
        [Display(Name = "Manager ")]
        public int Manager { get; set; }

        [Required]
        public bool Active { get; set; }

        public IEnumerable<SelectListItem> Stores { get; set; }
        public IEnumerable<SelectListItem> Managers { get; set; }





    }
}
