using Microsoft.AspNetCore.Mvc.Rendering;

namespace StoreProject.Models
{
    public class CountryViewModel
    {
        public string Country { get; set; }
        public List<SelectListItem> Countries { get; } = new List<SelectListItem>
        {
                new SelectListItem{Value = "Mx", Text = "Mexico"},
                new SelectListItem{Value = "CA", Text = "Canada"},
                new SelectListItem{Value = "US", Text = "USA"},
        };

    }
}
