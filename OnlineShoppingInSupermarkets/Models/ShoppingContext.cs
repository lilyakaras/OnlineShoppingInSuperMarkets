using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShoppingInSupermarkets.Models
{
    public class ShoppingContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<SuperMarket> SuperMarkets { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        public ShoppingContext(DbContextOptions<ShoppingContext> options)
            : base(options)
        {
           Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            string adminRoleName = "admin";
            string userRoleName = "user";
            string buyerRoleName = "buyer";

            string adminEmail = "admin@gmail.com";
            string adminPassword = "123456";

            Role adminRole = new Role { Id = 1, Name = adminRoleName };
            Role userRole = new Role { Id = 2, Name = userRoleName };
            Role buyerRole = new Role { Id = 3, Name = buyerRoleName };

            User adminUser = new User { Id = 1, Email = adminEmail, Password = adminPassword, RoleId = adminRole.Id };
            User regularBuyerUser = new User { Id = 2, Email = "buyer@gmail.com", Password = "buyer", RoleId = buyerRole.Id, Type = BuyerType.Regular };
            User goldenBuyerUser = new User { Id = 3, Email = "golden_buyer@gmail.com", Password = "goldenbuyer", RoleId = buyerRole.Id, Type = BuyerType.Golden };

            modelBuilder.Entity<Role>().HasData(new Role[] { adminRole, userRole, buyerRole });
            modelBuilder.Entity<User>().HasData(new User[] { adminUser, regularBuyerUser, goldenBuyerUser });
            base.OnModelCreating(modelBuilder);
        }
    }
}
