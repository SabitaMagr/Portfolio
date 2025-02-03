using Microsoft.AspNetCore.Mvc;
using Portfolio.Domain.Entities;
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
        [HttpGet("{id}")]
        public PortfolioDetailsDto Index(int id)
        {

           var result = _portfolioRepo.GetAllDetails(id);
            return result;
        }
    }
}
