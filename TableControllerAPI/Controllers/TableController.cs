using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SharedModels;
using TableController;
using Models.Services;
using Microsoft.AspNetCore.Components;

namespace TableControllerApi.Controllers;

[ApiController]
[Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
[Produces("application/json")]
public class TableController : ControllerBase
{
    private IHttpClientFactory _clientFactory { get; set; }
    private readonly ITableControllerService _tableControllerService;

    private readonly Progress<ITableStatusReport> _progress = new Progress<ITableStatusReport>(message =>
    {
        Debug.WriteLine(message);
    });
    public TableController(ITableControllerService tableControllerService, IHttpClientFactory clientFactory)
    {
        _tableControllerService = tableControllerService;
        _clientFactory = clientFactory;
    }
    [HttpGet("{guid}")]
    public async Task<ActionResult<ITable>> GetFullTableInfo(string guid)
    {
        try
        {
            //ignore SSL errors instad of fixing them :)
            var handler = new HttpClientHandler
            {
            ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            };
            var client = new HttpClient(handler);
            //var client = _clientFactory!.CreateClient("default");
            var _tableController = await _tableControllerService.GetTableController(guid, client);
            ITable? table = await _tableController.GetFullTableInfo(guid);
            return Ok(await Task.FromResult(table));
        }
        catch (HttpRequestException httpRequestException)
        {
            Debug.WriteLine(httpRequestException.Message);
            return StatusCode(503, await Task.FromResult("Failed to establish SSL connection."));
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
            //ignore SSL errors instad of fixing them :)
            var handler = new HttpClientHandler
            {
            ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            };
            var client = new HttpClient(handler);
            //var client = _clientFactory!.CreateClient("default");
            var _tableController = await _tableControllerService.GetTableController(guid, client);
            await _tableController.SetTableHeight(height, guid, _progress);
            return Ok(await Task.FromResult("Table height set successfully."));
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