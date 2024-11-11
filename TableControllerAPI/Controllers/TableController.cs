using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SharedModels;
using TableController;

namespace TableControllerApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class TableController : ControllerBase
{
    private readonly ITableController _tableController;
    public TableController(ITableController tableController)
    {
        _tableController = tableController;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<string>>> GetTables()
    {
        var tableIds = await _tableController.GetAllTableIds();
        if (tableIds.Length == 0) return NotFound(await Task.FromResult("No tables found."));
        return Ok(await Task.FromResult(tableIds));
    }
    /*[HttpGet("{guid}/height")]
    public async Task<ActionResult<int>> GetTableHeight(string guid)
    {
        var height = _tableController.GetTableHeight(guid);
        if (height == -1) return NotFound(await Task.FromResult("Table not found."));
        return Ok(await Task.FromResult(height));
    }
    [HttpPut("{guid}/height")]
    public async Task<ActionResult> SetTableHeight(string guid, [FromBody] int height)
    {
        _tableController.SetTableHeight(height, guid);
        if (_tableController.GetTableHeight(guid) == height)
        {
            return Ok(await Task.FromResult("Table height set successfully."));
        }
        return BadRequest(await Task.FromResult("Failed to set table height."));
    }
    [HttpGet("{guid}/speed")]
    public async Task<ActionResult<int>> GetTableSpeed(string guid)
    {
        var speed = _tableController.GetTableSpeed(guid);
        if (speed == -1) return NotFound(await Task.FromResult("Table not found."));
        return Ok(await Task.FromResult(speed));
    }
    [HttpGet("{guid}/status")]
    public async Task<ActionResult<string>> GetTableStatus(string guid)
    {
        var status = _tableController.GetTableStatus(guid);
        if (status == "") return NotFound(await Task.FromResult("Table not found."));
        return Ok(await Task.FromResult(status));
    }
    [HttpGet("{guid}/error")]
    public async Task<ActionResult<List<ITableError>>> GetTableError(string guid)
    {
        _tableController.GetTableError(guid);
        return Ok(await Task.FromResult(_tableController.ErrorList));
    }*/
}