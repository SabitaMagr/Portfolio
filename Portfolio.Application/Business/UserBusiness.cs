using Portfolio.Domain.Entities.User;
using Portfolio.Domain.Interfaces;
using Portfolio.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portfolio.Application.Business
{
    public class UserBusiness:Iuser
    {
        UserRepository _repo;

       public UserBusiness(UserRepository repo)
        {
            _repo = repo;
        }   

        public bool AddUser(SignUpModel model)
        {
            return _repo.AddUser(model);
        }
    }
}
