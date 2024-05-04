using KursovaDBFinal.Models;
using Microsoft.EntityFrameworkCore;

namespace KursovaDBFinal.Seeders;

public class OrderDetailSeeder
{
    private readonly HouseholdAppliancesContext _context;

    public OrderDetailSeeder(HouseholdAppliancesContext context)
    {
        _context = context;
    }

    public async Task SeedAsync()
    {
        // if (!_context.OrderItems.Any()) // Check if any orders exist (optional)
        {
            var orders = await _context.Orders.ToListAsync(); // Get all orders
            var products = await _context.Appliances.ToListAsync(); // Get all products

            if (orders.Count != 0 && products.Count != 0) // Check if both orders and products exist
            {
                var random = new Random();
                foreach (var order in orders)
                {
                    var itemCount = random.Next(1, 4); // Generate 1-3 order items per order

                    for (var i = 0; i < itemCount; i++)
                    {
                        var product = products[random.Next(products.Count)]; // Pick random product

                        _context.OrderDetails.Add(new OrderDetail
                        {
                            OrderId = order.OrderId,
                            ApplianceId = product.ApplianceId,
                            Quantity = random.Next(1, 10) // Random quantity between 1 and 9
                        });
                    }
                }

                await _context.SaveChangesAsync();
            }
        }
    }
}