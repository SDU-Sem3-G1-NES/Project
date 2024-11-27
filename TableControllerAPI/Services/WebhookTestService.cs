
using TableControllerApi.Models;

namespace TableControllerApi.Services
{
    public class WebhookTestService : BackgroundService
    {
        private readonly WebhookUserService _webhookUserService;
        private readonly HttpClient _httpClient;
        public WebhookTestService(WebhookUserService webhookUserService, IHttpClientFactory httpClientFactory)
        {
            _webhookUserService = webhookUserService;
            _httpClient = httpClientFactory.CreateClient();
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var Time = DateTime.Now;
                var newStatus = new StatusChange // change to table data
                {
                    Status = "Test Status at: " + Time,
                };
                foreach (var uri in _webhookUserService.Webhooks)
                {
                    var response = await _httpClient.PostAsJsonAsync(uri, newStatus, stoppingToken);
                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine($"Successfully sent status to {uri}");
                    }
                    else
                    {
                        Console.WriteLine($"Failed to send status to {uri}");
                    } 
                }
                await Task.Delay(5000, stoppingToken); // Wait for 5 seconds before sending the next status
            }
        }
    }
}