using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portfolio.Domain.Entities
{
    [Table("Education_Details")]
    public class EducationDetail
    {
        [Key]
        public int Id { get; set; }
        public string? Institution { get; set; }
        public string? Location { get; set; }
        public string? Degree { get; set; }
        public string? Grade { get; set; }
        public string? FieldStudy { get; set; }
        public string? Specialization { get; set; }
        public DateTime? StartDt { get; set; }
        public DateTime? EndDt { get; set; }
        public string? Summary { get; set; }
        public DateTime? Created_dt { get; set; }
        public int? Created_by { get; set; }
        public DateTime? Modified_dt { get; set; }
        public int? Modified_by { get; set; }
        public string? Status { get; set; }

    }
    public class EducationDtl
    {
        public int Id { get; set; }
        public string? Institution { get; set; }
        public string? Degree { get; set; }
        public string? Location { get; set; }
        public string? Grade { get; set; }
        public string? FieldStudy { get; set; }
        public string? Specialization { get; set; }
        public DateTime? StartDt { get; set; }
        public DateTime? EndDt { get; set; }
        public string? Summary { get; set; }
    }
}
