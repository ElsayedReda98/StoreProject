using System.ComponentModel.DataAnnotations;

namespace StoreProject.Models
{
    public class Store
    {
        public int StoreId { get; set; }
        [Display(Name ="Store Name")]
        public string StoreName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }

        [Display(Name ="Zip Code")]
        public string ZipCode { get; set; }

        // relationship
        public ICollection<Staff> Staffs { get; set; }

        public Stock Stock { get; set; }

        public ICollection<Order> Orders { get; set; }

    }
}
