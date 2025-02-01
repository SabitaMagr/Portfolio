using Portfolio.Domain.Entities;
using Portfolio.Domain.Entities.User;
using Portfolio.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
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
        public List<DataCountModel> getTotalData(int userId)
        {
            return _repo.getTotalData(userId);
        }

        #region Skills
        public bool AddSkills(List<string> skills,int userId)
        {
            return _repo.AddSkills(skills,userId);
        }
        public List<SkillDetail> getSkills(int userId)
        {
            return _repo.getSkills(userId);
        }
        public bool UpdateSkills(int id, int userId)
        {
            return _repo.UpdateSkills(id, userId);
        }
        public Skills GetSkillById(int id)
        {
            return _repo.GetSkillById(id);
        }
        public bool UpdateSkillbyId(List<string> skills, int userId,int id)
        {
            return _repo.UpdateSkillbyId(skills, userId,id);
        }
        #endregion
        #region Personal Details
        public bool AddData(PersonalDtl data, int userId)
        {
            return _repo.AddData(data,userId);
        }
        public List<PersonalDtl> GetPersonalDtl(int userId)
        {
            return _repo.GetPersonalDtl(userId);
        }
        public bool DeletePersonalData(int id, int userId)
        {
            return _repo.DeletePersonalData(id, userId);
        }
        public PersonalDtl GetPersonalDtById(int? id)
        {
            return _repo.GetPersonalDtById(id);
        }
        #endregion
        #region Education
        public bool AddEducationData(EducationDtl data, int userId)
        {
            return _repo.AddEducationData(data, userId);
        }
        public List<EducationDtl> GetEducationDtl(int userId)
        {
            return _repo.GetEducationDtl(userId);
        }
        public bool DeleteEducationData(int id, int userId)
        {
            return _repo.DeleteEducationData(id, userId);
        }
        public EducationDtl GetEducationDtById(int? id)
        {
            return _repo.GetEducationDtById(id);
        }
        #endregion
        #region Experience
        public bool AddExperienceData(ExperienceDtl data, int userId)
        {
            return _repo.AddExperienceData(data, userId);
        }
        public List<ExperienceDtl> GetExperienceDtl(int userId)
        {
            return _repo.GetExperienceDtl(userId);
        }
        public bool DeleteExperienceData(int id, int userId)
        {
            return _repo.DeleteExperienceData(id, userId);
        }
        public ExperienceDtl GetExperienceDtById(int? id)
        {
            return _repo.GetExperienceDtById(id);
        }
        #endregion
        #region Project
        public bool AddProjectData(ProjectDtl data, int userId)
        {
            return _repo.AddProjectData(data, userId);
        }
        public List<ProjectDtl> GetProjectDtl(int userId)
        {
            return _repo.GetProjectDtl(userId);
        }
        public bool DeleteProjectData(int id, int userId)
        {
            return _repo.DeleteProjectData(id, userId);
        }
        public ProjectDtl GetProjectDetailById(int? id)
        {
            return _repo.GetProjectDetailById(id);
        }
        #endregion
        #region Change Password
        public bool checkOldPassword(string oldPassword,int userId)
        {
            return _repo.checkOldPassword(oldPassword, userId);
        }
        public bool changePassword(string NewPassword, int userId)
        {
            return _repo.changePassword(NewPassword, userId);
        }
        #endregion
    }
}
