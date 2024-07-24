using Microsoft.EntityFrameworkCore;
using PointOfSale.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PointOfSale
{
    public class POSDbContext : DbContext
    {
        public POSDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Product> Products { get; set; }
        public static void SeedData(POSDbContext context)
        {
            if (!context.Users.Any())
            {
                context.Users.AddRange(
                    new User { Id = 1, name = "admin", email = "email", password = "adminpass", role = "Admin" }

                );
                context.SaveChanges();
            }
        }
        public DbSet<User> Users { get; set; }
        public DbSet<SaleProducts> SaleProducts { get; set; } 

    }
}
