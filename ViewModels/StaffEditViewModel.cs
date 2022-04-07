using Microsoft.AspNetCore.Mvc.Rendering;

namespace StoreProject.ViewModels
{
    public class StaffEditViewModel
    {
        public int StaffId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public bool Active { get; set; }
        public int Store { get; set; }
        public int Manager { get; set; }
        public IEnumerable<SelectListItem> Stores { get; set; }
        public IEnumerable<SelectListItem> Managers { get; set; }

    }
}
