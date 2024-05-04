using KursovaDBFinal.Models;

namespace KursovaDBFinal.Seeders;

public class OrderStatusSeeder
{
    private readonly HouseholdAppliancesContext _context; 

    public OrderStatusSeeder(HouseholdAppliancesContext context)
    {
        _context = context;
    }

    public async Task SeedAsync()
    {
        if (!_context.OrderStatuses.Any()) 
        {
            _context.OrderStatuses.AddRange(
                new OrderStatus { StatusId = 1, Name = "Pending" },
                new OrderStatus { StatusId = 2, Name = "Processing" },
                new OrderStatus { StatusId = 3, Name = "Shipped" },
                new OrderStatus { StatusId = 4, Name = "Delivered" },
                new OrderStatus { StatusId = 5, Name = "Cancelled" },
                new OrderStatus { StatusId = 6, Name = "On Hold" },
                new OrderStatus { StatusId = 7, Name = "Refunded" },
                new OrderStatus { StatusId = 8, Name = "Backordered" },
                new OrderStatus { StatusId = 9, Name = "Returned" }
            );
        }

        await _context.SaveChangesAsync();
    }
}