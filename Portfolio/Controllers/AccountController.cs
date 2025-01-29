using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Portfolio.Domain.Entities.User.ChangePassword;
using Portfolio.Domain.HelperClass;
using Portfolio.Domain.Interfaces;
using System.Net.Mail;
using System.Xml.Linq;

namespace Portfolio.Controllers
{
    public class AccountController : Controller
    {
        private readonly IChangePassword _changePassword;
        public AccountController(IChangePassword changePassword) {
            _changePassword = changePassword;

        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Index(UsernameModel model)
        {
            if (ModelState.IsValid)
            {
                var userDetails =_changePassword.checkUsername(model.Username);
                if (userDetails != null)
                {
                    try
                    {
                        var toEmail = userDetails.Email;
                        var toName = userDetails.FullName;
                        var code = new Random().Next(100000, 999999);
                        var expiryDate = DateTime.Now.AddDays(1);
                        var codeData = new CodeModel
                        {
                            Code = code,
                            UserId = userDetails.UserId,
                            Expiry_date = expiryDate,
                        };
                        _changePassword.add(codeData);
                        if (string.IsNullOrEmpty(toEmail) || !IsValidEmail(toEmail))
                        {
                            throw new Exception("Receiver email is not set or valid.");
                        }
                        var body = $"Hi {toName},<br/>You can enter the following reset code:<br/><b>{code}</b><br/><br>Your Code will expire on {expiryDate:yyyy-MM-dd HH:mm:ss}";
                        var mail = new MailMessage
                        {
                            To = { toEmail },
                            Subject = $"Password Recovery",
                            Body = body,
                            IsBodyHtml = true
                        };
                        var emailSent = EmailHelper.SendEmail(mail);

                        if (!emailSent)
                        {
                            TempData["MessageType"] = "Failure";
                            TempData["Message"] = "Failed to send code!"; 
                            return View();
                        }
                        TempData["MessageType"] = "Success";
                        TempData["Message"] = "Successfully send code in your gmail!";
                        return RedirectToAction("Code", new { userDetails.UserId });
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Error: {e.Message}");
                    }
                }
            }
            return View();
        }
        [HttpGet]
        public IActionResult Code(string UserId)
        {
            return View();
        }
        [HttpPost]
        public IActionResult Code(CodeModel data)
        {
            var codeDetails = _changePassword.checkCode(data.Code);
            if (data==null)
            {
                TempData["MessageType"] = "Failure";
                TempData["Message"] = "Code doesnot match!";
                return View();
            }
            return RedirectToAction("changePassword", new { codeDetails.UserId });
        }
        [HttpGet]
        public IActionResult changePassword(int UserId)
        {
            ViewData["UserId"] = UserId;
            return View();
        }
        [HttpPost]
        public IActionResult changePassword(PasswordModel model)
        {
            bool result =_changePassword.updatePassword(model.ConfirmPassword,model.UserId);
            if (result)
            {
                TempData["MessageType"] = "Success";
                TempData["Message"] = "Password updated successfully!";
                return RedirectToAction("LogIn", "Home");
            }
            else

            {
                ViewData["MessageType"] = "Failure";
                ViewData["Message"] = "Failed to update password!";
            }
            return View();
        }
        private static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
