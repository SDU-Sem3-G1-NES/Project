using System.Diagnostics;
using System.Net;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;
using DotNetEnv;

using SharedModels;

namespace Famicom.TableController;

public class LinakSimulatorController : ITableController
{
    private LinakTable _table;


    public LinakSimulatorController(LinakTable table)
    {
        _table = table;
    }

    public int GetTableHeight()
    {
        try 
        {
            var response = LinakSimulatorTasks.GetTableInfo(_table.GUID).Result;
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
        _table.Height = height;
        var tempTable = new ApiTable {position = _table.Height};
        var response = LinakSimulatorTasks.SetTableInfo(tempTable).Result;

        // Because return type is void, we must throw exceptions if something goes wrong
        if (!response.IsSuccessStatusCode) throw new Exception("Failed to set table height!"); 
    }

    public int GetTableSpeed()
    {
        try 
        {
            var response = LinakSimulatorTasks.GetTableInfo(_table.GUID).Result;
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
            var response = LinakSimulatorTasks.GetTableInfo(_table.GUID).Result;
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

internal static class LinakSimulatorTasks {
    private static readonly HttpClient _client = new HttpClient();
    private static readonly LinakSimulatorControllerOptions _options = new LinakSimulatorControllerOptions();
    private static readonly string _baseUrl = $"{_options.Url}:{_options.Port}/api/{_options.Version}/{_options.Key}";

    public static async Task<ApiTable?> GetTableInfo(string guid) {
        var url = $"{_baseUrl}/table/{guid}";
        var response = await _client.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var jsonString = response.Content.ReadAsStringAsync();
        var apiTable = JsonSerializer.Deserialize<ApiTable>(jsonString.Result);

        return apiTable;
    }

    public static async Task<HttpResponseMessage> SetTableInfo(ApiTable table) {
        if(table.id == null) throw new Exception("Table ID is null!");
        var tempTable = GetTableInfo(table.id);
        if(tempTable == null) throw new Exception("Table not found on API!");

        var url = $"{_baseUrl}/table/{table.id}";
        
        // Loop through all non-null properties of ApiTable, serialise, and send to web API
        foreach (PropertyInfo prop in table.GetType().GetProperties().Where(x => x != null))
        {
            var json = JsonSerializer.Serialize(prop.GetValue(table));
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync(url, content);
            response.EnsureSuccessStatusCode();
            var responseBody = response.Content.ReadAsStringAsync();
            var serialisedResponseBody = JsonSerializer.Deserialize(responseBody.Result, prop.PropertyType);
            
            // response.EnsureSuccessStatusCode() will throw an exception if the status code is not 200, however, it is a good idea to have 
            // a check here to ensure that the response body is what we expect it to be.
            if(!prop.GetValue(table)!.Equals(serialisedResponseBody)) throw new Exception($"Failed to set table {prop.Name}!");
        }

        return new HttpResponseMessage(HttpStatusCode.OK);
    }


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
internal class ApiTable {
    public string? id { get; set; }
    public string? name { get; set; }
    public string? manufacturer { get; set; }
    public int? position { get; set; }
    public int? speed { get; set; }
    public string? status { get; set; }
}