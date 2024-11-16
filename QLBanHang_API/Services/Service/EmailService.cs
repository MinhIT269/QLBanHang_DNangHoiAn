﻿using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using QLBanHang_API.Dto.Request;
using QLBanHang_API.Services.IService;
using System.Net;
using System.Net.Mail;

namespace QLBanHang_API.Services.Service
{
    public class EmailService : IEmailService 
    {
        private readonly IConfiguration configuration;
        public EmailService (IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public async Task<bool> SendEmail(EmailRequest emailRequest)
        {
            try
            {
                var email = configuration.GetValue<string>("EMAIL_CONFIGURATION:EMAIL");
                var password = configuration.GetValue<string>("EMAIL_CONFIGURATION:PASSWORD");
                var host = configuration.GetValue<string>("EMAIL_CONFIGURATION:HOST");
                var port = configuration.GetValue<int>("EMAIL_CONFIGURATION:PORT");

                var smtpClient = new SmtpClient(host, port);
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(email, password);
                var message = new MailMessage()
                {
                    From = new MailAddress(email),
                    Subject = emailRequest.Subject,
                    Body = emailRequest.Body
                };
                message.To.Add(new MailAddress(emailRequest.EmailReceive));
                await smtpClient.SendMailAsync(message);
                return true;
            }
            catch (Exception ex) 
            {
                return false;
            }

        }
    }
}