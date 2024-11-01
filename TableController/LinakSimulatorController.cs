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

public class LinakSimulatorController : ITableController
{
    private LinakTable? _table;
    private ILinakSimulatorTasks _tasks;


    public LinakSimulatorController(LinakTable? table = null)
    {
        _table = table;
        _tasks = new LinakSimulatorTasks();
    }

    public string[] GetAllTableIds()
    {
        try 
        {
            var response = _tasks.GetAllTableIds().Result;
            return response;
        } 
        catch (Exception e) 
        {
            Debug.WriteLine(e.Message);
            return new string[] {};
        }
    }

    public LinakTable? GetFullTableInfo()
    {
        try 
        {
            var response = _tasks.GetTableInfo(_table!.GUID).Result;
            if (response == null) throw new Exception("Table not found on API!");

            _table.Name = response.name!;
            _table.Height = response.position;
            _table.Speed = response.speed;
            return _table;
        } 
        catch (Exception e) 
        {
            Debug.WriteLine(e.Message);
            return null;
        }
    }

    public int GetTableHeight()
    {
        try 
        {
            var response = _tasks.GetTableInfo(_table!.GUID).Result;
            if (response == null) throw new Exception("Table not found on API!");
            _table.Height = response.position;
            return response.position ?? -1;
        } 
        catch (Exception e) 
        {
            Debug.WriteLine(e.Message);
            return -1;
        }
        
    }

    public void SetTableHeight(int height)
    {
        _table!.Height = height;
        var tempTable = new LinakApiTable {id = _table.GUID, position = _table.Height};
        var response = _tasks.SetTableInfo(tempTable).Result;

        // Because return type is void, we must throw exceptions if something goes wrong
        if (!response.IsSuccessStatusCode) throw new Exception("Failed to set table height!"); 
    }

    public int GetTableSpeed()
    {
        try 
        {
            var response = _tasks.GetTableInfo(_table!.GUID).Result;
            if (response == null) throw new Exception("Table not found on API!");
            _table.Speed = response.speed;
            return response.speed ?? -1;
        } 
        catch (Exception e) 
        {
            Debug.WriteLine(e.Message);
            return -1;
        }
    }

    public string GetTableStatus()
    {
        try 
        {
            var response = _tasks.GetTableInfo(_table!.GUID).Result;
            if (response == null) throw new Exception("Table not found on API!");
            return response.status ?? "";
        } 
        catch (Exception e) 
        {
            Debug.WriteLine(e.Message);
            return "";
        }
    }

    public void GetTableError()
    {
        throw new NotImplementedException();
    }

    public List<ITableError>? ErrorList { get; }

    public int GetActivationCounter()
    {
        throw new NotImplementedException();
    }

    public void GetSitStandCounter()
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