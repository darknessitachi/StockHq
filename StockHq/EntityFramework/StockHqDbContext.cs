using Microsoft.Data.Entity;
using StockHq.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockHq.EntityFramework
{
    public class StockHqDbContext : DbContext
    {
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }
        /*
             protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql()
            base.OnConfiguring(optionsBuilder);
        }
            */

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Make Blog.Url required
            modelBuilder.Entity<Blog>()
                        .Property(b => b.Url)
                        .IsRequired();
        }
    }
}
