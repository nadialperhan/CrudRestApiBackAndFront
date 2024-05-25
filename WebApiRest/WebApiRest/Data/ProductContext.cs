using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiRest.Data
{
    public class ProductContext : DbContext
    {
        public ProductContext(DbContextOptions<ProductContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Category>().HasData(new Category[] {
                new Category{ Id=1,Name="Elektronik"},
                new Category{Id=2,Name="Donanim"}
            });

            modelBuilder.Entity<Product>().Property(x => x.Price).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<Product>().HasData(new Product[] {
                new Product{Id=1,Name="Bilgisayar",CreatedDate=DateTime.Now,Price=1200,Stock=300,CategoryId=1},
                new Product{Id=2,Name="Telefon",CreatedDate=DateTime.Now.AddDays(-15),Price=1500,Stock=350,CategoryId=1},
                new Product{Id=3,Name="Klavye",CreatedDate=DateTime.Now.AddDays(-60),Price=1800,Stock=1000,CategoryId=2}
            });
            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}
