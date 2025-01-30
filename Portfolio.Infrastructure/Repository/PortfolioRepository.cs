using Portfolio.Domain.Interfaces;
using Portfolio.Infrastructure.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portfolio.Infrastructure.Repository
{
    public class PortfolioRepository:IPortfolioRepo
    {
        private readonly PortfolioDbContext _dbContext;
        public PortfolioRepository(PortfolioDbContext dbContext) { 
            _dbContext = dbContext;
        }

        public List<Dictionary<string, object>> GetAllDetails() {
            var result = new List<Dictionary<string, object>>();

        }

    }
}
