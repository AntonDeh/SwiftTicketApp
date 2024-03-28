using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace SwiftTicketApp.Services
{
    public class MailgunEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;

        public MailgunEmailService(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }

        public async Task SendEmailAsync(string to, string subject, string htmlContent)
        {
            var apiKey = _configuration["Mailgun:ApiKey"];
            var domain = _configuration["Mailgun:Domain"];
            var sender = _configuration["Mailgun:Sender"];

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes($"api:{apiKey}")));

#pragma warning disable CS8601 // Possible null reference assignment.
            var formData = new Dictionary<string, string>
            {
                ["from"] = sender,
                ["to"] = to,
                ["subject"] = subject,
                ["html"] = htmlContent
            };
#pragma warning restore CS8601 // Possible null reference assignment.

            var content = new FormUrlEncodedContent(formData);
            var response = await client.PostAsync($"https://api.mailgun.net/v3/{domain}/messages", content);
            response.EnsureSuccessStatusCode();
        }
    }
}
