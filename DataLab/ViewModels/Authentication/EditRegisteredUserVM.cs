using DataLab.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DataLab.ViewModels.Authentication
{
    public class EditRegisteredUserVM
    {
        public string Id { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
       
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Display(Name = "AccountType")]
        public Enum_AccountType? AccountType { get; set; }
    }
}
