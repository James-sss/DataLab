using DataLab.DataManager;
using DataLab.Enums;
using DataLab.Models;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DataLab.SeedHandler
{
    public class SeedInitializer : ISeedInitializer
    {
        private readonly DataLabDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public SeedInitializer(DataLabDbContext dbContext,
                               UserManager<ApplicationUser> userManager,
                               RoleManager<IdentityRole> roleManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public void ExecuteSeed()
        {

            var user = new ApplicationUser
            {
                Id = "dc5f2a8a-7abb-4b99-860c-314a0b86138f",
                FirstName = "Guest",
                LastName = "User",
                Email = "GuestUser@gmail.com",
                AccountType = (Enum_AccountType?)1,
                NormalizedEmail = "GUESTUSER@GMAIL.COM",
                UserName = "GuestUser@gmail.com",
                NormalizedUserName = "GUESTUSER@GMAIL.COM",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString("D"),
                LockoutEnabled = false,
            };


            if (!_dbContext.Roles.Any(r => r.Name == "Role_Admin"))
            {
                _roleManager.CreateAsync(new IdentityRole { Id = "59545597-4e90-486f-a8e6-a9c5ba642bc8", Name = "Role_Admin", NormalizedName = "ROLE_ADMIN" }).Wait();
            }

            if (!_dbContext.Roles.Any(r => r.Name == "Role_SuperAdmin"))
            {
                _roleManager.CreateAsync(new IdentityRole { Id = "17f58c5c-c490-45d2-9a73-62941088b476", Name = "Role_SuperAdmin", NormalizedName = "ROLE_SUPERADMIN" }).Wait();
            }


            if (!_dbContext.Users.Any(u => u.UserName == user.UserName))
            {
                var password = new PasswordHasher<ApplicationUser>();
                var hashed = password.HashPassword(user, "D-558c");
                user.PasswordHash = hashed;

                var result = _userManager.CreateAsync(user).Result;

                if (result.Succeeded)
                {
                    _userManager.AddToRoleAsync(user, "Role_Admin").Wait();
                    _userManager.AddToRoleAsync(user, "Role_SuperAdmin").Wait();
                }

            }


            if (!_dbContext.Customers.Any())
            {
                var Data = JsonConvert.DeserializeObject<List<Customers>>(File.ReadAllText(@"Seed" + Path.DirectorySeparatorChar + "Customers.json"));
                _dbContext.AddRangeAsync(Data).Wait();
                _dbContext.SaveChangesAsync().Wait();
            }


            if (!_dbContext.AuthorizedUsers.Any())
            {
                var Data = JsonConvert.DeserializeObject<List<AuthorizedUsers>>(File.ReadAllText(@"Seed" + Path.DirectorySeparatorChar + "AuthorizedUsers.json"));
                _dbContext.AddRangeAsync(Data).Wait();
                _dbContext.SaveChangesAsync().Wait();

            }


            if (!_dbContext.SensorTypes.Any())
            {
                var Data = JsonConvert.DeserializeObject<List<SensorTypes>>(File.ReadAllText(@"Seed" + Path.DirectorySeparatorChar + "SensorTypes.json"));
                _dbContext.AddRangeAsync(Data).Wait();
                _dbContext.SaveChangesAsync().Wait();
            }


            if (!_dbContext.CollectedData.Any())
            {
                var Data = JsonConvert.DeserializeObject<List<CollectedData>>(File.ReadAllText(@"Seed" + Path.DirectorySeparatorChar + "CollectedData.json"));
                _dbContext.AddRangeAsync(Data).Wait();
                _dbContext.SaveChangesAsync().Wait();
            }
        }
    }
}
