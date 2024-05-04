using Microsoft.EntityFrameworkCore;

namespace KursovaDBFinal.Models;

public class TransactionSeeder
{
    private readonly HouseholdAppliancesContext _context;

    public TransactionSeeder(HouseholdAppliancesContext context)
    {
        _context = context;
    }
    
    public async Task SeedAsync()
    {
        // if (!_context.Transactions.Any()) // Check if any transactions exist (optional)
        {
            var orders = await _context.Orders.ToListAsync(); // Get all orders
            var paymentMethods = await _context.PaymentMethods.ToListAsync(); // Get all payment methods (optional)

            if (orders.Count != 0 && paymentMethods.Count != 0) // Check if orders exist
            {
                var random = new Random();
                foreach (var order in orders)
                {
                    _context.Transactions.Add(new Transaction
                    {
                        OrderId = order.OrderId,
                        TotalSum = order.TotalSum, // Assuming transaction amount matches order total
                        TransactionDate = order.OrderDate.AddDays(random.Next(1, 4)), // Transaction 1-3 days after order
                        PaymentMethodId = paymentMethods[random.Next(paymentMethods.Count)].PaymentMethodId  // Use default ID if no payment methods exist
                    });
                }

                await _context.SaveChangesAsync();
            }
        }
    }
}