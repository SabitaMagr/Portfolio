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
