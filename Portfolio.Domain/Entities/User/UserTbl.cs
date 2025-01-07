using System;
using System.Collections.Generic;

namespace Portfolio.Domain.Entities.User
{
    public partial class UserTbl
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;
        public DateTime CreatedDt { get; set; }
        public string? Status { get; set; }
    }
}
