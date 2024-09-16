﻿using Microsoft.EntityFrameworkCore;
using ProductStoreAPI.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductStoreAPI.Infrastructure.Context
{
    public class ProductStoreDbContext : DbContext
    {
        public ProductStoreDbContext(DbContextOptions<ProductStoreDbContext> options) : base(options) { }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }
    }
}