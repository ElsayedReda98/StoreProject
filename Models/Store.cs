namespace StoreProject.Models
{
    public class Store
    {
        public int StoreId { get; set; }
        public string StoreName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCodde { get; set; }

        // relationship
        public ICollection<Staff> Staffs { get; set; }

        public Stock Stock { get; set; }

        public ICollection<Order> Orders { get; set; }

    }
}
