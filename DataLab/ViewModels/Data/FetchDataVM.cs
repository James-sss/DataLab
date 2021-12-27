using DataLab.Enums;
using DataLab.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataLab.ViewModels.Data
{
    public class FetchDataVM
    {
        
        public string CustomerName { get; set; }
        public Enum_DataSource? DataSource { get; set; }
        public string Address { get; set; }
        public IEnumerable<CollectedData> CollectedData { get; set; }
    }
}
