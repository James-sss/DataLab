using DataLab.Enums;
using DataLab.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataLab.ViewModels.Customer
{
    public class AssignUserToCustVM
    {
        public IEnumerable<ApplicationUser> ListUsersAssigned { get; set; }
        public IEnumerable<ApplicationUser> ListUsersNotAssigned { get; set; }

        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public Enum_DataSource? DataSource { get; set; }
    }
}
