using KursovaDBFinal.Models;

namespace KursovaDBFinal.Seeders;

public class UserRoleSeeder
{
    private readonly HouseholdAppliancesContext _context; // Replace with your DbContext name

    public UserRoleSeeder(HouseholdAppliancesContext context)
    {
        _context = context;
    }

    public async Task SeedAsync()
    {
        if (!_context.UserRoles.Any()) // Check if any user roles exist (optional)
        {
            // Seed user roles data here
            _context.UserRoles.AddRange(
                new UserRole { RoleId = 1, Role = "Admin" },
                new UserRole { RoleId = 2, Role = "Manager" },
                new UserRole { RoleId = 3, Role = "Customer" } 
            );
        }

        await _context.SaveChangesAsync();
    }
}