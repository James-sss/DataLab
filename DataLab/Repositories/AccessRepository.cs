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
    public class AccessRepository : IAccessService
    {
        private readonly DataLabDbContext _context;

        public AccessRepository(DataLabDbContext context)
        {
            _context = context;
        }

        public IEnumerable<CollectedData> GetData(int customerId)
        {
            return _context.CollectedData.Where(e => e.CustomerId == customerId).Include(e => e.SensorTypes).ToList();
        }

        public IEnumerable<AuthorizedUsers> ReturnResourcesByUserId(string userId)
        {
            return _context.AuthorizedUsers.Where(e => e.ApplicationUser.Id == userId).Include(e => e.Customers).Include(e => e.ApplicationUser).ToList();
        }

        public IEnumerable<ApplicationUser> ReturnAuthorizedUsers(int customerId)
        {
            return _context.AuthorizedUsers.Where(e => e.CustomerId == customerId).Select(e => new ApplicationUser
            {
                Email = e.ApplicationUser.Email
            }).ToList();
        }
    }
}
