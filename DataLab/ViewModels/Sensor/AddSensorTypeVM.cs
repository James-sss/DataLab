using DataLab.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DataLab.ViewModels.Sensor
{
    public class AddSensorTypeVM
    {
        [Required]
        public string SensorType { get; set; }

        public IEnumerable<SensorTypes> ListSensorTypes { get; set; }
    }
}
