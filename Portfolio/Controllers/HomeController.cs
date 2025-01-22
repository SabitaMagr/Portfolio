using AspNetCore.ReCaptcha;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Portfolio.Domain.Entities;
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
            catch (Exception )
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
            catch (Exception )
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
            }catch(Exception )
            {
                ViewData["Message"] = "An error occured !";
                return View();
            }
        }
        #region Profile Details
        public IActionResult ProfileDetails()
        {
            return View();
        }
        [HttpGet]
        public IActionResult PersonalDetail(int? id)
        {
            if (id.HasValue)
            {
                var data = _user.GetPersonalDtById(id);

                if (data == null)
                {
                    ViewData["MessageType"] = "Failure";
                    ViewData["Message"] = "Error fetching data!";
                }

                return View(data); // Pass the model to the view for editing
            }
            return View();
        }
        [HttpPost]
        public IActionResult PersonalDetail(PersonalDtl data)
        {
            try
            {
                if (data.UserId == 0)
                {
                    ModelState.Remove(nameof(data.UserId));
                }
                if (ModelState.IsValid)
                {
                    var token = HttpContext.Request.Cookies["AuthToken"];
                    if (string.IsNullOrEmpty(token))
                    {
                        return View();
                    }
                    bool result=_user.AddData(data,token);
                    if(result)
                    {
                        ViewData["MessageType"] = "Success";
                        ViewData["Message"] = "Save data successfully!";
                    }
                    else
                    {
                        ViewData["MessageType"] = "Failure";
                        ViewData["Message"] = "Failed to save data !";
                    }
                }
                return View("~/Views/Home/ProfileDetails.cshtml");
            }
            catch (Exception )
            {
                ViewData["MessageType"] = "Failure";
                ViewData["Message"] = "Failed to save data !";
                return View();
            }
        }
        [HttpGet]
        public JsonResult GetPersonalDtl()
        {
            try
            {
                var token = HttpContext.Request.Cookies["AuthToken"];
                if (string.IsNullOrEmpty(token))
                {
                    return Json(new { data = new List<object>() });
                }
                var data = _user.GetPersonalDtl(token);
                return Json(new { data });
            }
            catch (Exception)
            {
                return Json(new { data = new List<object>() });
            }
        }
        [HttpDelete]
        public IActionResult DeletePersonalData(int id)
        {
            try
            {
                var token = HttpContext.Request.Cookies["AuthToken"];
                if (string.IsNullOrEmpty(token))
                {
                    return RedirectToAction("Skills", "Home");
                }
                bool data = _user.DeletePersonalData(id, token);
                if (data)
                {
                    ViewData["MessageType"] = "Success";
                    ViewData["Message"] = "Skill updated successfully !";
                }
                else
                {
                    ViewData["MessageType"] = "Failure";
                    ViewData["Message"] = "Failed to delete skill !";
                }
                return View("~/Views/Home/Skills.cshtml");
            }
            catch (Exception)
            {
                TempData["MessageType"] = "Failure";
                TempData["Message"] = "Failed to delete skill !";
                return RedirectToAction("Skills", "Home");
            }
        }
        #endregion Profile Details
        #region Education
        [HttpGet]
        public IActionResult Education()
        {
            return View();
        }
        [HttpGet]
        public IActionResult EducationDetail(int? id)
        {
            if (id.HasValue)
            {
                var data = _user.GetEducationDtById(id);

                if (data == null)
                {
                    ViewData["MessageType"] = "Failure";
                    ViewData["Message"] = "Error fetching data!";
                }

                return View(data); // Pass the model to the view for editing
            }
            return View();
        }
        [HttpPost]
        public IActionResult EducationDetail(EducationDtl data)
        {
            try
            {
                if (data.Id == 0)
                {
                    ModelState.Remove(nameof(data.Id));
                }
                if (ModelState.IsValid)
                {
                    var token = HttpContext.Request.Cookies["AuthToken"];
                    if (string.IsNullOrEmpty(token))
                    {
                        return View();
                    }
                    bool result = _user.AddEducationData(data, token);
                    if (result)
                    {
                        ViewData["MessageType"] = "Success";
                        ViewData["Message"] = "Save data successfully!";
                    }
                    else
                    {
                        ViewData["MessageType"] = "Failure";
                        ViewData["Message"] = "Failed to save data !";
                    }
                }
                return View("~/Views/Home/Education.cshtml");
            }
            catch (Exception)
            {
                ViewData["MessageType"] = "Failure";
                ViewData["Message"] = "Failed to save data !";
                return View();
            }
        }
        [HttpGet]
        public JsonResult GetEducationDtl()
        {
            try
            {
                var token = HttpContext.Request.Cookies["AuthToken"];
                if (string.IsNullOrEmpty(token))
                {
                    return Json(new { data = new List<object>() });
                }
                var data = _user.GetEducationDtl(token);
                return Json(new { data });
            }
            catch (Exception)
            {
                return Json(new { data = new List<object>() });
            }
        }
        [HttpPost]
        public IActionResult DeleteEducationData(int id)
        {
            try
            {
                var token = HttpContext.Request.Cookies["AuthToken"];
                if (string.IsNullOrEmpty(token))
                {
                    return RedirectToAction("Education", "Home");
                }
                bool data = _user.DeleteEducationData(id, token);
                if (data)
                {
                    ViewData["MessageType"] = "Success";
                    ViewData["Message"] = "Data updated successfully !";
                }
                else
                {
                    ViewData["MessageType"] = "Failure";
                    ViewData["Message"] = "Failed to delete data !";
                }
                return View("~/Views/Home/Skills.cshtml");
            }
            catch (Exception)
            {
                TempData["MessageType"] = "Failure";
                TempData["Message"] = "Failed to delete data !";
                return RedirectToAction("Skills", "Home");
            }
        }
        #endregion Education
        #region Skills
        public IActionResult Skills()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Skills(List<string> Skills,int id)
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
                if (id==0)
                {
                    bool result = _user.AddSkills(Skills, token);
                    ViewData["MessageType"] = "Success";
                    ViewData["Message"] = "Skill added successfully!";
                }
                else
                {
                    bool result = _user.UpdateSkillbyId(Skills,token,id);
                    ViewData["MessageType"] = "Success";
                    ViewData["Message"] = "Skill updated successfully!";
                }
                return View();

            }
            catch (Exception )
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
                    return Json(new { data = new List<object>() });
                }
                var data = _user.getSkills(token);
                return Json(new { data });
            }
            catch (Exception )
            {
                return Json(new { data = new List<object>() });
            }
        }
        [HttpDelete]
        public IActionResult DeleteSkill(int id)
        {
            try
            {
                var token = HttpContext.Request.Cookies["AuthToken"];
                if (string.IsNullOrEmpty(token))
                {
                    return RedirectToAction("Skills","Home");
                }
                bool data = _user.UpdateSkills(id,token);
                if (data)
                {
                    ViewData["MessageType"] = "Success";
                    ViewData["Message"] = "Skill updated successfully !";
                }
                else
                {
                    ViewData["MessageType"] = "Failure";
                    ViewData["Message"] = "Failed to delete skill !";
                }
                return View("~/Views/Home/Skills.cshtml");
            }
            catch (Exception ) 
            {
                TempData["MessageType"] = "Failure";
                TempData["Message"] = "Failed to delete skill !";
                return RedirectToAction("Skills", "Home");
            }
        }
        [HttpGet]
        public IActionResult GetSkillById(int id)
        {
            var skill = _user.GetSkillById(id);
            if (skill != null)
            {
                return Json(new { success = true, data = skill });
            }

            return Json(new { success = false, message = "Skill not found." });
        }
        #endregion Skills
        #region Experience
        public IActionResult Experience()
        {
            return View();
        }
        [HttpGet]
        public IActionResult ExperienceDetail(int? id)
        {
            if (id.HasValue)
            {
                var data = _user.GetExperienceDtById(id);

                if (data == null)
                {
                    ViewData["MessageType"] = "Failure";
                    ViewData["Message"] = "Error fetching data!";
                }

                return View(data); // Pass the model to the view for editing
            }
            return View();
        }
        [HttpPost]
        public IActionResult ExperienceDetail(ExperienceDtl data)
        {
            try
            {
                if (data.Id == 0)
                {
                    ModelState.Remove(nameof(data.Id));
                }
                if (ModelState.IsValid)
                {
                    var token = HttpContext.Request.Cookies["AuthToken"];
                    if (string.IsNullOrEmpty(token))
                    {
                        return View();
                    }
                    bool result = _user.AddExperienceData(data, token);
                    if (result)
                    {
                        ViewData["MessageType"] = "Success";
                        ViewData["Message"] = "Save data successfully!";
                    }
                    else
                    {
                        ViewData["MessageType"] = "Failure";
                        ViewData["Message"] = "Failed to save data !";
                    }
                }
                return View("~/Views/Home/Experience.cshtml");
            }
            catch (Exception)
            {
                ViewData["MessageType"] = "Failure";
                ViewData["Message"] = "Failed to save data !";
                return View();
            }
        }
        [HttpGet]
        public JsonResult GetExperienceDtl()
        {
            try
            {
                var token = HttpContext.Request.Cookies["AuthToken"];
                if (string.IsNullOrEmpty(token))
                {
                    return Json(new { data = new List<object>() });
                }
                var data = _user.GetExperienceDtl(token);
                return Json(new { data });
            }
            catch (Exception)
            {
                return Json(new { data = new List<object>() });
            }
        }
        [HttpPost]
        public IActionResult DeleteExperienceData(int id)
        {
            try
            {
                var token = HttpContext.Request.Cookies["AuthToken"];
                if (string.IsNullOrEmpty(token))
                {
                    return RedirectToAction("Education", "Home");
                }
                bool data = _user.DeleteExperienceData(id, token);
                if (data)
                {
                    ViewData["MessageType"] = "Success";
                    ViewData["Message"] = "Data updated successfully !";
                }
                else
                {
                    ViewData["MessageType"] = "Failure";
                    ViewData["Message"] = "Failed to delete data !";
                }
                return View("~/Views/Home/Skills.cshtml");
            }
            catch (Exception)
            {
                TempData["MessageType"] = "Failure";
                TempData["Message"] = "Failed to delete data !";
                return RedirectToAction("Skills", "Home");
            }
        }
        #endregion Experience
        #region Project 
        public IActionResult Project()
        {
            return View();
        }

        #endregion
        #region Project
        public IActionResult Project()
        {
            return View();
        }
        #endregion Project
        public string GenerateToken(UserTbl data)
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
