using Gezo.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gezo.Infrastructure.Persistence
{
    public class StoreDbContext : IdentityDbContext<ApplicationUser>

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
