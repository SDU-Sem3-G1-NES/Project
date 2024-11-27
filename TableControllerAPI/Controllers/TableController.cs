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
    private readonly ITableControllerService _tableControllerService;

    private readonly Progress<ITableStatusReport> _progress = new Progress<ITableStatusReport>(message =>
    {
        Debug.WriteLine(message);
    });
    public TableController(ITableControllerService tableControllerService)
    {
        _tableControllerService = tableControllerService;
    }
    [HttpGet("{guid}")]
    public async Task<ActionResult<ITable>> GetFullTableInfo(string guid)
    {
        try
        {
            var _tableController = await _tableControllerService.GetTableController(guid);
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
            var _tableController = await _tableControllerService.GetTableController(guid);
            await _tableController.SetTableHeight(height, guid, _progress);
            return Ok(await Task.FromResult("Table height set successfully."));
        }
        catch (HttpRequestException httpRequestException)
        {
            Debug.WriteLine(httpRequestException.Message);
            return StatusCode(503, await Task.FromResult("Failed to establish SSL connection."));
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