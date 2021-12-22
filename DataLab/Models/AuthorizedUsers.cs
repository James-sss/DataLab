using DataLab.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DataLab.Models
{
    public class AuthorizedUsers
    {
        public int CustomerId { get; set; }
        public string UserId { get; set; }

        [ForeignKey("CustomerId")]
        public virtual Customers Customers { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
