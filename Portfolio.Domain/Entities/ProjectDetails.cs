using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portfolio.Domain.Entities
{
    [Table("ProjectDetails")]
    public class ProjectDetails
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? GitLink { get; set; }
        public string? DeployLink {  get; set; }
        public string? ImageName { get; set; }
        public string? Status { get; set; }
        public DateTime? Created_dt { get; set; }
        public int? Created_by { get; set; }
        public DateTime? Modified_dt { get; set; }
        public int? Modified_by { get; set; }
    }
    public class ProjectDtl
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? GitLink { get; set; }
        public string? DeployLink { get; set; }
        public string? ImageName { get; set; }
        [NotMapped]
        public IFormFile? ImageFile { get; set; }
        [NotMapped]
        public string? ImageUrl { get; set; }
    }
}
