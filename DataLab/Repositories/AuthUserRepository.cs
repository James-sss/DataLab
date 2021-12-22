using DataLab.DataManager;
using DataLab.IServices;
using DataLab.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataLab.Repositories
{
    public class AuthUserRepository : IAuthUserService
    {
        private readonly DataLabDbContext _context;

        public AuthUserRepository(DataLabDbContext context)
        {
            _context = context;
        }

        public async Task<AuthorizedUsers> AddAuthUsers(AuthorizedUsers users)
        {
            await _context.AuthorizedUsers.AddAsync(users);
            await _context.SaveChangesAsync();
            return users;
        }

        public IEnumerable<AuthorizedUsers> GetAuthUsersList(int CustomerId)
        {
            return _context.AuthorizedUsers.Where(u => u.CustomerId == CustomerId).Include(u => u.ApplicationUser).ToList();
        }

        public bool IsUserAssigned(ApplicationUser user, int CustomerId)
        {
            return _context.AuthorizedUsers.Any(e => e.ApplicationUser.Id == user.Id && e.CustomerId == CustomerId);
        }

        public async Task<AuthorizedUsers> RemoveAuthUsers(AuthorizedUsers users)
        {
            _context.AuthorizedUsers.RemoveRange(users);
            await _context.SaveChangesAsync();
            return users;
        }
    }
}
