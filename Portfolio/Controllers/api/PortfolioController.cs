using Microsoft.AspNetCore.Mvc;

namespace Portfolio.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class PortfolioController : ControllerBase
    {
        public PortfolioController() { 
        }
        public Dictionary<string,string> Index()
        {
          var  result=new Dictionary<string, string>();
            result=
            return result;
        }
    }
}
