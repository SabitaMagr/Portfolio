using Portfolio.Domain.Entities;
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
        public bool AddSkills(List<string> skills,string token)
        {
            return _repo.AddSkills(skills,token);
        }
        public List<SkillDetail> getSkills(string token)
        {
            return _repo.getSkills(token);
        }
        public bool UpdateSkills(int id, string token)
        {
            return _repo.UpdateSkills(id, token);
        }
        public Skills GetSkillById(int id)
        {
            return _repo.GetSkillById(id);
        }
        public bool UpdateSkillbyId(List<string> skills, string token,int id)
        {
            return _repo.UpdateSkillbyId(skills, token,id);
        }
        public bool AddData(PersonalDtl data, string token)
        {
            return _repo.AddData(data,token);
        }
        public List<PersonalDtl> GetPersonalDtl(string token)
        {
            return _repo.GetPersonalDtl(token);
        }
        public bool DeletePersonalData(int id, string token)
        {
            return _repo.DeletePersonalData(id, token);
        }
        public PersonalDtl GetPersonalDtById(int id)
        {
            return _repo.GetPersonalDtById(id);
        }
    }
}
