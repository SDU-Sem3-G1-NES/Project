using System.Diagnostics;
using System.Net;
using System.Reflection;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using DotNetEnv;
using SharedModels;
using Sprache;

namespace TableController;

/// <summary>
/// Table Controller for the Linak Simulator API given by Krzysztof.
/// </summary>
public class LinakSimulatorController : ITableController
{
    private ILinakSimulatorTasks _tasks;

    /// <summary>
    /// Constructor for LinakSimulatorController
    /// </summary>
    public LinakSimulatorController()
    {
        _tasks = new LinakSimulatorTasks();
    }

    /// <summary>
    /// Method to get all table GUIDs stored in the API.
    /// </summary>
    /// <returns>Array of all found GUIDs as string.</returns>
    public Task<string[]> GetAllTableIds()
    {
        try 
        {
            var response = _tasks.GetAllTableIds().Result;
            return Task.FromResult(response);
        } 
        catch (Exception e) 
        {
            Debug.WriteLine(e.Message);
            return Task.FromException<string[]>(new Exception(e.Message));
        }
    }

    /// <summary>
    /// Method to get full information of table stored in TableControlelr from the API.
    /// </summary>
    /// <returns>LinakTable; Null if not found in API or on other error.</returns>
    /// <param name="guid">GUID of the table to get information from. Optional.</param>
    public Task<LinakTable> GetFullTableInfo(string guid)
    {
        try 
        {
            var response = _tasks.GetTableInfo(guid).Result;
            if (response == null
                || response.id == null
                || response.name == null
                || response.position == null) 
            {
                return Task.FromException<LinakTable>(new Exception("Table not found on API!"));
            } 

            var returnTable = new LinakTable(
                response.id,
                response.name
            );
            returnTable.Height = response.position;
            returnTable.Speed = response.speed; 

            return Task.FromResult(returnTable);
        } 
        catch (Exception e) 
        {
            Debug.WriteLine(e.Message);
            return Task.FromException<LinakTable>(new Exception(e.Message));
        }
    }

    /// <summary>
    /// Method to get the height of the table stored in TableController from the API.
    /// </summary>
    /// <returns>Height as int; -1 if table not found or other error.</returns>
    /// <param name="guid">GUID of the table to get height from.</param>
    public Task<int> GetTableHeight(string guid)
    {
        try 
        {
            var response = _tasks.GetTableInfo(guid).Result;
            if (response == null
                || response.id == null
                || response.name == null
                || response.position == null) 
            {
                return Task.FromException<int>(new Exception("Table not found on API!"));
            } 

            return Task.FromResult(response.position.Value);
        } 
        catch (Exception e) 
        {
            Debug.WriteLine(e.Message);
            return Task.FromException<int>(new Exception(e.Message));
        }
        
    }


    /// <summary>
    /// Method to set the height of the table stored in TableController with the API.
    /// </summary>
    /// <param name="height">New Height for the table.</param>
    /// <param name="guid">GUID of the table to set height for.</param>
    /// <exception cref="Exception">Thrown if anything went wrong in the process.</exception>
    public Task SetTableHeight(int height, string guid)
    {
        try {
            var tempTable = new LinakApiTable {id = guid, position = height};
            var response = _tasks.SetTableInfo(tempTable).Result;

            // Because return type is void, we must throw exceptions if something goes wrong
            if (!response.IsSuccessStatusCode) return Task.FromException(new Exception("Failed to set table height!"));
            return Task.CompletedTask; 

        } 
        catch (Exception e) 
        {
            Debug.WriteLine(e.Message);
            return Task.FromException(new Exception(e.Message));
        }
        
    }

    /// <summary>
    /// Method to get the speed of the table stored in TableController from the API.
    /// </summary>
    /// <returns>TableSpeed as int; -1 if table not found or other error.</returns>
    /// <param name="guid">GUID of the table to get speed from.</param>
    public Task<int> GetTableSpeed(string guid)
    {
        try 
        {
            var response = _tasks.GetTableInfo(guid).Result;
            if (response == null) throw new Exception("Table not found on API!");
            if(response.speed == null) return Task.FromException<int>(new Exception("Could not get speed."));
            return Task.FromResult(response.speed!.Value);
        } 
        catch (Exception e) 
        {
            Debug.WriteLine(e.Message);
            return Task.FromException<int>(new Exception(e.Message));
        }
    }


    /// <summary>
    /// Method to get the status of the table stored in TableController from the API.
    /// </summary>
    /// <returns>Status as string. Empty string if no status or some error.</returns>
    /// <param name="guid">GUID of the table to get status from.</param>
    public Task<string> GetTableStatus(string guid)
    {
        try 
        {
            var response = _tasks.GetTableInfo(guid).Result;
            if (response == null) throw new Exception("Table not found on API!");
            return Task.FromResult(response.status ?? "");
        } 
        catch (Exception e) 
        {
            Debug.WriteLine(e.Message);
            return Task.FromException<string>(new Exception(e.Message));
        }
    }

    public Task GetTableError(string guid)
    {
        throw new NotImplementedException();
    }

    public Task<int> GetActivationCounter(string guid)
    {
        throw new NotImplementedException();
    }

    public Task GetSitStandCounter(string guid)
    {
        throw new NotImplementedException();
    }
}

internal class LinakSimulatorTasks : ILinakSimulatorTasks {
    private readonly  HttpClient _client = new HttpClient();
    private readonly LinakSimulatorControllerOptions _options;
    private readonly string _baseUrl;

    internal LinakSimulatorTasks() {
        _options = new LinakSimulatorControllerOptions();
        _baseUrl = $"http://{_options.Url}:{_options.Port}/api/{_options.Version}/{_options.Key}";
    }

    public async Task<LinakApiTable?> GetTableInfo(string guid) {
        var url = $"{_baseUrl}/desks/{guid}";
        var response = await _client.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var jsonString = response.Content.ReadAsStringAsync();
        var apiTable = JsonSerializer.Deserialize<LinakApiTable>(jsonString.Result);

        return apiTable ?? new LinakApiTable();
    }

    public async Task<HttpResponseMessage> SetTableInfo(LinakApiTable table) {
        if(table.id == null) throw new Exception("Table ID is null!");
        var tempTable = GetTableInfo(table.id);
        if(tempTable == null) throw new Exception("Table not found on API!");

        var url = $"{_baseUrl}/desks/{table.id}";
        
        // Loop through all non-null properties of ApiTable, serialise, and send to web API
        foreach (PropertyInfo prop in table.GetType().GetProperties().Where(x => x.GetValue(table) != null && x.Name != "id"))
        {
            var data = new Dictionary<string, object> { { prop.Name, prop.GetValue(table)! } };
            var json = JsonSerializer.Serialize(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.PutAsync(url, content);
            response.EnsureSuccessStatusCode();
            var responseBody = response.Content.ReadAsStringAsync().Result;
            var serialisedResponseBody = JsonSerializer.Deserialize<Dictionary<string, object>>(responseBody);
            
            // response.EnsureSuccessStatusCode() will throw an exception if the status code is not 200, however, it is a good idea to have 
            // a check here to ensure that the response body is what we expect it to be.
            if(data[prop.Name].ToString() != serialisedResponseBody![prop.Name].ToString()) new Exception($"Failed to set table {prop.Name}!");
        }

        return new HttpResponseMessage(HttpStatusCode.OK);
    }

    public async Task<string[]> GetAllTableIds() {
        var url = $"{_baseUrl}/desks";
        var response = await _client.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var jsonString = response.Content.ReadAsStringAsync();
        var apiTables = JsonSerializer.Deserialize<string[]>(jsonString.Result);

        return apiTables ?? [];
    }
}

internal interface ILinakSimulatorTasks {
    Task<LinakApiTable?> GetTableInfo(string guid);
    Task<HttpResponseMessage> SetTableInfo(LinakApiTable table);
    Task<string[]> GetAllTableIds();
}

internal class LinakSimulatorControllerOptions {
    public string Url {get ; set;}
    public string Port {get ; set;}
    public string Version {get ; set;}
    public string Key {get ; set;}

    public LinakSimulatorControllerOptions()
    {
        var envPath = Path.Combine(AppContext.BaseDirectory, ".env");
        Env.Load(envPath);

        Url = Environment.GetEnvironmentVariable("LSAPI_URL")!;
        Port = Environment.GetEnvironmentVariable("LSAPI_PORT")!;
        Version = Environment.GetEnvironmentVariable("LSAPI_VERSION")!;
        Key = Environment.GetEnvironmentVariable("LSAPI_KEY")!;
    }
}

[Serializable]
internal class LinakApiTable {
    public string? id { get; set; }
    public string? name { get; set; }
    public string? manufacturer { get; set; }
    public int? position { get; set; }
    public int? speed { get; set; }
    public string? status { get; set; }
}