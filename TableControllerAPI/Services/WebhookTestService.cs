
using System.Diagnostics;
using TableControllerApi.Models;

namespace TableControllerApi.Services
{
    public class WebhookTestService : BackgroundService
    {
        private readonly SubscriberUriService _subscriberUriService;
        private readonly HttpClient _httpClient;
        public WebhookTestService(SubscriberUriService subscriberUriService, IHttpClientFactory httpClientFactory)
        {
            _subscriberUriService = subscriberUriService;
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
                foreach (var webhook in _subscriberUriService.Webhooks)
                {
                    var uri = webhook.WebhookUri;
                    try
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
                        catch (Exception e)
                    {
                        Console.WriteLine($"Failed to send status to {uri} with error: {e.Message}");
                    }
                }
                await Task.Delay(10000, stoppingToken); // Wait for 5 seconds before sending the next status
            }
        }
    }
}