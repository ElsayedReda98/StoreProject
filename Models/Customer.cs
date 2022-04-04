using System.ComponentModel.DataAnnotations;

namespace StoreProject.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }
        
        [Required]
        [Display(Name ="First Name")]
        [StringLength(50, MinimumLength = 3)]
        public string FirstName { get; set; }
        
        [Required]
        [Display(Name ="Last Name")]
        [StringLength(50, MinimumLength =3)]
        public string LastName { get; set; }

        [StringLength(50, MinimumLength = 11)]
        public string? Phone { get; set; }

        [StringLength(50, MinimumLength = 15)]
        public string Email { get; set; }

        [StringLength(50, MinimumLength = 3)]
        public string Street { get; set; }
        
        [StringLength(50, MinimumLength = 3)]
        public string City { get; set; }
        
        public string State { get; set; }
        
        [Display(Name ="Zip Code")]
        [StringLength(50, MinimumLength = 5)]
        public string ZipCode { get; set; }

        public string FullName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }

        // relationship
        public ICollection<Order> Orders { get; set; }

    }
}
