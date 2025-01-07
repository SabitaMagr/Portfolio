using System;
using System.Collections.Generic;

namespace Portfolio.Domain.Entities.User
{
    public partial class UserTbl
    {
        public int Id { get; set; }
        public string Full_name { get; set; } = null!;
        public string User_name { get; set; } = null!;
        public string Password { get; set; } = null!;
        public DateTime Created_dt { get; set; }
        public string? Status { get; set; }
    }
}
