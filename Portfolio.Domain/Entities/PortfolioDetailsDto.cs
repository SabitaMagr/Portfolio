using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portfolio.Domain.Entities
{
	public class PortfolioDetailsDto
	{
		public List<PersonalDetails>? PersonalDetails { get; set; }
		public List<Skills>? Skills { get; set; }
		public List<ProjectDetails>? Projects { get; set; }
		public List<ExperienceDetail>? Experience { get; set; }
		public List<EducationDetail>? Education { get; set; }
	}
}
