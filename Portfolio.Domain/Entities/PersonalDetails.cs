using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portfolio.Domain.Entities
{
    [Table("PersonalDetails")]
    public class PersonalDetails
    {
        [Key]
        public int UserId { get; set; }
        [Required]
        public string? FullName { get; set; }
        [Required]
        public string? MobileNo { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "Enter a valid email address.")]
        public string? Email { get; set; }
        [Required]
        public string? Address { get; set; }
        public string? Profile { get; set; }
        public string? About { get; set; }
        public string? Summary { get; set; }
        public DateTime? Created_dt { get; set; } 
        public int? Created_by { get; set; }
        public DateTime? Modified_dt { get; set; } 
        public int? Modified_by { get; set; }
        public string? Status { get; set; }
    }
    public class PersonalDtl
    {
        public int UserId { get; set; }
        [Required]
        public string? FullName { get; set; }
        [Required]
        public string? MobileNo { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "Enter a valid email address.")]
        public string? Email { get; set; }
        [Required]
        public string? Address { get; set; }
        public string? Profile { get; set; }
        public string? About { get; set; }
        public string? Summary { get; set; }
    }
}
