namespace StoreProject.Models
{
    public class Stock
    {
        public int StoreId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        // rshp between stock and products
        // stock is one and zero
        // product is one and only one
        public Product Product { get; set; }

        // rshp between stock and stores
        // stock is one and zero
        // store is one and only one
        public Store Store { get; set; }

        
    }
}
