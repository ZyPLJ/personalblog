using Microsoft.EntityFrameworkCore;
using Personalblog.Model.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Personalblog.Model.ViewModels.Arc;

namespace Personalblog.Model
{
    public class MyDbContext : DbContext
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
        public DbSet<Link> links { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Notice> notice { get; set; }
        public DbSet<LinkExchange> LinkExchanges { get; set; }
        public DbSet<Messages> Messages { get; set; }
        public DbSet<Replies> Replies { get; set; }
        public DbSet<AnonymousUser> AnonymousUsers { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<PostTag> PostTags { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Post>()
                .HasMany(e => e.Tags)
                .WithMany(e => e.Posts)
                .UsingEntity<PostTag>();

            // 禁用外键约束
            modelBuilder.Entity<Post>()
                .HasOne(p => p.Categories)
                .WithMany()
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Post>()
                .HasIndex(p => p.ViewCount);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Parent)
                .WithMany(c => c.Comments)
                .HasForeignKey(c => c.ParentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Post)
                .WithMany(p => p.Comments)
                .HasForeignKey(c => c.PostId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.AnonymousUser)
                .WithMany()
                .HasForeignKey(c => c.AnonymousUserId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Category>()
                .HasOne(c => c.Parent)
                .WithMany()
                .HasForeignKey(c => c.ParentId)
                .IsRequired(false);
            modelBuilder.Entity<Category>()
                .Property(c => c.ParentId)
                .ValueGeneratedNever();

            modelBuilder.Entity<Category>()
                .HasMany(c => c.Posts)
                .WithOne(p => p.Categories)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Category>()
                .Property(c => c.Visible)
                .HasDefaultValue(true);

            modelBuilder.ApplyConfigurationsFromAssembly(
                this.GetType().Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}
