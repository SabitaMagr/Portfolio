using Microsoft.EntityFrameworkCore;
using Portfolio.Domain.Entities;
using Portfolio.Domain.Entities.User;
using Portfolio.Domain.Entities.User.ChangePassword;
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
        public void add(CodeModel data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            int maxId = GetMaxId<UserTbl>("Id");
            data.Id = maxId;
            _dbContext.CodeDetails.Add(data);
            _dbContext.SaveChanges();
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
        public bool updatePassword(string password, int userId)
        {
            try
            {
                var user = _dbContext.UserTbl.FirstOrDefault(u => u.Id == userId);

                if (user == null)
                {
                    return false;
                }
               user.Password=StaticHelper.EncryptString(password);
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw new Exception("Error fetching data by ID", ex);
            }
        }
        public CodeModel checkCode(int code)
        {
            try
            {
                var data = _dbContext.CodeDetails.FirstOrDefault(u => u.Code == code);

                if (data == null)
                {
                    return null;
                }
                return data;
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching data by ID", ex);
            }
        }
    }
}
