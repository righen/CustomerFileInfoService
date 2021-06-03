using CustomerFileInfoService.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerFileInfoService
{
    public class CustomerFileInfoDbContext : DbContext
    {
        public CustomerFileInfoDbContext(DbContextOptions<CustomerFileInfoDbContext> options): base(options)
        {
        }

        public DbSet<UserNotificationPath> UserNotificationPaths { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserNotificationPath>().ToTable("UserNotificationPath");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging(true);
        }
    }
}
