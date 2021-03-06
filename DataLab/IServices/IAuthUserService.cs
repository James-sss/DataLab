using DataLab.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataLab.IServices
{
    public interface IAuthUserService
    {
        bool IsUserAssigned(ApplicationUser user, int CustomerId);
        Task<AuthorizedUsers> AddAuthUsers(AuthorizedUsers users);
        Task<AuthorizedUsers> RemoveAuthUsers(AuthorizedUsers users);
        IEnumerable<AuthorizedUsers> GetAuthUsersList(int CustomerId);
    }
}
