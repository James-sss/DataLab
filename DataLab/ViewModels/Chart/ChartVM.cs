using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataLab.ViewModels.Chart
{
    public class ChartVM
    {
        public string Month { get; set; }
        public string SensorType { get; set; }
        public decimal AverageValues { get; set; }
        public decimal MinimumValues { get; set; }
        public decimal MaxmumValues { get; set; }
        public int TotalFilesCount { get; set; }
    }
}
