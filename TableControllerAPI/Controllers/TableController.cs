using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SharedModels;
using TableController;
using Models.Services;

namespace TableControllerApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class TableController : ControllerBase
{
    private IHttpClientFactory _clientFactory { get; set; }
    private readonly ITableControllerService _tableControllerService;
    private HttpClient _client;
    private string statusMessage = "";

    private readonly Progress<ITableStatusReport> _progress;
    public TableController(ITableControllerService tableControllerService, IHttpClientFactory clientFactory)
    {
        _tableControllerService = tableControllerService;
        _clientFactory = clientFactory;
        _client = _clientFactory.CreateClient("default");
        _progress  = new Progress<ITableStatusReport>(message =>
        {
            Debug.WriteLine(message.Message);
            statusMessage = message.Message;
        });
    }
    [HttpGet("{guid}")]
    public async Task<ActionResult<ITable>> GetFullTableInfo(string guid)
    {
        try
        {
            var _tableController = await _tableControllerService.GetTableController(guid, _client);
            ITable? table = await _tableController.GetFullTableInfo(guid);
            return Ok(await Task.FromResult(table));
        }
        catch (Exception e)
        {
            Debug.WriteLine(e.Message);
            return NotFound(await Task.FromResult("Table not found."));
        }
    }
    [HttpPut("{guid}/height")]
    public async Task<ActionResult> SetTableHeight(string guid, [FromBody] int height)
    {
        try
        {
            statusMessage = "Table height set successfully.";
            var _tableController = await _tableControllerService.GetTableController(guid, _client);
            await _tableController.SetTableHeight(height, guid, _progress);

            return Ok(await Task.FromResult(statusMessage));
        }
        catch (Exception e)
        {
            if (e.Message.Contains("Failed to set table height!"))
            {
                Debug.WriteLine(e.Message);
                return StatusCode(503, await Task.FromResult("Failed to set table height!"));
            }
            Debug.WriteLine(e.Message);
            return NotFound(await Task.FromResult("Table not found."));
        }
    }
}