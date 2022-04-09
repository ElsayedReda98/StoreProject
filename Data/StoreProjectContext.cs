#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StoreProject.Models;

namespace StoreProject.Data
{
    public class StoreProjectContext : DbContext
    {
        public StoreProjectContext(DbContextOptions<StoreProjectContext> options)
            : base(options)
        {
        }



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"server=.;Database=BikeStores;Trusted_Connection=true;");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>(builder =>
            {
                builder.ToTable("categories", "production");
                builder.Property(c => c.CategoryId).HasColumnName("category_id");
                builder.Property(c => c.CategoryName).HasColumnName("category_name");
            });

            modelBuilder.Entity<Brand>(builder =>
            {
                builder.ToTable("brands", "production");
                builder.Property(b => b.BrandId).HasColumnName("brand_id");
                builder.Property(b => b.BrandName).HasColumnName("brand_name");
            });
            //modelBuilder.Entity<Product>()
            //    .HasKey(pk => pk.ProductId);
            
            modelBuilder.Entity<Product>(builder =>
            {
                
                builder.ToTable("products", "production");
                builder.Property(p => p.ProductId).HasColumnName("product_id");
                builder.Property(p => p.ProductName).HasColumnName("product_name");
                builder.Property(p => p.BrandId).HasColumnName("brand_id");
                builder.Property(p => p.CategoryId).HasColumnName("category_id");
                builder.Property(p => p.ModelYear).HasColumnName("model_year");
                builder.Property(p => p.ListPrice).HasColumnName("list_price");
            });

            //pk for stock table
            modelBuilder.Entity<Stock>()
                .HasKey(pk => new { pk.StoreId, pk.ProductId });

            modelBuilder.Entity<Stock>(builder =>
            {
                builder.ToTable("stocks", "production");
                builder.Property(s => s.StoreId).HasColumnName("store_id");
                builder.Property(s => s.ProductId).HasColumnName("product_id");
                builder.Property(s => s.Quantity).HasColumnName("quantity");

            });

            modelBuilder.Entity<Customer>(builder =>
            {
                builder.ToTable("customers", "sales");
                builder.Property(c => c.CustomerId).HasColumnName("customer_id");
                builder.Property(c => c.FirstName).HasColumnName("first_name");
                builder.Property(c => c.LastName).HasColumnName("last_name");
                builder.Property(c => c.Phone).HasColumnName("phone");
                builder.Property(c => c.Email).HasColumnName("email");
                builder.Property(c => c.Street).HasColumnName("street");
                builder.Property(c => c.City).HasColumnName("city");
                builder.Property(c => c.State).HasColumnName("state");
                builder.Property(c => c.ZipCode).HasColumnName("zip_code");
            });

            modelBuilder.Entity<Order>(builder =>
            {
                builder.ToTable("orders", "sales");
                builder.Property(o => o.OrderId).HasColumnName("order_id");
                builder.Property(o => o.CustomerId).HasColumnName("customer_id");
                builder.Property(o => o.OrderStatus).HasColumnName("order_status");
                builder.Property(o => o.OrderDate).HasColumnName("order_date");
                builder.Property(o => o.RequiredDate).HasColumnName("required_date");
                builder.Property(o => o.ShippedDate).HasColumnName("shipped_date");
                builder.Property(o => o.StoreId).HasColumnName("store_id");
                builder.Property(o => o.StaffId).HasColumnName("staff_id");

            });

            modelBuilder.Entity<Staff>(builder =>
            {
                builder.ToTable("staffs", "sales");
                builder.Property(s => s.StaffId).HasColumnName("staff_id");
                builder.Property(s => s.FirstName).HasColumnName("first_name");
                builder.Property(s => s.LastName).HasColumnName("last_name");
                builder.Property(s => s.Email).HasColumnName("email");
                builder.Property(s => s.Phone).HasColumnName("phone");
                builder.Property(s => s.Active).HasColumnName("active");
                builder.Property(s => s.StoreId).HasColumnName("store_id");
                builder.Property(s => s.ManagerId).HasColumnName("manager_id");
            });

            

            modelBuilder.Entity<Store>(builder =>
            {
                builder.ToTable("stores", "sales");
                builder.Property(o => o.StoreId).HasColumnName("store_id");
                builder.Property(o => o.StoreName).HasColumnName("store_name");
                builder.Property(o => o.Phone).HasColumnName("phone");
                builder.Property(o => o.Email).HasColumnName("email");
                builder.Property(o => o.Street).HasColumnName("street");
                builder.Property(o => o.City).HasColumnName("city");
                builder.Property(o => o.State).HasColumnName("state");
                builder.Property(o => o.ZipCode).HasColumnName("zip_code");
            });

            // pk for orderitem table
            modelBuilder.Entity<OrderItem>()
                .HasKey(pk => new { pk.itemId, pk.OrderId });

            modelBuilder.Entity<OrderItem>(builder =>
            {
                builder.ToTable("order_items", "sales");
                builder.Property(o => o.OrderId).HasColumnName("order_id");
                builder.Property(o => o.itemId).HasColumnName("item_id");
                builder.Property(o => o.ProductId).HasColumnName("product_id");
                builder.Property(o => o.Quantity).HasColumnName("quantity");
                builder.Property(o => o.ListPrice).HasColumnName("list_price");
                builder.Property(o => o.Discount).HasColumnName("discount");
            });

            //pk for stock table
            modelBuilder.Entity<Stock>()
                .HasKey(pk => new { pk.StoreId, pk.ProductId });

            modelBuilder.Entity<Stock>(builder =>
            {
                builder.ToTable("stocks", "production");
                builder.Property(s => s.StoreId).HasColumnName("store_id");
                builder.Property(s => s.ProductId).HasColumnName("product_id");
                builder.Property(s => s.Quantity).HasColumnName("QUANTITY");
            });

            // one to many rsh between order and staff
            modelBuilder.Entity<Staff>()
                .HasMany<Order>(s => s.Orders)
                .WithOne(s => s.Staff)
                .HasForeignKey(s => s.StaffId);

            // one to many rsh between staff and staff
            modelBuilder.Entity<Staff>()
                .HasOne<Staff>(s => s.Manager)
                .WithMany(s => s.Managers)
                .HasForeignKey(s => s.ManagerId);


            //// one to many rsh between staff and staff
            //modelBuilder.Entity<Staff>()
            //    .HasMany<Staff>(s => s.Managers)
            //    .WithOne(s => s.Manager)
            //    .HasForeignKey(s => s.ManagerId);
                

            // one to many rsh between store and staff
            modelBuilder.Entity<Staff>()
                .HasOne<Store>(s => s.Store)
                .WithMany(s => s.Staffs)
                .HasForeignKey(s => s.StoreId);




        }
        public DbSet<StoreProject.Models.Category> Category { get; set; }
        public DbSet<StoreProject.Models.Brand> Brand { get; set; }
        public DbSet<StoreProject.Models.Product> Product { get; set; }
        public DbSet<StoreProject.Models.Customer> Customer { get; set; }
        public DbSet<StoreProject.Models.Order> Order { get; set; }
        public DbSet<StoreProject.Models.Staff> Staff { get; set; }
        public DbSet<StoreProject.Models.Store> Store { get; set; }
        public DbSet<StoreProject.Models.OrderItem> OrderItem { get; set; }
        public DbSet<StoreProject.Models.Stock> Stock { get; set; }
    } 

}

