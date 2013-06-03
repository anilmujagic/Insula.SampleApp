using Insula.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insula.SampleApp.Models
{
    public class Country
    {
        [Mapped]
        [Key]
        public string CountryID { get; set; }
        [Mapped]
        public string Name { get; set; }
    }
}
