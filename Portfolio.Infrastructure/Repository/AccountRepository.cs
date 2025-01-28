using Portfolio.Domain.Entities;
using Portfolio.Domain.Entities.User;
using Portfolio.Domain.HelperClass;
using Portfolio.Domain.Interfaces;
using Portfolio.Infrastructure.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portfolio.Infrastructure.Repository
{
    public class AccountRepository:IChangePassword
    {
        PortfolioDbContext _dbContext;
        public AccountRepository(PortfolioDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public PersonalDetails checkUsername(string username)
        {
            try
            {
                var user = _dbContext.UserTbl.FirstOrDefault(u => u.User_name == username);

                if (user == null)
                {
                    return null;  
                }
                var data = _dbContext.PersonalDetails.FirstOrDefault(u=>u.UserId==user.Id);
                return data;
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching data by ID", ex);
            }
        }
    }
}
