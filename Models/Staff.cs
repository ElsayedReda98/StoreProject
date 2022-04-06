using System.ComponentModel.DataAnnotations;

namespace StoreProject.Models
{
    public class Staff
    {
        public int StaffId { get; set; }

        [Required]
        [Display(Name = "First Name")]
        [StringLength(50, MinimumLength = 3)]
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



        ////one to many relationship between staffs and stores
        //staff is many and stores is one

        [Display(Name = "Store ")]
        public int? StoreId { get; set; }
        public Store Store { get; set; }

        [Display(Name = "Manager ")]
        public int? ManagerId { get; set; }
        public Staff Manager { get; set; }

        public ICollection<Staff> Managers { get; set; }

        //rsh
        public ICollection<Order> Orders { get; set; }



    }
}