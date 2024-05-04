using KursovaDBFinal.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace KursovaDBFinal.Seeders;

public class OrderSeeder
{
    private readonly HouseholdAppliancesContext _context;

    public OrderSeeder(HouseholdAppliancesContext context)
    {
        _context = context;
    }

    public async Task SeedAsync()
    {
        // if (!_context.Orders.Any()) // Check if any orders exist (optional)
        {
            var customers = await _context.Customers.ToListAsync(); // Get all customers

            // Generate random orders
            var random = new Random();
            for (var i = 0; i < 1000; i++)
            {
                var customer = customers[random.Next(customers.Count)]; // Pick random customer
                int sellerUserId;

                do
                {
                    sellerUserId = random.Next(1, 501); // Generate sellerUserId in range (1, 500)
                } while (sellerUserId % 3 != 2); // Loop until sellerUserId % 3 == 2

                var order = new Order
                {
                    CustomerId = customer.CustomerId, // Use customer ID from retrieved customer
                    ManagerUserId = sellerUserId,
                    OrderDate = DateTime.UtcNow.AddDays(random.Next(-10, 10)), // Random date within 10 days
                    TotalSum = (decimal)random.NextDouble() * 1000, // Random total sum between 0 and 1000
                    StatusId = random.Next(1, 9) // Status ID in range (1, 9)
                };

                _context.Orders.Add(order);
            }

            await _context.SaveChangesAsync();
        }
    }
}