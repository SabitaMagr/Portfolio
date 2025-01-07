using Microsoft.AspNetCore.Mvc;
using Portfolio.Domain.Entities.User;
using Portfolio.Domain.Interfaces;
using Portfolio.Models;
using System.Diagnostics;

namespace Portfolio.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        Iuser _user;
        public HomeController(ILogger<HomeController> logger, Iuser user)
        {
            _logger = logger;
            _user = user;

        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult LogIn()
        {
            return View();
        }
        [HttpPost]
        public IActionResult LogIn(SignUpModel model)
        {
            return View();
        }
        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SignUp(SignUpModel model)
        {
            if (ModelState.IsValid)
            {
                bool result = _user.AddUser(model);
                if (result)
                {
                    return RedirectToAction("LogIn");
                }
                else
                {
                    return View(model);
                }
            }
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
