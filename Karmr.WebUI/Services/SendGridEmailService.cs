using System;
using System.Net;
using System.Threading.Tasks;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Karmr.WebUI.Services
{
    public sealed class SendGridEmailService : IEmailService
    {
        private readonly string apiKey;

        public SendGridEmailService(string apiKey)
        {
            this.apiKey = apiKey;
        }

        public async Task SendPasswordReset(string to, string resetUrl)
        {
            await this.SendEmail(to, "d-17e1b35fd54347059401e11d7e4ac3ca", new {resetUrl = resetUrl});
        }

        public async Task SendPasswordChangeNotification(string to)
        {
            await this.SendEmail(to, "d-49d400d45ab04f2d9f6e02c2d08ee81b");
        }

        private async Task SendEmail(string to, string templateId, object templateData = null)
        {
            var msg = new SendGridMessage();
            msg.AddTo(new EmailAddress(to));
            msg.From = new EmailAddress("info@karmr.azurewebsites.net");
            msg.TemplateId = templateId;
            if (templateData != null)
            {
                msg.SetTemplateData(templateData);
            }
            var client = new SendGridClient(this.apiKey);
            var response = await client.SendEmailAsync(msg);
            if (response.StatusCode != HttpStatusCode.Accepted)
            {
                throw new Exception($"Failed to send email: {response.StatusCode}");
            }
        }
    }
}