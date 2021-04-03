using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace ETicaret.Data
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            MailMessage mail = new MailMessage
            {
                From = new MailAddress("youremail@gmail.com","Yönetim",System.Text.Encoding.UTF8),
                Subject = subject,
                Body = htmlMessage,
                IsBodyHtml = true
            };
            mail.To.Add(email);
            SmtpClient smp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("youremail@gmail.com", "yourpassword"),
                Port = 587,
                EnableSsl = true
            };
            smp.Send(mail);
            
            return Task.CompletedTask;
        }
    }
}