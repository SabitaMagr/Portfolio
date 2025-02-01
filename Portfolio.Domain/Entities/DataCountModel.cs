using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portfolio.Domain.Entities
{
    public class DataCountModel
    {
        [Key]
        public int SortOrder { get; set; }
        public int Count { get; set; }
        public string Heading { get; set; }
        public string Color { get; set; }
        public string Icon { get; set; }

    }
}
