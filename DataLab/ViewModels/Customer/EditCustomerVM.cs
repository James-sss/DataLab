using DataLab.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataLab.ViewModels.Customer
{
    public class EditCustomerVM : AddCustomerVM
    {
        public int CustomerId { get; set; }
        public IEnumerable<AuthorizedUsers> AuthorizedUsersList { get; set; }
    }
}
