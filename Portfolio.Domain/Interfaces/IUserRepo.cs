using Portfolio.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portfolio.Domain.Interfaces
{
    public interface IUserRepo
    {
        bool AddUser(SignUpModel model);
        UserTbl ValidateUser(LoginModel model);
    }
}
