using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace StoreProject.Models
{
    public class CountryViewModel
    {
        //first try

        public IEnumerable<string> CountryCodes { get; set; }
        public List<SelectListItem> Countries { get; } = new List<SelectListItem>
        {
                new SelectListItem{Value = "Mx", Text = "Mexico"},
                new SelectListItem{Value = "CA", Text = "Canada"},
                new SelectListItem{Value = "US", Text = "USA"},
                new SelectListItem { Value = "FR", Text = "France" },
                new SelectListItem { Value = "ES", Text = "Spain" },
                new SelectListItem { Value = "DE", Text = "Germany"}
    };
        //*******************************************************

        //second try

        //public CountryEnum EnumCountry { get; set; }

        //public enum CountryEnum
        //{
        //    [Display(Name ="United Mexican States")]
        //    Mexico,
        //    [Display(Name ="United States of American")]
        //    USA,
        //    Canada,
        //    France,
        //    Germany,
        //    Spain
        //}
        //************************************************

        [MinLength(5)]
        [MaxLength(1024)]
        public string Description { get; set; }

        //*************************************
        [Required]
        [EmailAddress]
        [Display(Name ="Email Address")]
        public string Email { get; set; }

        //***************************************
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool IsCkecked { get; set; }


    }
}
