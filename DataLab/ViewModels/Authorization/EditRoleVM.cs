using DataLab.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DataLab.ViewModels.Authorization
{
    public class EditRoleVM
    {
        
        public string Id { get; set; }
        [Required(ErrorMessage = "Role Name is required")]
        public string RoleName { get; set; }

        public IEnumerable<ApplicationUser> UsersInRoleList { get; set; }
    }
}
