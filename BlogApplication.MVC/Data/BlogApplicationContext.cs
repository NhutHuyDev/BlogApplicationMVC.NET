using BlogApplication.MVC.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace BlogApplication.MVC.Data
{
    public class BlogApplicationContext : IdentityDbContext
    {
        public BlogApplicationContext (DbContextOptions<BlogApplicationContext> options) 
            : base(options)
        {
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Page> Pages { get; set; }
    }
}
