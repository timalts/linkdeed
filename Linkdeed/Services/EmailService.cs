using Newtonsoft.Json;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System;
using Microsoft.CodeAnalysis;

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
            return await ExecuteEmail("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", subject, message, emails);
        }

        public async Task<Response> ExecuteEmail(string apiKey, string subject, string message, List<string> emails)
        {
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("emeric.du-gardin@epita.fr", "Dorset College"),
                Subject = subject
            };
            
            msg.SetTemplateId("d-97a23db3148d47b6903ee3a453252857");
            msg.SetTemplateData(new SendgridForgotPassword
            {
                Password = message
            });

            //msg.AddSubstitution("{{Password}}", message);
            foreach (var email in emails)
            {
                msg.AddTo(new EmailAddress(email));
            }

            

            Response response = await client.SendEmailAsync(msg);
            Console.WriteLine(response.StatusCode);
            return response;
        }

        private class SendgridForgotPassword
        {
            [JsonProperty("Password")]
            public string Password { get; set; }
        }
    }
}
