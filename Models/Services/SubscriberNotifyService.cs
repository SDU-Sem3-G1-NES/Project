using TableController;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Http;

namespace Models.Services
{
    public class SubscriberNotifyService : IHostedService
    {
        private readonly ISubscriberUriService _subscriberUriService;
        private readonly IHttpClientFactory _clientFactory;
        private readonly HttpClient _httpClient;
        private readonly ITableControllerService _tableControllerService;
        private readonly LinakSimulatorController? _linakSimulatorController;
        private readonly LinakTableController? _linakTableController;
        public SubscriberNotifyService(SubscriberUriService subscriberUriService, IHttpClientFactory clientFactory, TableControllerService tableControllerService)
        {
            _subscriberUriService = subscriberUriService;
            _clientFactory = clientFactory;
            _httpClient = _clientFactory.CreateClient("default");
            _tableControllerService = tableControllerService;

            _linakSimulatorController = _tableControllerService.GetTableControllerByApiName("Linak Simulator API V2", _httpClient).Result as LinakSimulatorController;
            if (_linakSimulatorController != null) _linakSimulatorController.OnTableHeightSet += NotifySubscribers;
            
            _linakTableController = _tableControllerService.GetTableControllerByApiName("Linak API", _httpClient).Result as LinakTableController;
            if (_linakTableController != null) _linakTableController.OnTableHeightSet += NotifySubscribers;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        private void NotifySubscribers(object? sender, TableHeightSetEventArgs eventArgs)
        {
            var tableGuid = eventArgs.Guid;
            var height = eventArgs.Height;
            var message = eventArgs.Message;
            var status = eventArgs.Status;
            dynamic infoObject = new { status = status, height = height, message = message };
            var json = JsonSerializer.Serialize(infoObject);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var uri = _subscriberUriService.GetByTableId(tableGuid);
            if (uri != "")
            {
                try
                {
                    var response = _httpClient.PostAsync(uri,content);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Failed to send status to {uri}. Error: {e.Message}");
                }
            }
        }
        
    }
}
