using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portfolio.Domain.Entities.User.ChangePassword
{
    public class UsernameModel
    {
        [Required(ErrorMessage ="User name is required!")]
        public string Username { get; set; }
    }
}
