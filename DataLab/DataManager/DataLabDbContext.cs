using DataLab.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataLab.DataManager
{
    public class DataLabDbContext: IdentityDbContext<ApplicationUser>
    {
        public DataLabDbContext(DbContextOptions<DataLabDbContext> options) : base(options)
        {

        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Customers> Customers { get; set; }
        public DbSet<CollectedData> CollectedData { get; set; }
        public DbSet<AuthorizedUsers> AuthorizedUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>()
               .Property(u => u.FullName)
               .HasComputedColumnSql("[FirstName] + ' ' + [LastName]");

            modelBuilder.Entity<CollectedData>()
               .HasKey(c => new
               {
                   c.Id
               });

            modelBuilder.Entity<Customers>()
                  .HasKey(c => new
                  {
                      c.CustomerId
                  });

            modelBuilder.Entity<AuthorizedUsers>()
                  .HasKey(c => new
                  {
                      c.CustomerId
                  });

        }
    }
}
