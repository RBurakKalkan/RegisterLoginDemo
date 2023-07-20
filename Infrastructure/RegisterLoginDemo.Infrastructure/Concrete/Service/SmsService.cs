using Hangfire;
using Microsoft.Extensions.Configuration;
using RegisterLoginDemo.Application.Abstraction.Service;
using System.Net.Http.Json;

namespace RegisterLoginDemo.Infrastructure.Concrete.Service
{
    public class SmsService : ISmsService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly int _headerId;
        private readonly string _baseUrl;

        public SmsService(IConfiguration configuration)
        {
            _apiKey = configuration.GetValue<string>("SmsServiceProvider:ApiKey");
            _headerId = configuration.GetValue<int>("SmsServiceProvider:HeaderId");
            _baseUrl = configuration.GetValue<string>("SmsServiceProvider:BaseUrl");

            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("X-Organik-Auth", _apiKey);
        }


        public void SendVerificationCode(string recipient, string message)
        {
            BackgroundJob.Enqueue(() => SendSms(recipient, message));
        }

        public async Task SendSms(string recipients, string message)
        {
            var url = $"{_baseUrl}/sms/send";
            var payload = new
            {
                message = message,
                recipients,
                header = _headerId
            };

            var response = await _httpClient.PostAsJsonAsync(url, payload);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine($"SMS sent successfully. Recipient={recipients}, Message={message}");
            }
            else
            {
                Console.WriteLine($"Failed to send SMS. Recipient={recipients}, Message={message}");
            }
        }
    }
}
