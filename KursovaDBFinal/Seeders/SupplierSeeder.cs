using KursovaDBFinal.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace KursovaDBFinal.Seeders;

public class SupplierSeeder
{
    private readonly HouseholdAppliancesContext _context;

    public SupplierSeeder(HouseholdAppliancesContext context)
    {
        _context = context;
    }

    public async Task SeedAsync()
    {
        // if (!_context.Suppliers.Any()) 
        {
            // Read data from JSON file
            var seedData = await ReadSupplierDataFromJSONAsync();

            // Add data to context
            _context.Suppliers.AddRange(seedData);

            // Save changes to database
            await _context.SaveChangesAsync();
        }
    }

    private async Task<List<Supplier>> ReadSupplierDataFromJSONAsync()
    {
        string filePath = Path.Combine(Directory.GetCurrentDirectory(), "JSONData", "suppliers.json");
        string jsonData = await File.ReadAllTextAsync(filePath);

        List<Supplier> suppliers;
        using (JsonTextReader reader = new JsonTextReader(new StringReader(jsonData)))
        {
            var serializer = new JsonSerializer();
            suppliers = serializer.Deserialize<List<Supplier>>(reader);
        }

        return suppliers;
    }
}