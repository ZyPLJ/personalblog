using Microsoft.EntityFrameworkCore;
using Personalblog.Model.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Personalblog.Model
{
    public class MyDbContext:DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) { }
        public DbSet<Category> categories { get; set; }
        public DbSet<Post> posts { get; set; }
        public DbSet<Photo> photos { get; set; }
        public DbSet<FeaturedPhoto> featuredPhotos { get; set; }
        public DbSet<FeaturedCategory> featuredCategories { get; set; }
        public DbSet<TopPost> topPosts { get; set; }
        public DbSet<FeaturedPost> featuredPosts { get; set; }
        public DbSet<User> users { get; set; }
        public DbSet<VisitRecord> visitRecords { get; set; }
        public DbSet<ConfigItem> configItems { get; set; } 
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(
                this.GetType().Assembly);
        }
    }
}
