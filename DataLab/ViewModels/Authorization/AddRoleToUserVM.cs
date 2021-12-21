using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataLab.ViewModels.Authorization
{
    public class AddRoleToUserVM
    {
        public List<IdentityRole> RolesAssignedToUser { get; set; }
        public List<IdentityRole> RolesNotAssignedToUser { get; set; }

        public string UserId { get; set; }
        public string FullNames { get; set; }
        public List<string> ListRolesToAddORRemove { get; set; }

        public AddRoleToUserVM()
        {
            ListRolesToAddORRemove = new List<string>();
        }
    }
}
