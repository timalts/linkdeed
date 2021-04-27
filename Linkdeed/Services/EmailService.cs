﻿using Newtonsoft.Json;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Linkdeed.Services
{
    public interface IEmailService
    {
        Task<Response> SendEmailAsync(List<string> emails, string subject, string message);
    }
    public class EmailService : IEmailService
    {
        public IConfiguration Configuration { get; }

        public EmailService(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public async Task<Response> SendEmailAsync(List<string> emails, string subject, string message)
        {
            return await ExecuteEmail(Configuration.GetSection("Sendgrid").GetSection("Key").Value, subject, message, emails);
        }

        public async Task<Response> ExecuteEmail(string apiKey, string subject, string message, List<string> emails)
        {
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                // you will need your own email address here which has been added in sendgrid as an authorized sender
                From = new EmailAddress("nabillucky7@gmail.com", "Dorset College"),
                Subject = subject
            };

            msg.SetTemplateId(Configuration.GetSection("Sendgrid").GetSection("TemplateID").Value);
            msg.SetTemplateData(new SendgridForgotPassword
            {
                Password = message
            });

            foreach (var email in emails)
            {
                msg.AddTo(new EmailAddress(email));
            }

            Response response = await client.SendEmailAsync(msg);
            return response;
        }

        private class SendgridForgotPassword
        {
            [JsonProperty("password")]
            public string Password { get; set; }
        }
    }
}
