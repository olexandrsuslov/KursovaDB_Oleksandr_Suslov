using KursovaDBFinal.Models;
using Newtonsoft.Json;

namespace KursovaDBFinal.Seeders;

public class CustomerSeeder
{
    private readonly HouseholdAppliancesContext _context;

    public CustomerSeeder(HouseholdAppliancesContext context)
    {
        _context = context;
    }

    public async Task SeedAsync()
    {
        // if (!_context.Customers.Any()) 
        {
            var seedData = await ReadCustomerDataFromJSONAsync();

            _context.Customers.AddRange(seedData);

            await _context.SaveChangesAsync();
        }
    }

    private async Task<List<Customer>> ReadCustomerDataFromJSONAsync()
    {
        string filePath = Path.Combine(Directory.GetCurrentDirectory(), "JSONData", "customers.json");
        string jsonData = await File.ReadAllTextAsync(filePath);

        List<Customer> customers;
        using (JsonTextReader reader = new JsonTextReader(new StringReader(jsonData)))
        {
            var serializer = new JsonSerializer();
            customers = serializer.Deserialize<List<Customer>>(reader);
        }

        return customers;
    }
}