using Gezo.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gezo.Infrastructure.Persistence
{
    public class StoreDbContext : DbContext
    {
        public StoreDbContext(DbContextOptions<StoreDbContext> options):base(options)
        {

        }
        public DbSet<Product> products { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Category>Categories { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }


    }
}
