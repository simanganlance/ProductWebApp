using ProductApi.Models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ProductApi.Repositories
{
    public class ProductDbContext : DbContext
    {
        public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }

    }
}
