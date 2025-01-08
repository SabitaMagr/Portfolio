using Microsoft.EntityFrameworkCore;
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
    public class UserRepository : IUserRepo
    {
        PortfolioDbContext _dbContext;
        public UserRepository(PortfolioDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public bool AddUser(SignUpModel model)
        {
            try
            {
                int maxId = GetMaxId<UserTbl>("Id");
                string formattedDate = DateTime.Now.ToString("dd-MMM-yyyy");
                string hashPassword = StaticHelper.EncryptString(model.Password);
                var newUser = new UserTbl
                {
                    Id  =maxId,
                    Full_name = model.FullName,
                    User_name = model.Username,
                    Password = hashPassword,
                    Created_dt = DateTime.ParseExact(formattedDate, "dd-MMM-yyyy", System.Globalization.CultureInfo.InvariantCulture),
                    Status = "E"
                };
                _dbContext.UserTbl.Add(newUser);
                _dbContext.SaveChanges();
                return true;

            }catch(Exception ex)
            {
                 return false;
            }
        }

        public UserTbl ValidateUser(LoginModel model)
        {
            var hashPassord = "";
            if (!string.IsNullOrEmpty(model.Password))
            {
                hashPassord = StaticHelper.decryptString(model.Password);
            }
            var user = _dbContext.UserTbl.FirstOrDefault(u => u.User_name == model.UserName && u.Password == hashPassord && u.Status == "E");
            return user;
        }
        public int GetMaxId<T>(string columnName) where T : class
        {
            try
            {
                var maxId = _dbContext.Set<T>()
                    .Max(e => EF.Property<int?>(e, columnName)) ?? 0;

                return maxId + 1;
            }
            catch
            {
                return 1; // Default to 1 if an error occurs
            }
        }

    }
}
