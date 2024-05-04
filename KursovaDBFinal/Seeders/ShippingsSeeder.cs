using KursovaDBFinal.Models;
using Microsoft.EntityFrameworkCore;

namespace KursovaDBFinal.Seeders;

public class ShippingsSeeder
{
    private readonly HouseholdAppliancesContext _context;

    public ShippingsSeeder(HouseholdAppliancesContext context)
    {
        _context = context;
    }
    
    public async Task SeedAsync()
    {
        if (!_context.Shippings.Any()) // Check if any shipping info exists (optional)
        {
            var orders = await _context.Orders.ToListAsync(); // Get all orders
            var shippingMethods = await _context.ShippingMethods.ToListAsync(); // Get all shipping methods (optional)

            if (orders.Count != 0) // Check if orders exist
            {
                var random = new Random();
                foreach (var order in orders)
                {
                    var address = $"{(random.Next(2) == 0 ? "123 Main St" : "456 Elm St")}, Anytown, CA 12345"; // Generate sample addresses

                    _context.Shippings.Add(new Shipping
                    {
                        OrderId = order.OrderId,
                        ShippingAddress = address,
                        TrackingNumber = $"SHIP-{order.OrderId}-{random.Next(100000, 999999)}", // Generate sample tracking numbers
                        ShippingMethodId = shippingMethods[random.Next(shippingMethods.Count)].ShippingMethodId  // Use default ID if no shipping methods exist
                    });
                }

                await _context.SaveChangesAsync();
            }
        }
    }
}