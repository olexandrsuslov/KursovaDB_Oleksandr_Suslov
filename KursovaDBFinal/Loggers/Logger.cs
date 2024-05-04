using Newtonsoft.Json;

namespace KursovaDBFinal.Loggers;

public static class Logger
{
    private static readonly string _filePath = Path.Combine(Directory.GetCurrentDirectory(), "JSONData", "log.json");
    private static readonly List<LogEntry> _logEntries = new();

    public static async Task Log(string user, string action, string table, DateTime timestamp)
    {
        try
        {
            var logEntry = new LogEntry
            {
                User = user, 
                Action = action, 
                Table = table, 
                Timestamp = timestamp
            };
            
            _logEntries.Add(logEntry);
            var jsonData = JsonConvert.SerializeObject(_logEntries, Formatting.Indented);
            await File.WriteAllTextAsync(_filePath, jsonData);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error writing to log file: {ex.Message}");
        }
    }
}


public class LogEntry
{
    public string User { get; set; }
    public string Action { get; set; }
    public string Table { get; set; }
    public DateTime Timestamp { get; set; }
}