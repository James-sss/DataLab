using DataLab.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataLab.IServices
{
    public interface IAccessService
    {
        IEnumerable<AuthorizedUsers> ReturnResourcesByUserId(string userId);
        IEnumerable<CollectedData> GetData(int customerId);
        IEnumerable<ApplicationUser> ReturnAuthorizedUsers(int customerId);
    }
}
