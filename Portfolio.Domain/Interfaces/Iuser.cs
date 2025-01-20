using Portfolio.Domain.Entities;
using Portfolio.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portfolio.Domain.Interfaces
{
    public interface Iuser
    {
        bool AddUser(SignUpModel model);
        UserTbl ValidateUser(LoginModel model);
        
        bool AddSkills(List<string> skills,string token);
        List<SkillDetail> getSkills(string token);

        bool UpdateSkills(int id,string token);
        Skills GetSkillById(int id);
        bool UpdateSkillbyId(List<string> skills, string token,int id);
        bool AddData(PersonalDtl data,string token);
        List<PersonalDtl> GetPersonalDtl(string token);
        bool DeletePersonalData(int id, string token);
        PersonalDtl GetPersonalDtById(int? id);

    }
}
