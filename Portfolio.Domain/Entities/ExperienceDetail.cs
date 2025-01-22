using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portfolio.Domain.Entities
{
    [Table("ExperienceDetails")]
    public class ExperienceDetail
    {
        [Key]
        public int Id { get; set; }
        public string? Position { get; set; }
        public string? Company { get; set; }
        public string? Location { get; set; }
        public DateTime? StartDt { get; set; }
        public DateTime? EndDt { get; set; }
        public string? Skills { get; set; }
        public string? Achievement { get; set; }
        public string? JobDescription { get; set; }
        public DateTime? Created_dt { get; set; }
        public int? Created_by { get; set; }
        public DateTime? Modified_dt { get; set; }
        public int? Modified_by { get; set; }
        public string? Status { get; set; }
    }
    public class ExperienceDtl
    {
        public int Id { get; set; }
        public string? Position { get; set; }
        public string? Company { get; set; }
        public string? Location { get; set; }
        public DateTime? StartDt { get; set; }
        public DateTime? EndDt { get; set; }
        public string? Skills { get; set; }
        public string? Achievement { get; set; }
        public string? JobDescription { get; set; }
    }
}
