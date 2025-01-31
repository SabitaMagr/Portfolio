using Microsoft.AspNetCore.Mvc;
using Portfolio.Domain.Interfaces;

namespace Portfolio.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class PortfolioController : ControllerBase
    {
        private readonly IPortfolioRepo _portfolioRepo;
        public PortfolioController(IPortfolioRepo portfolioRepo) { 
            _portfolioRepo = portfolioRepo;
        }
        public Dictionary<string,object> Index(int id)
        {
          var  result=new Dictionary<string, object>();
            result = _portfolioRepo.GetAllDetails(id);
            return result;
        }
    }
}
