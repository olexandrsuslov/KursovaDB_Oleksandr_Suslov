using KursovaDBFinal.Models;
using Microsoft.EntityFrameworkCore;

namespace KursovaDBFinal.Seeders;

public class InventoryLocationSeeder
{
    private readonly HouseholdAppliancesContext _context;

    public InventoryLocationSeeder(HouseholdAppliancesContext context)
    {
        _context = context;
    }

    public async Task SeedAsync()
    {
        // if (!_context.InventoryLocations.Any()) // Check if any orders exist (optional)
        {
            var inventoryItems = await _context.InventoryItems.ToListAsync(); // Get all orders

            if (inventoryItems.Count != 0) // Check if both orders and products exist
            {
                var random = new Random();
                foreach (var inventoryItem in inventoryItems)
                {
                        _context.InventoryLocations.Add(new InventoryLocation
                        {
                            InventoryItemId = inventoryItem.InventoryItemId,
                            Row = random.Next(1, 30),
                            Shelf = random.Next(1, 10),
                        });
                }

                await _context.SaveChangesAsync();
            }
        }
    }
}