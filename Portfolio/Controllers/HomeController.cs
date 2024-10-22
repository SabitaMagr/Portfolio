using Microsoft.AspNetCore.Mvc;
using AspNetCore.ReCaptcha;

namespace Portfolio.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Home()
        {
            return View();
        }
        public IActionResult LogIn()
        {
            return View();
        }
    }
}
