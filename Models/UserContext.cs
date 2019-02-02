using System;
using Microsoft.EntityFrameworkCore;

namespace wall.Models
{
    public class UserContext : DbContext
    {
        public DbSet<User> user { get; set; }
        public DbSet<Messages> messages { get; set; }
        public DbSet<Comments> comments { get; set; }
        public UserContext(DbContextOptions<UserContext> options) : base(options) { }
    }
}