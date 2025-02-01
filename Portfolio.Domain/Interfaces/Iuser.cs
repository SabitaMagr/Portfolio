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
        List<DataCountModel> getTotalData(int userId);
        #region Skills
        bool AddSkills(List<string> skills,int userId);
        List<SkillDetail> getSkills(int userId);

        bool UpdateSkills(int id,int userId);
        Skills GetSkillById(int id);
        bool UpdateSkillbyId(List<string> skills, int userId,int id);
        #endregion
        #region PersonalDetails
        bool AddData(PersonalDtl data,int userId);
        List<PersonalDtl> GetPersonalDtl(int userId);
        bool DeletePersonalData(int id, int userId);
        PersonalDtl GetPersonalDtById(int? id);
        #endregion
        #region Education
        bool AddEducationData(EducationDtl data, int userId);
        EducationDtl GetEducationDtById(int? id);
        List<EducationDtl> GetEducationDtl(int userId);
        bool DeleteEducationData(int id, int userId);
        #endregion
        #region Experience
        bool AddExperienceData(ExperienceDtl data, int userId);
        ExperienceDtl GetExperienceDtById(int? id);
        List<ExperienceDtl> GetExperienceDtl(int userId);
        bool DeleteExperienceData(int id, int userId);
        #endregion
        #region Project
        bool AddProjectData(ProjectDtl data, int userId);
        ProjectDtl GetProjectDetailById(int? id);
        List<ProjectDtl> GetProjectDtl(int userId);
        bool DeleteProjectData(int id, int userId);
        #endregion
        #region ChangePassword
        public bool checkOldPassword(string oldPassword, int userId);
        public bool changePassword(string NewPassword, int userId);
        #endregion
    }
}
