using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ShoppingApp.Models;

namespace ShoppingApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductOrder> ProductOrders { get; set; }
        public DbSet<OrdersHistory> OrdersHistories { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //seeding data 
            modelBuilder.Entity<Category>()
                .HasData(new Category[]
                {   //add primary key
                    new Category { Id = 1 , Name = "Dresses"},
                    new Category { Id = 2 , Name = "Tops"},
                    new Category { Id = 3 , Name = "Pants"},
                    new Category { Id = 4 , Name = "Accessories"},
                    new Category { Id = 5 , Name = "Sports"},
                    new Category { Id = 6 , Name = "Shoes"},
                }
                );
           
            modelBuilder.Entity<ProductOrder>()
                .HasKey(p => new { p.OrderId, p.ProductId });



            base.OnModelCreating(modelBuilder);
            modelBuilder.Seed();

        }



    }
}
