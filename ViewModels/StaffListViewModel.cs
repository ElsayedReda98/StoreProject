using Microsoft.AspNetCore.Mvc.Rendering;
using StoreProject.Models;
using X.PagedList;

namespace StoreProject.ViewModels
{
    public class StaffListViewModel
    {
        public string NameSearch { get; set; }
        public string EmailSearch { get; set; }
        public string PhoneSearch { get; set; }
        public bool? ActiveBox { get; set; } 
        public int SelectedManager { get; set; }
        public int SelectedStore { get; set; }
        public int PageNumber { get; set; } = 1;
        public IPagedList<Staff> Staffs { get; set; }
        public IEnumerable<SelectListItem> Managers { get; set; }
        public List<Staff> ActiveList { get; set; }
        public IEnumerable<SelectListItem> Stores { get; set; }


    }
}
