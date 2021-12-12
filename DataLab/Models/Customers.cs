using DataLab.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DataLab.Models
{
    public class Customers
    {
        [Key]
        public int CustomerId { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string CustomerName { get; set; }

        [Column(TypeName = "nvarchar(20)")]
        public Enum_DataSource? DataSource { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string Address { get; set; }

        public virtual ICollection<CollectedData> CollectedData { get; set; }
        public virtual ICollection<AuthorizedUsers> AuthorizedUsers { get; set; }
    }
}
