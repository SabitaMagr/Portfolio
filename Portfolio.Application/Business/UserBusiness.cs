using Portfolio.Domain.Entities.User;
using Portfolio.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portfolio.Application.Business
{
    public class UserBusiness : Iuser
    {
        IUserRepo _repo;

       public UserBusiness(IUserRepo repo)
        {
            _repo = repo;
        }   

        public bool AddUser(SignUpModel model)
        {
            return _repo.AddUser(model);
        }
        public UserTbl ValidateUser(LoginModel model)
        {
            return _repo.ValidateUser(model);
        }
    }
}
