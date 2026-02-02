using CinemaMusicBlog.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace CinemaMusicBlog.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Veritabanındaki tablolarımız
        public DbSet<Category> Categories { get; set; }

        public DbSet<Admin> Admins { get; set; }
        public DbSet<Post> Posts { get; set; }

        public DbSet<PostItem> PostItems { get; set; }
    }
}