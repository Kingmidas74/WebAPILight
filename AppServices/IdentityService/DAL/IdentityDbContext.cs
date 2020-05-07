using System;
using Microsoft.EntityFrameworkCore;

namespace IdentityService {
    public class AppDbContext : DbContext {
        public AppDbContext (DbContextOptions<AppDbContext> options) : base (options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Identity> Identities { get; set; }
    }

    public class User {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public string Phone { get; set; }
    }

    public class Identity {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public string Phone { get; set; }
        public string Code {get;set;}
    }
}