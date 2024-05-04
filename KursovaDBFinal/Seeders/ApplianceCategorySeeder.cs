using KursovaDBFinal.Models;

namespace KursovaDBFinal.Seeders;

public class ApplianceCategorySeeder
{
    private readonly HouseholdAppliancesContext _context; 

    public ApplianceCategorySeeder(HouseholdAppliancesContext context)
    {
        _context = context;
    }

    public async Task SeedAsync()
    {
        if (!_context.ApplianceCategories.Any()) 
        {
            _context.ApplianceCategories.AddRange(
                new ApplianceCategory { CategoryId = 1, Name = "Refrigerators" },
                new ApplianceCategory { CategoryId = 2, Name = "Freezers" },
                new ApplianceCategory { CategoryId = 3, Name = "Dishwashers" },
                new ApplianceCategory { CategoryId = 4, Name = "Ovens" },
                new ApplianceCategory { CategoryId = 5, Name = "Microwave Ovens" },
                new ApplianceCategory { CategoryId = 6, Name = "Cooktops" },
                new ApplianceCategory { CategoryId = 7, Name = "Ranges" },
                new ApplianceCategory { CategoryId = 8, Name = "Range Hoods" },
                new ApplianceCategory { CategoryId = 9, Name = "Food Processors" },
                new ApplianceCategory { CategoryId = 10, Name = "Coffee Makers" },
                new ApplianceCategory { CategoryId = 11, Name = "Toasters" },
                new ApplianceCategory { CategoryId = 12, Name = "Blenders" },
                new ApplianceCategory { CategoryId = 13, Name = "Juicers" },
                new ApplianceCategory { CategoryId = 14, Name = "Washing Machines" },
                new ApplianceCategory { CategoryId = 15, Name = "Dryers" },
                new ApplianceCategory { CategoryId = 16, Name = "Washer-Dryer Combos" },
                new ApplianceCategory { CategoryId = 17, Name = "Irons" },
                new ApplianceCategory { CategoryId = 18, Name = "Steamers" },
                new ApplianceCategory { CategoryId = 19, Name = "Vacuum Cleaners" },
                new ApplianceCategory { CategoryId = 20, Name = "Carpet Cleaners" },
                new ApplianceCategory { CategoryId = 21, Name = "Steam Cleaners" },
                new ApplianceCategory { CategoryId = 22, Name = "Air Conditioners" },
                new ApplianceCategory { CategoryId = 23, Name = "Fans" },
                new ApplianceCategory { CategoryId = 24, Name = "Heaters" },
                new ApplianceCategory { CategoryId = 25, Name = "Humidifiers" },
                new ApplianceCategory { CategoryId = 26, Name = "Dehumidifiers" },
                new ApplianceCategory { CategoryId = 27, Name = "Air Purifiers" },
                new ApplianceCategory { CategoryId = 28, Name = "Electric Kettles" },
                new ApplianceCategory { CategoryId = 29, Name = "Rice Cookers" },
                new ApplianceCategory { CategoryId = 30, Name = "Slow Cookers" },
                new ApplianceCategory { CategoryId = 31, Name = "Pressure Cookers" },
                new ApplianceCategory { CategoryId = 32, Name = "Electric Grills" },
                new ApplianceCategory { CategoryId = 33, Name = "Waffle Makers" },
                new ApplianceCategory { CategoryId = 34, Name = "Electric Skillets" },
                new ApplianceCategory { CategoryId = 35, Name = "Food Dehydrators" },
                new ApplianceCategory { CategoryId = 36, Name = "Ice Cream Makers" },
                new ApplianceCategory { CategoryId = 37, Name = "Bread Makers" },
                new ApplianceCategory { CategoryId = 38, Name = "Hair Dryers" },
                new ApplianceCategory { CategoryId = 39, Name = "Hair Straighteners" },
                new ApplianceCategory { CategoryId = 40, Name = "Electric Shavers" },
                new ApplianceCategory { CategoryId = 41, Name = "Electric Toothbrushes" },
                new ApplianceCategory { CategoryId = 42, Name = "Facial Cleansers" },
                new ApplianceCategory { CategoryId = 43, Name = "Massagers" },
                new ApplianceCategory { CategoryId = 44, Name = "Heating Pads" }
            );
        }


        await _context.SaveChangesAsync();
    }
}