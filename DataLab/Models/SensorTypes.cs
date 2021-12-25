using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DataLab.Models
{
    public class SensorTypes
    {
        [Key]
        public int SensorTypeId { get; set; }

        [Column(TypeName = "nvarchar(20)")]
        public string SensorType { get; set; }

        public virtual ICollection<CollectedData> CollectedData { get; set; }
    }
}
