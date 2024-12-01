using Microsoft.AspNetCore.Mvc;
using Models.Services;
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
            try
            {
                if(_subscriberUriService.Add(guid, uri))
                {
                    return Ok(await Task.FromResult("Subscription added."));
                }
                else 
                {
                    return BadRequest(await Task.FromResult("Subscribtion failed. This table might already have a subscription. Are you using the right format?\n\"http://www.example.com/example\""));
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return BadRequest(await Task.FromResult("Subscribtion failed. " + e.Message));
            }
        }
        [HttpPost("{guid}/unsubscribe")]
        public async Task<ActionResult> RemoveWebhook(string guid)
        {
            try
            {
                if(_subscriberUriService.Remove(guid))
                {
                    return Ok(await Task.FromResult("Unsubscribed successfully."));
                }
                else 
                {
                    return NotFound(await Task.FromResult("Subscription not found"));
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return BadRequest(await Task.FromResult("Failed to unsubscribe. " + e.Message));
            }
        }
    }
}