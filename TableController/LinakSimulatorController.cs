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
                || response.config.name == null
                || response.state.position_mm == null) 
            {
                return Task.FromException<LinakTable>(new Exception("Table not found on API!"));
            } 

            var returnTable = new LinakTable(
                response.id,
                response.config.name
            );
            returnTable.Height = response.state.position_mm;
            returnTable.Speed = response.state.speed_mm; 

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
                || response.config.name == null
                || response.state.position_mm == null) 
            {
                return Task.FromException<int>(new Exception("Table not found on API!"));
            } 

            return Task.FromResult(response.state.position_mm.Value);
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
    public async Task SetTableHeight(int height, string guid)
    {
        try {
            var tempTable = new LinakApiTable {id = guid, state = new LinakApiTableState()};
            tempTable.state.position_mm = height;
            var response = _tasks.SetTableInfo(tempTable).Result;
            var result = await _tasks.WatchTableAsItMoves(guid, height);

            // Because return type is void, we must throw exceptions if something goes wrong
            if (!response.IsSuccessStatusCode) await Task.FromException(new Exception("Failed to set table height!"));
            await Task.CompletedTask; 

        } 
        catch (Exception e) 
        {
            Debug.WriteLine(e.Message);
            await Task.FromException(new Exception(e.Message));
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
            if(response.state.speed_mm == null) return Task.FromException<int>(new Exception("Could not get speed."));
            return Task.FromResult(response.state.speed_mm!.Value);
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
            return Task.FromResult(response.state.status ?? "");
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
    private readonly  HttpClient _client = null!;
    private readonly LinakSimulatorControllerOptions _options;
    private readonly string _baseUrl;

    internal LinakSimulatorTasks() {
        _options = new LinakSimulatorControllerOptions();
        _baseUrl = $"https://{_options.Url}:{_options.Port}/api/{_options.Version}/{_options.Key}";

        var handler = new HttpClientHandler {
            ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
        };
        _client = new HttpClient(handler);
    }

    public async Task<LinakApiTable?> GetTableInfo(string guid) {
        var url = $"{_baseUrl}/desks/{guid}";
        var response = await _client.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var jsonString = response.Content.ReadAsStringAsync();
        var apiTable = JsonSerializer.Deserialize<LinakApiTable>(jsonString.Result);

        apiTable!.id = guid;
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
            var requestUrl = $"{url}/{prop.Name}";
            var data = new Dictionary<string, object> {};

            foreach (PropertyInfo subProp in prop.GetValue(table)!.GetType().GetProperties().Where(x => x.GetValue(prop.GetValue(table)) != null))
            {
                if(subProp.GetValue(prop.GetValue(table)) != null) data.Add(subProp.Name, subProp.GetValue(prop.GetValue(table))!);
            }

            var json = JsonSerializer.Serialize(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.PutAsync(requestUrl, content);
            response.EnsureSuccessStatusCode();
            var responseBody = response.Content.ReadAsStringAsync().Result;
            var serialisedResponseBody = JsonSerializer.Deserialize<Dictionary<string, object>>(responseBody);
            
            // response.EnsureSuccessStatusCode() will throw an exception if the status code is not 200, however, it is a good idea to have 
            // a check here to ensure that the response body is what we expect it to be.
            if(data.Any(x => x.Value.Equals(serialisedResponseBody![x.Key.ToString()]))) {
                throw new Exception("Response body does not match request body.");
            }
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

    public async Task<int> WatchTableAsItMoves(string guid, int newPosition) {

        // Get last error
        var tempTable = await GetTableInfo(guid);
        var lastError = tempTable!.lastErrors.LastOrDefault();

        bool check = true;
        while(check) {
            var table = await GetTableInfo(guid);

            // GET errors
            if(table == null) throw new Exception("Table not found on API!");
            if(table.state.position_mm == null) throw new Exception("Could not get position.");

            // Table Errors
            if(table.lastErrors.LastOrDefault()!.time_s < lastError!.time_s || 
                (table.lastErrors.LastOrDefault()!.time_s != null && lastError!.time_s == null)) 
            {
                return (int)table.lastErrors.LastOrDefault()!.errorCode!;
            }

            // Other Table Properties relating to collisions, etc...
            if(table.state.isPositionLost ?? false) {
                return 1;
            }
            if(table.state.isOverloadProtectionUp ?? false) {
                return 2;
            }
            if(table.state.isOverloadProtectionDown ?? false) {
                return 3;
            }
            if(table.state.isAntiCollision ?? false) {
                return 9;
            }

            // Success if table has reached new position
            if(table.state.position_mm == newPosition) {
                check = false;
            }
            await Task.Delay(200); 
        }
        return 0;
    }
}

internal interface ILinakSimulatorTasks {
    Task<LinakApiTable?> GetTableInfo(string guid);
    Task<HttpResponseMessage> SetTableInfo(LinakApiTable table);
    Task<string[]> GetAllTableIds();
    Task<int> WatchTableAsItMoves(string guid, int newPosition);
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
    public string id { get; set; } = null!;
    public LinakApiTableConfig config { get; set; } = null!;
    public LinakApiTableState state { get; set; } = null!;
    public LinakApiTableUsage usage { get; set; } = null!;
    public LinakApiTableError[] lastErrors { get; set; } = null!;
}

[Serializable]
internal class LinakApiTableConfig {
    public string? name { get; set; }
    public string? manufacturer { get; set; }
}

[Serializable]
internal class LinakApiTableState {
    public int? position_mm { get; set; }
    public int? speed_mm { get; set; }
    public string? status { get; set; }
    public bool? isPositionLost { get; set; }
    public bool? isOverloadProtectionUp { get; set; }
    public bool? isOverloadProtectionDown { get; set; }
    public bool? isAntiCollision { get; set; }
}

[Serializable]
internal class LinakApiTableUsage {
    public int? activationCounter { get; set; }
    public int? sitStandCounter { get; set; }
}

[Serializable]
internal class LinakApiTableError {
    public int? time_s { get; set; }
    public int? errorCode { get; set; }
}