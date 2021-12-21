using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DataLab.ViewModels.Authorization
{
    public class UserRolesVM
    {
        [Required(ErrorMessage = "Role Name is required")]
        public string RoleName { get; set; }

        //Return all List of roles
        public IEnumerable<IdentityRole> IdentityRole { get; set; }
    }
}
