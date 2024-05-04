using KursovaDBFinal.Models;
using Newtonsoft.Json;

namespace KursovaDBFinal.Seeders;

public class ApplianceSeeder
{
    private readonly HouseholdAppliancesContext _context;

    public ApplianceSeeder(HouseholdAppliancesContext context)
    {
        _context = context;
    }

    public async Task SeedAsync()
    {
        // if (!_context.Products.Any()) 
        {
            // Read data from JSON file
            var seedData = await ReadApplianceDataFromJSONAsync();

            // Add data to context
            _context.Appliances.AddRange(seedData);

            // Save changes to database
            await _context.SaveChangesAsync();
        }
    }

    private async Task<List<Appliance>> ReadApplianceDataFromJSONAsync()
    {
        // Replace "products.json" with your actual file name and path
        string filePath = Path.Combine(Directory.GetCurrentDirectory(), "JSONData", "appliances.json");
        string jsonData = await File.ReadAllTextAsync(filePath);

        List<Appliance> appliances;
        using (JsonTextReader reader = new JsonTextReader(new StringReader(jsonData)))
        {
            var serializer = new JsonSerializer();
            appliances = serializer.Deserialize<List<Appliance>>(reader);
        }

        return appliances;
    }
}
