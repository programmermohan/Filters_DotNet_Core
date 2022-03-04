using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCore_Filters.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions dbContextOptions)
        : base(dbContextOptions)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserLogin> UserLogins { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<TokenHistory> TokenHistories { get; set; }
        public DbSet<LogDetails> LogDetails { get; set; }
        public DbSet<LogErrors> LogErrors { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //create a default role and default user with manager role
            modelBuilder.Entity<Role>().HasData(new Role
            {
                Id = 1,
                RoleName = "Manager",
            });

            modelBuilder.Entity<User>().HasData(new User
            {
                Id = 1,
                UserName = "MohanChandra",
                Password = "Mohan@123",
                RoleId = 1
            });
        }
    }
}
