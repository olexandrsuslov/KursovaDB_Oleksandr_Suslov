using KursovaDBFinal.Models;
using Microsoft.EntityFrameworkCore;

namespace KursovaDBFinal.Seeders;

public class InventoryItemSeeder
{
    private readonly HouseholdAppliancesContext _context;

    public InventoryItemSeeder(HouseholdAppliancesContext context)
    {
        _context = context;
    }

    public async Task SeedAsync()
    {
        // if (!_context.InventoryItems.Any()) // Check if any stock items exist (optional)
        {
            var suppliers = await _context.Suppliers.ToListAsync(); // Get all suppliers
            var products = await _context.Appliances.ToListAsync(); // Get all products

            if (suppliers.Count != 0 && products.Count != 0) // Check if both suppliers and products exist
            {
                var random = new Random();
                foreach (var product in products)
                {
                    var supplier = suppliers[random.Next(suppliers.Count)]; // Pick random supplier

                    _context.InventoryItems.Add(new InventoryItem
                    {
                        ApplianceId = product.ApplianceId,
                        SupplierId = supplier.SupplierId,
                        Quantity = random.Next(10, 100), // Random quantity between 10 and 99
                        LastUpdated = DateTime.UtcNow,
                    });
                }

                await _context.SaveChangesAsync();
            }
        }
    }

}