using DataLab.Enums;
using DataLab.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DataLab.ViewModels.Authentication
{
    public class RegisterVM
    {
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [Remote(action: "CheckEmailAvailability", controller: "Account")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "AccountType")]
        public Enum_AccountType? AccountType { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "Password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        //return List of registered users
        public IEnumerable<ApplicationUser> ApplicationUserList { get; set; }
    }
}
