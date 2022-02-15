using Microsoft.AspNetCore.Mvc.Rendering;

namespace StoreProject.Models
{
    public class CountryViewModelGroup
    {
        public CountryViewModelGroup()
        {
            var NorthAmericaGroup = new SelectListGroup { Name = "North American" };
            var EuropeGroup = new SelectListGroup { Name = "Europe" };

            Countries = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Value ="MEX",
                    Text ="Mexico",
                    Group = NorthAmericaGroup,
                },
                new SelectListItem
                {
                    Value="CAN",
                    Text="Canada",
                    Group= NorthAmericaGroup,
                },
                new SelectListItem
                {
                    Value ="US",
                    Text = "USA",
                    Group = NorthAmericaGroup
                },
                new SelectListItem
                {
                    Value ="Fr",
                    Text="France",
                    Group= EuropeGroup
                },
                new SelectListItem
                {
                    Value ="ES",
                    Text ="Spain",
                    Group = EuropeGroup
                },
                new SelectListItem
                {
                    Value="DE",
                    Text ="Germany",
                    Group= EuropeGroup
                }
            };
           
        }

        public string Country { get; set; }
        public List<SelectListItem> Countries { get; set; }
    }
}
