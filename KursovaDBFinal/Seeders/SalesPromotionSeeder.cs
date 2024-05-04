using KursovaDBFinal.Models;
using Microsoft.EntityFrameworkCore;

namespace KursovaDBFinal.Seeders;

public class SalesPromotionSeeder
{
    private readonly HouseholdAppliancesContext _context;

    public SalesPromotionSeeder(HouseholdAppliancesContext context)
    {
        _context = context;
    }

    public async Task SeedAsync()
    {
        if (!_context.SalesPromotions.Any()) // Check if any promotions exist (optional)
        {
            var appliances = await _context.Appliances.ToListAsync(); // Get all products

            if (appliances.Count != 0) // Check if products exist
            {
                var random = new Random();
                foreach (var appliance in appliances.Take(100)) // Seed for 20 random products
                {
                    // Decide on promotion existence with a probability (e.g., 60%)
                    if (random.NextDouble() < 0.6) // Adjust probability as needed
                    {
                        var startDate = DateTime.UtcNow.AddDays(random.Next(-7, 0));
                        _context.SalesPromotions.Add(new Models.SalesPromotion
                        {
                            ProductId = appliance.ApplianceId,
                            DiscountPercentage = random.Next(5, 31), // Random discount between 5% and 30%
                            StartDate = startDate, // Start date within 7 days in the past
                            EndDate = startDate.AddDays(random.Next(7, 21)) // End date 7-20 days after start
                        });
                    }
                }

                await _context.SaveChangesAsync();
            }
        }
    }

}