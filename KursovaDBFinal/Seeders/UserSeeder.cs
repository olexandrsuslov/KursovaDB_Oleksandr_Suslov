using KursovaDBFinal.Models;
using Newtonsoft.Json;

namespace KursovaDBFinal.Seeders;

public class UserSeeder
{
    private readonly HouseholdAppliancesContext _context;

    public UserSeeder(HouseholdAppliancesContext context)
    {
        _context = context;
    }

    public async Task SeedAsync()
    {
        // if (!_context.UserAccounts.Any()) 
        {
            var seedData = await ReadUserDataFromJSONAsync();

            _context.Users.AddRange(seedData);

            await _context.SaveChangesAsync();
        }
    }

    private async Task<List<User>> ReadUserDataFromJSONAsync()
    {
        string filePath = Path.Combine(Directory.GetCurrentDirectory(), "JSONData", "users.json");
        string jsonData = await File.ReadAllTextAsync(filePath);

        List<User> users;
        using (JsonTextReader reader = new JsonTextReader(new StringReader(jsonData)))
        {
            var serializer = new JsonSerializer();
            users = serializer.Deserialize<List<User>>(reader);
        }

        return users;
    }
}