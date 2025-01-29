using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portfolio.Domain.Entities.User.ChangePassword
{
    [Table("CodeTbl")]
    public class CodeModel
    {
        [Key]
        public int Id { get; set; }
        public int Code { get; set; }
        public int? UserId { get; set; }
        public DateTime? Expiry_date { get; set; }

    }
}
