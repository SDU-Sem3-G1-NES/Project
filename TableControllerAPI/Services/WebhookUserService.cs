using TableControllerApi.Models;

namespace TableControllerApi.Services
{
    public class WebhookUserService
    {
        public List<Uri> Webhooks { get; set; } = new();
    }
}