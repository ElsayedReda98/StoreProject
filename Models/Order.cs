namespace StoreProject.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        
        public byte OrderStatus { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime RequiredDate { get; set; }
        public DateTime? ShippedDate { get; set; }
        
        // relationship
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        public int StoreId { get; set; }
        public Store Store { get; set; }
        public int StaffId { get; set; }
        public Staff Staff { get; set; }

        // rsh between order and orderitem
        // one to one
        public OrderItem OrderItem { get; set; }


    }
}
