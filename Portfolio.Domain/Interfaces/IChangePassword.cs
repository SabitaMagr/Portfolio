using Microsoft.AspNetCore.Identity;
using Portfolio.Domain.Entities;
using Portfolio.Domain.Entities.User;
using Portfolio.Domain.Entities.User.ChangePassword;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portfolio.Domain.Interfaces
{
    public interface IChangePassword
    {
        public PersonalDetails checkUsername(string username);
        public void add(CodeModel code);
        public bool updatePassword(string password,int userId);
        public CodeModel checkCode(int Code);

    }
}
