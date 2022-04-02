using System.ComponentModel.DataAnnotations;

namespace StoreProject.Models
{
    public class Staff
    {
        public int StaffId { get; set; }
        
        [Required]
        [Display(Name ="First Name")]
        [StringLength(50, MinimumLength =3)]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [StringLength(50, MinimumLength = 3)]
        public string LastName { get; set; }
        public string FullName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }
        
        [Required]
        [EmailAddress]
        [StringLength(50, MinimumLength = 3)]
        public string Email { get; set; }


        [StringLength(14, MinimumLength = 11)]
        [DataType(DataType.PhoneNumber)]
        [Phone]
        public string? Phone { get; set; }
        
        [Required]
        public byte Active { get; set; }

        [Required]
        [Display(Name = "Store Id")]
        public int StoreId { get; set; }

        
        [Display(Name = "Manager Id")]
        public int? ManagerId { get; set; }

        ////one to many relationship between staffs and stores
        //staff is many and stores is one
        public Store Store { get; set; }


        //public Staff StaffProperty { get; set; }

    }
}
