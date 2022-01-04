using DataLab.Enums;
using DataLab.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataLab.ViewModels.Accsess
{
    public class GetResourceVM
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public Enum_DataSource? DataSource { get; set; }
        public IEnumerable<CollectedData> CollectedData { get; set; }
        public IEnumerable<ApplicationUser> AuthUsers { get; set; }
    }
}
