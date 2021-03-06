using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DataLab.Models
{
    public class CollectedData
    {
        [Key]
        public int Id { get; set; }

        public int CustomerId { get; set; }

        public int SensorTypeId { get; set; }

        [ForeignKey("CustomerId")]
        public virtual Customers Customers { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string Location { get; set; }

        [Column(TypeName = "Datetime")]
        public DateTime? Datetime { get; set; }

        [ForeignKey("SensorTypeId")]
        public virtual SensorTypes SensorTypes { get; set; }

        [Column(TypeName = "decimal(5,1)")]
        public decimal Value { get; set; }
    }
}
