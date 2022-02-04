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
        public StoreProjectContext (DbContextOptions<StoreProjectContext> options)
            : base(options)
        {
        }

        public DbSet<StoreProject.Models.Category> Category { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"server=.;Database=BikeStores;Trusted_Connection=true;");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>(builder =>
            {
                builder.ToTable("categories", "production");
                builder.Property(c => c.Id).HasColumnName("category_id");
                builder.Property(c => c.CategoryName).HasColumnName("category_name");
            });
        }
    }
}
