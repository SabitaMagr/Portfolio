using AspNetCore.ReCaptcha;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Portfolio.Domain.Entities;
using Portfolio.Domain.Entities.User;
using Portfolio.Domain.HelperClass;
using Portfolio.Domain.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Portfolio.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        Iuser _user;
        IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public HomeController(ILogger<HomeController> logger, Iuser user, IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _user = user;
            _configuration = configuration;
            _httpClient = httpClientFactory.CreateClient();

        }

        public async Task<IActionResult> Index()
        {
            int userId = 1; // Pass an appropriate user ID
            var apiUrl = $"{_configuration["ApiBaseUrl"]}/api/Portfolio/{userId}";

            var response = await _httpClient.GetAsync(apiUrl);
            if (response.IsSuccessStatusCode)
            {
				var data = await response.Content.ReadFromJsonAsync<Dictionary<string, object>>();
				return View(data);
            }

            return View(new Dictionary<string, object>());
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
                var userName = StaticHelper.GetDetail(token, "Full_name");
                TempData["UserName"] = userName;
                var countData = _user.getTotalData(userId);
                return View(countData);
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
                    bool result=_user.AddData(data,userId);
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
                var data = _user.GetPersonalDtl(userId);
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
                bool data = _user.DeletePersonalData(id, userId);
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
                    bool result = _user.AddEducationData(data, userId);
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
                var data = _user.GetEducationDtl(userId);
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
                bool data = _user.DeleteEducationData(id, userId);
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
                return View("~/Views/Home/Education.cshtml");
            }
            catch (Exception)
            {
                TempData["MessageType"] = "Failure";
                TempData["Message"] = "Failed to delete data !";
                return RedirectToAction("Education", "Home");
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
                if (id==0)
                {
                    bool result = _user.AddSkills(Skills, userId);
                    ViewData["MessageType"] = "Success";
                    ViewData["Message"] = "Skill added successfully!";
                }
                else
                {
                    bool result = _user.UpdateSkillbyId(Skills, userId, id);
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
                var data = _user.getSkills(userId);
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
                bool data = _user.UpdateSkills(id, userId);
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
                    bool result = _user.AddExperienceData(data, userId);
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
                var data = _user.GetExperienceDtl(userId);
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
                bool data = _user.DeleteExperienceData(id, userId);
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
                return View("~/Views/Home/Experience.cshtml");
            }
            catch (Exception)
            {
                TempData["MessageType"] = "Failure";
                TempData["Message"] = "Failed to delete data !";
                return RedirectToAction("Experience", "Home");
            }
        }
        #endregion Experience
        #region Project 
        public IActionResult Project()
        {
            return View();
        }
        [HttpGet]
        public IActionResult ProjectDetail(int? id)
        {
            if (id.HasValue)
            {
                var data = _user.GetProjectDetailById(id);
                string uploadsFolder = Path.Combine("wwwroot", "Images", "Project");
                if (!string.IsNullOrEmpty(data.ImageName))
                    {
                    string physicalPath = Path.Combine(uploadsFolder, data.ImageName);
                    if (System.IO.File.Exists(physicalPath))
                    {
                        data.ImageUrl = $"{Request.Scheme}://{Request.Host}/Images/Project/{data.ImageName}";
                    }
                    else
                    {
                        data.ImageUrl = null;
                    }
                }
                
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
        public IActionResult ProjectDetail(ProjectDtl data)
        {
            try
            {
                if (data.Id == 0)
                {
                    ModelState.Remove(nameof(data.Id));
                }

                if (ModelState.IsValid)
                {
                    // Handle file upload
                    if (data.ImageFile != null)
                    {
                        string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images","Project");
                        if (!Directory.Exists(uploadsFolder))
                        {
                            Directory.CreateDirectory(uploadsFolder);
                        }
                        string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                        string uniqueFileName = $"{timestamp}_{Guid.NewGuid().ToString("N")}.png";
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            data.ImageFile.CopyTo(fileStream);
                        }

                        data.ImageName = uniqueFileName; // Save the relative path
                    }

                    bool result = _user.AddProjectData(data, userId);
                    if (result)
                    {
                        ViewData["MessageType"] = "Success";
                        ViewData["Message"] = "Save data successfully!";
                    }
                    else
                    {
                        ViewData["MessageType"] = "Failure";
                        ViewData["Message"] = "Failed to save data!";
                    }
                }

                return View("~/Views/Home/Project.cshtml");
            }
            catch (Exception)
            {
                ViewData["MessageType"] = "Failure";
                ViewData["Message"] = "Failed to save data!";
                return View(data);
            }
        }

        [HttpGet]
        public JsonResult GetProjectDtl()
        {
            try
            {
                var data = _user.GetProjectDtl(userId);
                string uploadsFolder = Path.Combine("wwwroot", "Images", "Project");
                foreach (var project in data)
                {
                    if(!string.IsNullOrEmpty(project.ImageName))
                    {
                        string physicalPath = Path.Combine(uploadsFolder, project.ImageName);
                        if (System.IO.File.Exists(physicalPath))
                        {
                            project.ImageUrl = $"{Request.Scheme}://{Request.Host}/Images/Project/{project.ImageName}";
                        }
                      }
                }
                return Json(new { data });
            }
            catch (Exception)
            {
                return Json(new { data = new List<object>() });
            }
        }
        [HttpPost]
        public IActionResult DeleteProjectData(int id)
        {
            try
            {
                bool data = _user.DeleteProjectData(id, userId);
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
                return View("~/Views/Home/Project.cshtml");
            }
            catch (Exception)
            {
                TempData["MessageType"] = "Failure";
                TempData["Message"] = "Failed to delete data !";
                return RedirectToAction("Project", "Home");
            }
        }
        [HttpPost]
        public IActionResult DeleteImage(string imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl))
            {
                return Json(new { success = false, message = "Invalid image URL" });
            }

            try
            {
                Uri imageUri = new Uri(imageUrl);

                string relativePath = imageUri.AbsolutePath.TrimStart('/');

                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", relativePath.Replace('/', Path.DirectorySeparatorChar));

                if (System.IO.File.Exists(filePath))
                {
                    // Delete the file
                    System.IO.File.Delete(filePath);
                    return Json(new { success = true, message = "File deleted successfully." });
                }
                if (System.IO.File.Exists(relativePath))
                {
                    System.IO.File.Delete(relativePath);
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false, message = "File not found" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        #endregion
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
        public IActionResult logOut()
        {
            HttpContext.Response.Cookies.Append("AuthToken", "", new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddDays(-1)
            });
            return RedirectToAction("LogIn", "Home");
        }
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return PartialView("~/Views/Shared/PartialView/_changePassword.cshtml");
            //return PartialView("_ChangePassword");
        }
        [HttpPost]
        public IActionResult ChangePassword(ChangePasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Modal"] = "changePasswordModal";
                return RedirectToAction("Dashboard",model);
            }
            if (!_user.checkOldPassword(model.OldPassword, userId))
            {
                TempData["MessageType"] = "Failure";
                TempData["Message"] = "Old password doesnot match!";
                return PartialView("~/Views/Shared/PartialView/_changePassword.cshtml");
            }
            var result = _user.changePassword(model.NewPassword, userId);

            if (result)
            {
                TempData["MessageType"] = "Success";
                TempData["Message"] = "Password change successfully!";
                return RedirectToAction("Dashboard");
            }
            ModelState.AddModelError("", "An error occurred while changing the password.");
            return View(model);
        }

    }
}
