using DataLab.Enums;
using DataLab.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DataLab.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Column(TypeName = "nvarchar(50)")]
        public string FirstName { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string LastName { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string FullName { get; private set; }

        [Column(TypeName = "nvarchar(20)")]
        public Enum_AccountType? AccountType { get; set; }


        public virtual ICollection<AuthorizedUsers> AuthorizedUsers { get; set; }
    }
}
