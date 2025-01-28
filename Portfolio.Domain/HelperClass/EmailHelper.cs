using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Portfolio.Domain.HelperClass
{
    public class EmailHelper
    {
        private const int MaxMassMail = 50;
        private const string MassEmailId = "mhrpravin@gmail.com";
        private const string SmtpHost = "smtp.gmail.com";
        private const int SmtpPort = 587;
        private const string SmtpUsername = "sabitamgr133@gmail.com";
        private const string SmtpPassword = "gxlgwfjehrbpdpsm";

        public static SmtpClient GetSmtpTransport()
        {
            var smtpClient = new SmtpClient(SmtpHost)
            {
                Port = SmtpPort,
                EnableSsl = true,
                Credentials = new NetworkCredential(SmtpUsername, SmtpPassword),
                DeliveryMethod = SmtpDeliveryMethod.Network
            };

            return smtpClient;
        }

        public static bool SendEmail(MailMessage mail)
        {
            var smtpClient = GetSmtpTransport();
            // Set the sender email address
            mail.From = new MailAddress(SmtpUsername);

            try
            {
                smtpClient.Send(mail);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
                return false;
            }
        }
    }
}
