using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TableControllerApi.Services;
using TableControllerApi.Models;
using System.Diagnostics;

namespace YourNamespace.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WebhookController : ControllerBase
    {
        private readonly WebhookUserService _webhookUserService;
        public WebhookController(WebhookUserService webhookUserService)
        {
            _webhookUserService = webhookUserService;
        }

        [HttpPost("subscribe")]
        public async Task<ActionResult> ReceiveWebhook([FromBody] String uri)
        {
            Debug.WriteLine("Received webhook with URI: " + uri);
            _webhookUserService.Webhooks.Add(new Uri(uri));
            return Ok(await Task.FromResult("Webhook received successfully with URI: " + uri));
        }
    }
}