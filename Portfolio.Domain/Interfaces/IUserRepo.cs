using Portfolio.Domain.Entities;
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
        bool AddSkills(List<string> skills,string token);
        List<SkillDetail> getSkills(string token);
        bool UpdateSkills(int id, string token);
        Skills GetSkillById(int id);
        bool UpdateSkillbyId(List<string> skills, string token, int id);

    }
}
