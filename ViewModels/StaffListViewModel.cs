using Microsoft.AspNetCore.Mvc.Rendering;
using StoreProject.Models;
using System.ComponentModel.DataAnnotations;
using X.PagedList;

namespace StoreProject.ViewModels
{
    public class StaffListViewModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public bool? Active { get; set; }
        public int Manager { get; set; }
        public int Store { get; set; }
        public int PageNumber { get; set; } = 1;
        public IPagedList<Staff> Staffs { get; set; }
        public IEnumerable<SelectListItem> Managers { get; set; }
        //public List<Staff> ActiveList { get; set; }
        public IEnumerable<SelectListItem> Stores { get; set; }


    }
}
