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
        #region Skills
        bool AddSkills(List<string> skills, string token);
        List<SkillDetail> getSkills(string token);

        bool UpdateSkills(int id, string token);
        Skills GetSkillById(int id);
        bool UpdateSkillbyId(List<string> skills, string token, int id);
        #endregion
        #region PersonalDetails
        bool AddData(PersonalDtl data, string token);
        List<PersonalDtl> GetPersonalDtl(string token);
        bool DeletePersonalData(int id, string token);
        PersonalDtl GetPersonalDtById(int? id);
        #endregion
        #region Education
        bool AddEducationData(EducationDtl data, string token);
        EducationDtl GetEducationDtById(int? id);
        List<EducationDtl> GetEducationDtl(string token);
        bool DeleteEducationData(int id, string token);
        #endregion
         #region Experience
        bool AddExperienceData(ExperienceDtl data, string token);
        ExperienceDtl GetExperienceDtById(int? id);
        List<ExperienceDtl> GetExperienceDtl(string token);
        bool DeleteExperienceData(int id, string token);
        #endregion

    }
}
