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
    private readonly HttpClient _client;

    /// <summary>
    /// Constructor for LinakSimulatorController
    /// </summary>
    public LinakSimulatorController(HttpClient client)
    {
        _client = client;
        _tasks = new LinakSimulatorTasks(_client);
    }

    /// <summary>
    /// Method to get all table GUIDs stored in the API.
    /// </summary>
    /// <returns>Array of all found GUIDs as string.</returns>
    public async Task<string[]> GetAllTableIds()
    {
        try 
        {
            var response = await _tasks.GetAllTableIds();
            return response;
        } 
        catch (Exception e) 
        {
            Debug.WriteLine(e.Message);
            throw new Exception(e.Message);
        }
    }

    /// <summary>
    /// Method to get full information of table stored in TableControlelr from the API.
    /// </summary>
    /// <returns>LinakTable; Null if not found in API or on other error.</returns>
    /// <param name="guid">GUID of the table to get information from. Optional.</param>
    public async Task<LinakTable> GetFullTableInfo(string guid)
    {
        try 
        {
            var response = await _tasks.GetTableInfo(guid);
            if (response == null
                || response.id == null
                || response.config.name == null
                || response.state.position_mm == null) 
            {
                throw new Exception("Table not found on API!");
            } 

            var returnTable = new LinakTable(
                response.id,
                response.config.name
            );
            returnTable.Height = response.state.position_mm;
            returnTable.Speed = response.state.speed_mms; 

            return returnTable;
        } 
        catch (Exception e) 
        {
            Debug.WriteLine(e.Message);
            throw new Exception(e.Message);
        }
    }

    /// <summary>
    /// Method to get the height of the table stored in TableController from the API.
    /// </summary>
    /// <returns>Height as int; -1 if table not found or other error.</returns>
    /// <param name="guid">GUID of the table to get height from.</param>
    public async Task<int> GetTableHeight(string guid)
    {
        try 
        {
            var response = await _tasks.GetTableInfo(guid);
            if (response == null
                || response.id == null
                || response.config.name == null
                || response.state.position_mm == null) 
            {
                throw new Exception("Table not found on API!");
            } 

            return response.state.position_mm.Value;
        } 
        catch (Exception e) 
        {
            Debug.WriteLine(e.Message);
            throw new Exception(e.Message);
        }
        
    }


    /// <summary>
    /// Method to set the height of the table stored in TableController with the API.
    /// </summary>
    /// <param name="height">New Height for the table.</param>
    /// <param name="guid">GUID of the table to set height for.</param>
    /// <exception cref="Exception">Thrown if anything went wrong in the process.</exception>
    /*public async Task SetTableHeight(int height, string guid, IProgress<ITableStatusReport> progress)
    {
        var taskProgress = new Progress<int>(message =>
        {
            var parsedStattus = ParseTableStatus(message);
            progress.Report( 
                new LinakStatusReport(guid, parsedStattus.Keys.First(), parsedStattus.Values.First())
                );
        });
        try {
            var tempTable = new LinakApiTable {id = guid, state = new LinakApiTableState()};
            tempTable.state.position_mm = height;
            var response = await _tasks.SetTableInfo(tempTable);
            var result = await _tasks.WatchTableAsItMoves(guid, height, taskProgress);

            // Because return type is void, we must throw exceptions if something goes wrong
            if (!response.IsSuccessStatusCode) await Task.FromException(new Exception("Failed to set table height!"));
            await Task.CompletedTask; 

        } 
        catch (Exception e) 
        {
            Debug.WriteLine(e.Message);
            await Task.FromException(new Exception(e.Message));
        }
        
    }*/
    public async Task SetTableHeight(int height, string guid, IProgress<ITableStatusReport> progress)
    {
        var taskProgress = new Progress<int>(message =>
        {
            var parsedStattus = ParseTableStatus(message);
            progress.Report( 
                new LinakStatusReport(guid, parsedStattus.Keys.First(), parsedStattus.Values.First())
                );
        });
        try {
            var response = await _tasks.SetTableHeight(height, guid);
            var result = await _tasks.WatchTableAsItMoves(guid, height, taskProgress);

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
    public async Task<int> GetTableSpeed(string guid)
    {
        try 
        {
            var response = await _tasks.GetTableInfo(guid);
            if (response == null) throw new Exception("Table not found on API!");
            if(response.state.speed_mms == null) throw new Exception("Could not get speed.");
            return response.state.speed_mms!.Value;
        } 
        catch (Exception e) 
        {
            Debug.WriteLine(e.Message);
            throw new Exception(e.Message);
        }
    }


    /// <summary>
    /// Method to get the status of the table stored in TableController from the API.
    /// </summary>
    /// <returns>Status as string. Empty string if no status or some error.</returns>
    /// <param name="guid">GUID of the table to get status from.</param>
    public async Task<string> GetTableStatus(string guid)
    {
        try 
        {
            var response = await _tasks.GetTableInfo(guid);
            if (response == null) throw new Exception("Table not found on API!");
            return response.state.status ?? "";
        } 
        catch (Exception e) 
        {
            Debug.WriteLine(e.Message);
            throw new Exception(e.Message);
        }
    }

    public async Task<ITableError[]> GetTableError(string guid)
    {
        var returnArray = new List<LinakTableError>();
        try 
        {
            var response = await _tasks.GetTableInfo(guid);
            if (response == null) throw new Exception("Table not found on API!");
            if(response.lastErrors!.Length == 0) throw new Exception("No errors found.");
            foreach(var error in response.lastErrors) {
                var parsedError = ParseTableStatus(error.errorCode!.Value);
                returnArray.Add(new LinakTableError(guid, error.time_s!.Value, error.errorCode!.Value, parsedError.Values.First()));
            }
        } 
        catch (Exception e) 
        {
            Debug.WriteLine(e.Message);
            throw new Exception(e.Message);
        }
        return returnArray.Cast<ITableError>().ToArray();
    }

    private Dictionary<TableStatus, string> ParseTableStatus(int errorCode)
    {
        var statusDictionary = new Dictionary<TableStatus, string>();

        switch (errorCode)
        {
            case 1:
                statusDictionary[TableStatus.Lost] = "Position Lost: The desk has an unknown position and needs to be initialized";
                break;
            case 2:
                statusDictionary[TableStatus.Overload] = "General Overload Up: Overload in upward direction has occurred";
                break;
            case 3:
                statusDictionary[TableStatus.Overload] = "General Overload Down: Overload in downward direction has occurred";
                break;
            case 8:
                statusDictionary[TableStatus.OtherError] = "Watchdog: Indicate that software failed to kick watchdog";
                break;
            case 9:
                statusDictionary[TableStatus.Collision] = "LIN collision: Collisions detected on the LIN bus";
                break;
            case 10:
                statusDictionary[TableStatus.OtherError] = "Power fail: Power fail happened or power regulator adjusted below 10%";
                break;
            case 11:
                statusDictionary[TableStatus.OtherError] = "Channel mismatch: Change in number of actuators since initialization";
                break;
            case 12:
                statusDictionary[TableStatus.OtherError] = "Position error: One channel have position different than others";
                break;
            case 13:
                statusDictionary[TableStatus.OtherError] = "Short circuit: Short circuit detected during operation";
                break;
            case 15:
                statusDictionary[TableStatus.OtherError] = "Power limit: System has reached its power limitation";
                break;
            case 16:
                statusDictionary[TableStatus.OtherError] = "Key Error: Illegal keys pressed (handled internally in DP1C)";
                break;
            case 17:
                statusDictionary[TableStatus.OtherError] = "Safety missing: LIN bus unit does not support safety feature";
                break;
            case 18:
                statusDictionary[TableStatus.OtherError] = "Missing Initialization plug: A special service tool is required to change number of channels to the system";
                break;
            case 23:
                statusDictionary[TableStatus.OtherError] = "Ch1 missing: Channel 1 is detected missing";
                break;
            case 24:
                statusDictionary[TableStatus.OtherError] = "Ch2 missing: Channel 2 is detected missing";
                break;
            case 25:
                statusDictionary[TableStatus.OtherError] = "Ch3 missing: Channel 3 is detected missing";
                break;
            case 26:
                statusDictionary[TableStatus.OtherError] = "Ch4 missing: Channel 4 is detected missing";
                break;
            case 29:
                statusDictionary[TableStatus.OtherError] = "Ch1 type: Channel 1 is not same type as when initialized";
                break;
            case 30:
                statusDictionary[TableStatus.OtherError] = "Ch2 type: Channel 2 is not same type as when initialized or not same type as channel 1";
                break;
            case 31:
                statusDictionary[TableStatus.OtherError] = "Ch3 type: Channel 3 is not same type as when initialized or not same type as channel 1";
                break;
            case 32:
                statusDictionary[TableStatus.OtherError] = "Ch4 type: Channel 4 is not same type as when initialized or not same type as channel 1";
                break;
            case 35:
                statusDictionary[TableStatus.OtherError] = "Ch1 pulse fail: Channel 1 had too many pulse errors";
                break;
            case 36:
                statusDictionary[TableStatus.OtherError] = "Ch2 pulse fail: Channel 2 had too many pulse errors";
                break;
            case 37:
                statusDictionary[TableStatus.OtherError] = "Ch3 pulse fail: Channel 3 had too many pulse errors";
                break;
            case 38:
                statusDictionary[TableStatus.OtherError] = "Ch4 pulse fail: Channel 4 had too many pulse errors";
                break;
            case 41:
                statusDictionary[TableStatus.Overload] = "Ch1 overload up: Overload up occurred on channel 1";
                break;
            case 42:
                statusDictionary[TableStatus.Overload] = "Ch2 overload up: Overload up occurred on channel 2";
                break;
            case 43:
                statusDictionary[TableStatus.Overload] = "Ch3 overload up: Overload up occurred on channel 3";
                break;
            case 44:
                statusDictionary[TableStatus.Overload] = "Ch4 overload up: Overload up occurred on channel 4";
                break;
            case 47:
                statusDictionary[TableStatus.Overload] = "Ch1 overload down: Overload down occurred on channel 1";
                break;
            case 48:
                statusDictionary[TableStatus.Overload] = "Ch2 overload down: Overload down occurred on channel 2";
                break;
            case 49:
                statusDictionary[TableStatus.Overload] = "Ch3 overload down: Overload down occurred on channel 3";
                break;
            case 50:
                statusDictionary[TableStatus.Overload] = "Ch4 overload down: Overload down occurred on channel 4";
                break;
            case 53:
                statusDictionary[TableStatus.Collision] = "Ch1 anti-col: Anti-collision triggered on channel 1";
                break;
            case 54:
                statusDictionary[TableStatus.Collision] = "Ch2 anti-col: Anti-collision triggered on channel 2";
                break;
            case 55:
                statusDictionary[TableStatus.Collision] = "Ch3 anti-col: Anti-collision triggered on channel 3";
                break;
            case 56:
                statusDictionary[TableStatus.Collision] = "Ch4 anti-col: Anti-collision triggered on channel 4";
                break;
            case 59:
                statusDictionary[TableStatus.Collision] = "Ch1 SLS/PIEZO: Safety limit switch activated on channel 1";
                break;
            case 60:
                statusDictionary[TableStatus.Collision] = "Ch2 SLS/PIEZO: Safety limit switch activated on channel 2";
                break;
            case 61:
                statusDictionary[TableStatus.Collision] = "Ch3 SLS/PIEZO: Safety limit switch activated on channel 3";
                break;
            case 62:
                statusDictionary[TableStatus.Collision] = "Ch4 SLS/PIEZO: Safety limit switch activated on channel 4";
                break;
            case 65:
                statusDictionary[TableStatus.OtherError] = "Ch1 pulse dir: Pulses counted wrong direction in channel 1";
                break;
            case 66:
                statusDictionary[TableStatus.OtherError] = "Ch2 pulse dir: Pulses counted wrong direction in channel 2";
                break;
            case 67:
                statusDictionary[TableStatus.OtherError] = "Ch3 pulse dir: Pulses counted wrong direction in channel 3";
                break;
            case 68:
                statusDictionary[TableStatus.OtherError] = "Ch4 pulse dir: Pulses counted wrong direction in channel 4";
                break;
            case 71:
                statusDictionary[TableStatus.OtherError] = "Ch1A short: Short circuit on channel 1 [If T-splitter is used short circuit on 1A]";
                break;
            case 72:
                statusDictionary[TableStatus.OtherError] = "Ch1B short: Short circuit on channel 1 [If T-splitter is used short circuit on 1B]";
                break;
            case 73:
                statusDictionary[TableStatus.OtherError] = "Ch2A short: Short circuit on channel 2 [If T-splitter is used short circuit on 2A]";
                break;
            case 74:
                statusDictionary[TableStatus.OtherError] = "Ch2B short: Short circuit on channel 2 [If T-splitter is used short circuit on 2B]";
                break;
            case 75:
                statusDictionary[TableStatus.OtherError] = "Ch3A short: Short circuit on channel 3 [If T-splitter is used short circuit on 3A]";
                break;
            case 76:
                statusDictionary[TableStatus.OtherError] = "Ch3B short: Short circuit on channel 3 [If T-splitter is used short circuit on 3B]";
                break;
            case 77:
                statusDictionary[TableStatus.OtherError] = "Ch4A short: Short circuit on channel 4 [If T-splitter is used short circuit on 4A]";
                break;
            case 78:
                statusDictionary[TableStatus.OtherError] = "Ch4B short: Short circuit on channel 4 [If T-splitter is used short circuit on 4B]";
                break;
            case 86:
                statusDictionary[TableStatus.OtherError] = "Master: Connection to master lost OR following messages are from master";
                break;
            case 87:
                statusDictionary[TableStatus.OtherError] = "Slave 1: Connection to 1st slave lost OR following messages are from 1st slave";
                break;
            case 88:
                statusDictionary[TableStatus.OtherError] = "Slave 2: Connection to 2nd slave lost OR following messages are from 2nd slave";
                break;
            case 89:
                statusDictionary[TableStatus.OtherError] = "Slave 3: Connection to 3rd slave lost OR following messages are from 3rd slave";
                break;
            case 93:
                statusDictionary[TableStatus.Collision] = "DeskSensor 1 - Activation: Detected trigger from LIN bus safety limit switch e.g. DS1";
                break;
            case 94:
                statusDictionary[TableStatus.OtherError] = "DeskSensor 1 - No longer detected: LIN SLS unit (e.g. DS1) no longer detected";
                break;
            case 100:
                statusDictionary[TableStatus.OtherError] = "Table Timeout: Table API timed out after 3000ms. Status unknown.";
                break;
            case 101:
                statusDictionary[TableStatus.Success] = "Table Success: Height changed successfully, however, a height limit was reached.";
                break;
            default:
                statusDictionary[TableStatus.OtherError] = "Unknown Error: An unknown error has occurred";
                break;
        }

        return statusDictionary;
    }
}

internal class LinakSimulatorTasks : ILinakSimulatorTasks {
    private readonly  HttpClient _client;
    private readonly LinakSimulatorControllerOptions _options;
    private readonly string _baseUrl;

    private readonly int maxHeight = 1320;
    private readonly int minHeight = 680;

    internal LinakSimulatorTasks(HttpClient client) {
        _client = client;
        _options = new LinakSimulatorControllerOptions();
        _baseUrl = $"https://{_options.Url}:{_options.Port}/api/{_options.Version}/{_options.Key}";
    }

    public async Task<LinakApiTable?> GetTableInfo(string guid) {
        var url = $"{_baseUrl}/desks/{guid}";
        var response = await _client.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var jsonString = await response.Content.ReadAsStringAsync();
        var apiTable = JsonSerializer.Deserialize<LinakApiTable>(jsonString);

        apiTable!.id = guid;
        return apiTable ?? new LinakApiTable();
    }

    public async Task<HttpResponseMessage> SetTableHeight(int height, string tableId) {
        if(tableId == null) throw new Exception("Table ID is null!");

        var url = $"{_baseUrl}/desks/{tableId}/state";
        var data = new Dictionary<string, object> {
            {"position_mm", height}
        };
        var json = JsonSerializer.Serialize(data);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _client.PutAsync(url, content);
        response.EnsureSuccessStatusCode();
        var responseBody = await response.Content.ReadAsStringAsync();
        var serialisedResponseBody = JsonSerializer.Deserialize<Dictionary<string, object>>(responseBody);
        
        // response.EnsureSuccessStatusCode() will throw an exception if the status code is not 200, however, it is a good idea to have 
        // a check here to ensure that the response body is what we expect it to be.
        if(data.Any(x => x.Value.Equals(serialisedResponseBody![x.Key.ToString()]))) {
            throw new Exception("Response body does not match request body.");
        }
        return new HttpResponseMessage(HttpStatusCode.OK);
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
            var responseBody = await response.Content.ReadAsStringAsync();
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

        var jsonString = await response.Content.ReadAsStringAsync();
        var apiTables = JsonSerializer.Deserialize<string[]>(jsonString);

        return apiTables ?? [];
    }

    public async Task<int> WatchTableAsItMoves(string guid, int newPosition, IProgress<int> progress) {

        // Get last error
        var tempTable = await GetTableInfo(guid);
        var lastError = tempTable!.lastErrors!.LastOrDefault();
        var lastPosition = tempTable.state.position_mm;
        var positionSameCounter = 0;
        var returnValue = -1;

        bool check = true;
        while(check) {
            var table = await GetTableInfo(guid);

            // GET errors
            if(table == null) throw new Exception("Table not found on API!");
            if(table.state.position_mm == null) throw new Exception("Could not get position.");

            // Table Errors
            if(table.lastErrors!.LastOrDefault()!.time_s < lastError!.time_s || 
                (table.lastErrors!.LastOrDefault()!.time_s != null && lastError!.time_s == null)) 
            {
                progress.Report((int)table.lastErrors!.LastOrDefault()!.errorCode!);
            }

            // Other Table Properties relating to collisions, etc...
            else if(table.state.isPositionLost ?? false) {
                progress.Report(1);
            }
            else if(table.state.isOverloadProtectionUp ?? false) {
                progress.Report(2);
            }
            else if(table.state.isOverloadProtectionDown ?? false) {
                progress.Report(3);
            }
            else if(table.state.isAntiCollision ?? false) {
                progress.Report(9);
            }

            // Success if table has reached new position
            if(table.state.position_mm == newPosition)
            {
                returnValue = 0;
                check = false;
            }
            else {
                if(table.state.position_mm != lastPosition) {
                    lastPosition = table.state.position_mm;
                    positionSameCounter = 0;
                }
                else {
                    if(positionSameCounter >= 5 && (table.state.position_mm == minHeight || table.state.position_mm == maxHeight)) {
                        progress.Report(101);
                        returnValue = 101;
                        check = false;
                    }
                    positionSameCounter++;
                }
            }

            if(positionSameCounter > 15) {
                progress.Report(100);
                returnValue = 100;
                check = false;
            }
            await Task.Delay(200); 
        }
        if(returnValue == -1) throw new Exception("Illegal Return Value. Something went catastrophically wrong.");
        else return returnValue;
    }
}

internal interface ILinakSimulatorTasks {
    Task<LinakApiTable?> GetTableInfo(string guid);
    Task<HttpResponseMessage> SetTableInfo(LinakApiTable table);
    Task<HttpResponseMessage> SetTableHeight(int height, string tableId);
    Task<string[]> GetAllTableIds();
    Task<int> WatchTableAsItMoves(string guid, int newPosition, IProgress<int> progress);
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

public class LinakStatusReport : ITableStatusReport
{
    public string guid { get; set; }
    public TableStatus Status { get; set; }
    public string Message { get; set; }

    public LinakStatusReport(string guid, TableStatus status, string message)
    {
        this.guid = guid;
        Status = status;
        Message = message;
    }
}

public class LinakTableError : ITableError
{
    public string guid { get; set; }
    public int TimeSinceError { get; set; }
    public int ErrorCode { get; set; }
    public string Message { get; set; }

    public LinakTableError(string guid, int timeSinceError, int errorCode, string message)
    {
        this.guid = guid;
        TimeSinceError = timeSinceError;
        ErrorCode = errorCode;
        Message = message;
    }
}

[Serializable]
internal class LinakApiTable {
    public string id { get; set; } = null!;
    public LinakApiTableConfig config { get; set; } = null!;
    public LinakApiTableState state { get; set; } = null!;
    public LinakApiTableUsage usage { get; set; } = null!;
    public LinakApiTableError[]? lastErrors { get; set; } = null!;
}

[Serializable]
internal class LinakApiTableConfig {
    public string? name { get; set; }
    public string? manufacturer { get; set; }
}

[Serializable]
internal class LinakApiTableState {
    public int? position_mm { get; set; }
    public int? speed_mms { get; set; }
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