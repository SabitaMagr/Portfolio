using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portfolio.Domain.Entities.User
{
    public class SkillDetail
    {
        public int Id { get; set; }
        public string Skill { get; set; }
        public string? Created_by { get; set; }
        public string? Created_dt { get; set; }
    }
}
