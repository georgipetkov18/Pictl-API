using Microsoft.EntityFrameworkCore;
using PictlData.Models;
using System.Collections.Generic;

namespace PictlData
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Album> Albums { get; set; }
        public DbSet<CategoryPhoto> CategoryPhotos { get; set; }
        public IEnumerable<Photo> Where { get; internal set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CategoryPhoto>().HasKey(sc => new { sc.CategoryId, sc.PhotoId });
        }
    }
}
