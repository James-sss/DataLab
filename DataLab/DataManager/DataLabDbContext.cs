using FarmApplication.Models;
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>()
               .Property(u => u.FullName)
               .HasComputedColumnSql("[FirstName] + ' ' + [LastName]");

        }
    }
}
