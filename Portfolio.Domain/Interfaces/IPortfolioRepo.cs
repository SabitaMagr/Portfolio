using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portfolio.Domain.Interfaces
{
    public interface IPortfolioRepo
    {
        public Dictionary<string, object> GetAllDetails(int id);
    }
}
