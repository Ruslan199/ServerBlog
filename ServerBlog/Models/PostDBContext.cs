
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerBlog.Models
{
    public class PostDBContext : DbContext
    {
        public DbSet<Post> Posts { get; set; }
        public DbSet<User> Users { get; set; }
        public PostDBContext(DbContextOptions<PostDBContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
