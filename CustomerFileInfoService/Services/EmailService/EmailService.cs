using Microsoft.Extensions.Configuration;
using System;
using System.Net;
using System.Net.Mail;

namespace CustomerFileInfoService.Services.EmailService
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Send(string toEmail, string subject, string body)
        {
            MailMessage mail = new MailMessage();
            mail.To.Add(toEmail);
            mail.From = new MailAddress(_configuration["Smtp:Username"]);
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;

            SmtpClient smtp = new SmtpClient(_configuration["Smtp:Host"], Convert.ToInt32(_configuration["Smtp:Port"]));
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(_configuration["Smtp:Username"], _configuration["Smtp:Password"]);
            smtp.Send(mail);
        }
    }
}
