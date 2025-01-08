using AspNetCore.ReCaptcha;
using Microsoft.AspNetCore.Mvc;
using Portfolio.Domain.Entities.User;
using Portfolio.Domain.Interfaces;
using Portfolio.Models;
using System.Diagnostics;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
        [ValidateAntiForgeryToken]
        [ValidateReCaptcha]
        public IActionResult LogIn(LoginModel model)
         
        {
            try
            { if(ModelState.IsValid)
                {
                    var data=_user.ValidateUser(model);
                    if (data!=null)
                    {
                        return View();
                    }
                    else
                    {
                        ViewData["Message"] = "Incorecct User name or Password.";
                        return View();
                    }
                }
                else
                {
                    return View();
                }

            }
            catch (Exception ex)
            {
                return View();
            }
        }
        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SignUp(SignUpModel model)
        {
            try
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
                return View(model);
            }
            catch (Exception ex)
            {
                ViewData["Response"] = "Failed to register user.";
                return View(model);
            }
        }
    }
}
