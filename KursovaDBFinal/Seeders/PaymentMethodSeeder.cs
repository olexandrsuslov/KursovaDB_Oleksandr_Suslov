using KursovaDBFinal.Models;

namespace KursovaDBFinal.Seeders;

public class PaymentMethodSeeder
{
    private readonly HouseholdAppliancesContext _context; 

    public PaymentMethodSeeder(HouseholdAppliancesContext context)
    {
        _context = context;
    }

    public async Task SeedAsync()
    {
        if (!_context.PaymentMethods.Any()) 
        {
            _context.PaymentMethods.AddRange(
                new PaymentMethod { PaymentMethodId = 1, Name = "Credit Card" },
                new PaymentMethod { PaymentMethodId = 2, Name = "Debit Card" },
                new PaymentMethod { PaymentMethodId = 3, Name = "PayPal" },
                new PaymentMethod { PaymentMethodId = 4, Name = "Bank Transfer" },
                new PaymentMethod { PaymentMethodId = 5, Name = "Cash on Delivery (COD)" },
                new PaymentMethod { PaymentMethodId = 6, Name = "Electronic Wallets" },
                new PaymentMethod { PaymentMethodId = 7, Name = "Cryptocurrency" },
                new PaymentMethod { PaymentMethodId = 8, Name = "Installment Payment" },
                new PaymentMethod { PaymentMethodId = 9, Name = "Check" },
                new PaymentMethod { PaymentMethodId = 10, Name = "Mobile Payment" }
            );
        }

        await _context.SaveChangesAsync();
    }
}