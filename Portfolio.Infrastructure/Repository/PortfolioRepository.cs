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

		public PortfolioDetailsDto GetAllDetails(int id) {
			return new PortfolioDetailsDto
			{
				PersonalDetails = getPersonalDetails(id),
				Skills = getSkiilsData(id),
				Projects = getProjectDetails(id),
				Experience = getExperinceData(id),
				Education = getEducationData(id)
			};
		}

        public List<PersonalDetails> getPersonalDetails(int id)
        {
            return _dbContext.PersonalDetails.Where(p => p.UserId == id && p.Status != null && p.Status == "E").ToList();
        }
        public List<Skills> getSkiilsData(int id)
        {
            return _dbContext.Skills.Where(p => p.Created_by == id && p.Status != null && p.Status == "E").ToList();
        }
        public List<ProjectDetails> getProjectDetails(int id)
        {
            return _dbContext.ProjectDetails.Where(p => p.Created_by == id && p.Status != null && p.Status == "E").ToList();
        }
        public List<ExperienceDetail> getExperinceData(int id)
        {
            return _dbContext.ExperienceDetail.Where(p => p.Created_by == id && p.Status != null && p.Status == "E").ToList();
        }
        public List<EducationDetail> getEducationData(int id)
        {
            return _dbContext.EducationDetail.Where(p => p.Created_by == id && p.Status != null && p.Status == "E").ToList();
        }
    }
}
