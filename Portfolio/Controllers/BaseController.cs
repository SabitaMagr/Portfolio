using Microsoft.AspNetCore.Mvc;
using Portfolio.Extension;

namespace Portfolio.Controllers
{
    public class BaseController : Controller
    {
        protected int userId => HttpContext.GetUserId();
    }
}
