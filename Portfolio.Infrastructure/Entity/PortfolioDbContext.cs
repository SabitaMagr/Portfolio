using Microsoft.EntityFrameworkCore;
using Portfolio.Domain.Entities;
using Portfolio.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portfolio.Infrastructure.Entity
{
    public partial class PortfolioDbContext : DbContext
    {
        public PortfolioDbContext(DbContextOptions<PortfolioDbContext> options)
        : base(options)
        {
        }

        public virtual DbSet<UserTbl> UserTbl { get; set; } = null!;
        public virtual DbSet<Skills> Skills { get; set; } = null!;
        public virtual DbSet<PersonalDetails> PersonalDetails { get; set; } = null!;
        public virtual DbSet<EducationDetail> EducationDetail { get; set; } = null!;
        public virtual DbSet<ExperienceDetail> ExperienceDetail { get; set; } = null!;

    }
}
