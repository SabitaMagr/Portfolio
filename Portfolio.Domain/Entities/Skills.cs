using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portfolio.Domain.Entities
{
    public partial class Skills
    {
        [Key]
        public int ID { get; set; }
        public string Skill  { get; set; }
        public long? Created_by {  get; set; }
        public long? Modified_by { get; set; }
        public DateTime? Created_dt { get; set; }
        public DateTime? Modified_dt { get; set; }
        public string Status { get; set; }
    }
}
