using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReasonableOrderMVC.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Order> Orders { get; set; }

        public DbSet<Sale> Sales { get; set; }

        public DbSet<Product> Products { get; set; }
    }
}
