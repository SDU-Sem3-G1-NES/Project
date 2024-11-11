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
        await _tableController.SetTableHeight(height, guid);
        return Ok(await Task.FromResult("Table height set successfully."));
        }
        catch (Exception e)
        {
            if(e.Message.Contains("Failed to set table height!"))
            {
                Debug.WriteLine(e.Message);
                return StatusCode(503, await Task.FromResult("Failed to set table height!"));
            }
            Debug.WriteLine(e.Message);
            return NotFound(await Task.FromResult("Table not found."));
        }
    }
}