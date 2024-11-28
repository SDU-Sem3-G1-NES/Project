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
        private readonly SubscriberUriService _subscriberUriService;
        public WebhookController(SubscriberUriService subscriberUriService)
        {
            _subscriberUriService = subscriberUriService;
        }

        [HttpPost("{guid}/subscribe")]
        public async Task<ActionResult> ReceiveWebhook(string guid, [FromBody] String uri)
        {
            Debug.WriteLine($"Received webhook subscription for table {guid} with URI: {uri}");
            try
            {
                if(_subscriberUriService.Add(guid, uri))
                {
                    return Ok(await Task.FromResult("Webhook added successfully."));
                }
                else 
                {
                    return BadRequest(await Task.FromResult("Failed to add webhook. Are you using the right format?\n\"http://www.example.com/example\""));
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return BadRequest(await Task.FromResult("Failed to add webhook. " + e.Message));
            }
        }
        [HttpPost("{guid}/unsubscribe")]
        public async Task<ActionResult> RemoveWebhook(string guid, [FromBody] String uri)
        {
            Debug.WriteLine($"Received webhook unsubscribe for table {guid} with URI: {uri}");
            try
            {
                if(_subscriberUriService.Remove(guid, uri))
                {
                    return Ok(await Task.FromResult("Webhook removed successfully."));
                }
                else 
                {
                    return NotFound(await Task.FromResult("Subscription not found"));
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return BadRequest(await Task.FromResult("Failed to remove webhook. " + e.Message));
            }
        }
    }
}