using KursovaDBFinal.Models;

namespace KursovaDBFinal.Seeders;

public class ShippingMethodSeeder
{
    private readonly HouseholdAppliancesContext _context; 

    public ShippingMethodSeeder(HouseholdAppliancesContext context)
    {
        _context = context;
    }

    public async Task SeedAsync()
    {
        if (!_context.ShippingMethods.Any()) 
        {
            _context.ShippingMethods.AddRange(
                new ShippingMethod { ShippingMethodId = 1, Type = "Standard Shipping" },
                new ShippingMethod { ShippingMethodId = 2, Type = "Expedited Shipping" },
                new ShippingMethod { ShippingMethodId = 3, Type = "Priority Shipping" },
                new ShippingMethod { ShippingMethodId = 4, Type = "Free Shipping" },
                new ShippingMethod { ShippingMethodId = 5, Type = "Same-Day Delivery" },
                new ShippingMethod { ShippingMethodId = 6, Type = "Next-Day Delivery" },
                new ShippingMethod { ShippingMethodId = 7, Type = "International Shipping" },
                new ShippingMethod { ShippingMethodId = 8, Type = "In-Store Pickup" },
                new ShippingMethod { ShippingMethodId = 9, Type = "Curbside Pickup" }
            );
        }

        await _context.SaveChangesAsync();
    }
}