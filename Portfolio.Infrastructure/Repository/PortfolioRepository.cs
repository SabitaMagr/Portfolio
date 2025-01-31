using Portfolio.Domain.Entities;
using Portfolio.Domain.Entities.User;
using Portfolio.Domain.Interfaces;
using Portfolio.Infrastructure.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portfolio.Infrastructure.Repository
{
    public class PortfolioRepository:IPortfolioRepo
    {
        private readonly PortfolioDbContext _dbContext;
        public PortfolioRepository(PortfolioDbContext dbContext) { 
            _dbContext = dbContext;
        }

        public Dictionary<string, object> GetAllDetails(int id) {
            var result = new Dictionary<string, object>();
            var personalDetails = getPersonalDetails(id);
            var skills = getSkiilsData(id);
            var projects = getProjectDetails(id);
            var experience = getExperinceData(id);
            var education = getEducationData(id);
            result["PersonalDetails"] = personalDetails;
            result["Skills"] = skills;
            result["Projects"] = projects;
            result["Experience"] = experience;
            result["Education"] = education;

            return result;
        }

        public List<PersonalDetails> getPersonalDetails(int id)
        {
            return _dbContext.PersonalDetails.Where(p => p.UserId == id).ToList();
        }
        public List<Skills> getSkiilsData(int id)
        {
            return _dbContext.Skills.Where(p => p.Created_by == id).ToList();
        }
        public List<ProjectDetails> getProjectDetails(int id)
        {
            return _dbContext.ProjectDetails.Where(p => p.Created_by == id).ToList();
        }
        public List<ExperienceDetail> getExperinceData(int id)
        {
            return _dbContext.ExperienceDetail.Where(p => p.Created_by == id).ToList();
        }
        public List<EducationDetail> getEducationData(int id)
        {
            return _dbContext.EducationDetail.Where(p => p.Created_by == id).ToList();
        }
    }
}
