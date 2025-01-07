using Microsoft.EntityFrameworkCore;
using Portfolio.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portfolio.Domain.Domain
{
    public partial class PortfolioDbContext:DbContext
    {
        public PortfolioDbContext(DbContextOptions<PortfolioDbContext> options)
        : base(options)
        {
        }

        public virtual DbSet<UserTbl> UserTbls { get; set; } = null!;
    }
}
