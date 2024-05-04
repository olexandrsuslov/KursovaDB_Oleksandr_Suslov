using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace KursovaDBFinal.Models;

public class AuthDbContext : IdentityDbContext
{
    public AuthDbContext (DbContextOptions<AuthDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        var managerRoleId = "2018dd43-73d2-4536-9773-41f57b3e69cb";
        var adminRoleId = "6b4e00f5-4d52-4e62-90f2-e88b259ba544";
        var customerRoleId = "37160b4a-b415-4e09-aeee-c78557b44885";
        
        var roles = new List<IdentityRole>
        {
            new()
            {
                Name = "Manager",
                NormalizedName = "MANAGER",
                Id = managerRoleId,
                ConcurrencyStamp = managerRoleId
            },
            new()
            {
                Name = "Admin",
                NormalizedName = "ADMIN",
                Id = adminRoleId,
                ConcurrencyStamp = adminRoleId
            },
            new()
            {
                Name = "Customer",
                NormalizedName = "CUSTOMER",
                Id = customerRoleId,
                ConcurrencyStamp = customerRoleId
            }
        };

        builder.Entity<IdentityRole>().HasData(roles);

        var adminId = "1e65b678-f3e4-4a61-9e48-b6dc98d43de1";

        var admin = new IdentityUser
        {
            UserName = "admin@email.com",
            Email = "admin@email.com",
            NormalizedEmail = "admin@email.com".ToUpper(),
            NormalizedUserName = "admin@email.com".ToUpper(),
            Id = adminId
        };

        admin.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(admin, "admin@123");
        builder.Entity<IdentityUser>().HasData(admin);

        var adminRoles = new List<IdentityUserRole<string>>
        {
            new()
            {
                RoleId = managerRoleId,
                UserId = adminId
            },
            new()
            {
                RoleId = adminRoleId,
                UserId = adminId
            },
            new()
            {
                RoleId = customerRoleId,
                UserId = adminId
            },
        };

        builder.Entity<IdentityUserRole<string>>().HasData(adminRoles);
    }
}