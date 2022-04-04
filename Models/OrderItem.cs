namespace StoreProject.Models
{
    public class OrderItem
    {
        public int OrderId { get; set; }
        public int itemId { get; set; }
        
        public int Quantity { get; set; }
        public decimal ListPrice { get; set; }
        public decimal Discount { get; set; }


        // rsh between order and orderitem
        // one to one
        public Order Order { get; set; }

        // rshp between orderitem and products
        // orderitem is many
        // product is one
        public int ProductId { get; set; }
        public Product Product { get; set; }

    }
}
