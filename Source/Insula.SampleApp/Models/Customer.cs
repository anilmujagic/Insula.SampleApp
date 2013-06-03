using Insula.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insula.SampleApp.Models
{
    public class Customer
    {
        [Mapped]
        [Key]
        [Identity]
        public int CustomerID { get; set; }
        [Mapped]
        public string Name { get; set; }
        [Mapped]
        public string City { get; set; }
        [Mapped]
        public string CountryID { get; set; }

        public Country Country { get; set; }
    }
}
