using Microsoft.EntityFrameworkCore;
using BlogApi.Models;
using System.Collections.Generic;

namespace BlogApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Blog> blogs { get; set; }
        public DbSet<Service> services { get; set; }
        public DbSet<Gallery> gallery { get; set; }
        public DbSet<Contact> contact_info { get; set; }
        public DbSet<About> about { get; set; }
        public DbSet<User> users { get; set; }
    }
}
