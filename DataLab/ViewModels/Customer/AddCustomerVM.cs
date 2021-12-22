using DataLab.Enums;
using DataLab.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DataLab.ViewModels.Customer
{
    public class AddCustomerVM
    {
        [Required]
        [Display(Name = "Customer")]
        public string CustomerName { get; set; }

        [Required]
        [Display(Name = "Data source")]
        public Enum_DataSource? DataSource { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Address")]
        public string Address { get; set; }

        public IEnumerable<Customers> CustomerList { get; set; }
    }
}
