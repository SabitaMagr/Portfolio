using AspNetCore.ReCaptcha;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Portfolio.Domain.Entities.User;
using Portfolio.Domain.HelperClass;
using Portfolio.Domain.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Portfolio.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        Iuser _user;
        IConfiguration _configuration;
        public HomeController(ILogger<HomeController> logger, Iuser user, IConfiguration configuration)
        {
            _logger = logger;
            _user = user;
            _configuration = configuration; 
        }

        public IActionResult Index()
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
                        var token=GenerateToken(data);
                        //Storing token in cookies
                        HttpContext.Response.Cookies.Append("AuthToken", token, new CookieOptions
                        {
                            HttpOnly = true,
                            Secure = true,
                            SameSite = SameSiteMode.Strict,
                            Expires = DateTime.UtcNow.AddHours(1)
                        });
                        TempData["Message"] = "Login successfully !";
                        TempData["UserName"] =data.User_name;
                        return RedirectToAction("Dashboard");
                    }
                    else
                    {
                        ViewData["MessageType"] = "Failure";
                        ViewData["Message"] = "Incorrect User name or Password.";
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
                ViewData["MessageType"] = "Failure";
                ViewData["Message"] = "Incorrect User name or Password.";
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
                        TempData["Message"] = "User registered successfully.";
                        return RedirectToAction("LogIn");
                    }
                    else
                    {
                        ViewData["Message"] = "Failed to register user.";
                        return View(model);
                    }
                }
                return View(model);
            }
            catch (Exception ex)
            {
                ViewData["MessageType"] = "Failure";
                ViewData["Message"] = "Failed to register user.";
                return View(model);
            }
        }
        public IActionResult Dashboard()
        {
            try
            {
                var token = HttpContext.Request.Cookies["AuthToken"];
                if(string.IsNullOrEmpty(token))
                {
                    return RedirectToAction("LogIn");
                }
                var userName = StaticHelper.GetDetail(token, ClaimTypes.Name);
                ViewData["UserName"] = userName;
                return View();
            }catch(Exception ex)
            {
                ViewData["Message"] = "An error occured !";
                return View();
            }
        }

        public IActionResult Skills()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Skills(List<string> Skills)
        {

            if(Skills==null || !Skills.Any())
            {
                ViewData["MessageType"] = "Failure";
                ViewData["Message"] = "No skills were provided !";
                return View();
            }
            try
            {
                var token = HttpContext.Request.Cookies["AuthToken"];
                if (string.IsNullOrEmpty(token))
                {
                    return View();
                }
                bool result = _user.AddSkills(Skills,token);
                return View();
            }
            catch (Exception ex)
            {
                ViewData["MessageType"] = "Failure";
                ViewData["Message"] = "Failed to save data !";
                return View();
            }
        }
        [HttpGet]
        public JsonResult GetSkills()
        {
            try
            {
                var token = HttpContext.Request.Cookies["AuthToken"];
                if (string.IsNullOrEmpty(token))
                {
                    return null;
                }
                var data = _user.getSkills(token);
                return Json(new { data });
            }
            catch (Exception ex)
            {
        return Json(new { data = new List<object>() });
            }
        }
        public  string GenerateToken(UserTbl data)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var key=new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));
            var creds=new SigningCredentials(key,SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim("Id", data.Id.ToString()),                     
                new Claim("Full_name", data.Full_name),                
                new Claim("User_name", data.User_name),                 
                new Claim("Status", data.Status ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Sub,data.User_name),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
             );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
