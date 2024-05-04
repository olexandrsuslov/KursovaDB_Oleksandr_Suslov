using KursovaDBFinal.Models;
using KursovaDBFinal.Seeders;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetSection("ConnectionStrings")["PostgreSQL_DB"];

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<HouseholdAppliancesContext>(options => options.UseNpgsql(connectionString));
builder.Services.AddDbContext<AuthDbContext>(options => options.UseNpgsql(connectionString));

builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<AuthDbContext>();

builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings.
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;

    // Lockout settings.
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // User settings.
    options.User.AllowedUserNameCharacters =
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = true;
});

builder.Services.ConfigureApplicationCookie(options =>
{
    // Cookie settings
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.SlidingExpiration = true;
});

builder.Services.AddLogging();
builder.Services.AddScoped<ApplianceSeeder>();
builder.Services.AddScoped<UserRoleSeeder>();
builder.Services.AddScoped<UserSeeder>();
builder.Services.AddScoped<CustomerSeeder>();
builder.Services.AddScoped<OrderStatusSeeder>();
builder.Services.AddScoped<PaymentMethodSeeder>();
builder.Services.AddScoped<ShippingMethodSeeder>();
builder.Services.AddScoped<ApplianceCategorySeeder>();
builder.Services.AddScoped<OrderSeeder>();
builder.Services.AddScoped<OrderDetailSeeder>();
builder.Services.AddScoped<SupplierSeeder>();
builder.Services.AddScoped<InventoryItemSeeder>();
builder.Services.AddScoped<SalesPromotionSeeder>();
builder.Services.AddScoped<TransactionSeeder>();
builder.Services.AddScoped<InventoryLocationSeeder>();
builder.Services.AddScoped<ShippingsSeeder>();

var app = builder.Build();

app.Lifetime.ApplicationStarted.Register(async () =>
{
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<HouseholdAppliancesContext>(); 

        // Delete all products 
        context.Appliances.RemoveRange(context.Appliances); 
        await context.SaveChangesAsync();
        
        // Delete all user accounts
        context.Users.RemoveRange(context.Users);
        await context.SaveChangesAsync();
        
        // Delete all user customers
        context.Customers.RemoveRange(context.Customers);
        await context.SaveChangesAsync();
        
        // Delete all user orders
        context.Orders.RemoveRange(context.Orders);
        await context.SaveChangesAsync();
        
        // Delete all order items
        context.OrderDetails.RemoveRange(context.OrderDetails);
        await context.SaveChangesAsync();
        
        // Delete all suppliers
        context.Suppliers.RemoveRange(context.Suppliers);
        await context.SaveChangesAsync();
        
        
        var applianceCategorySeeder = scope.ServiceProvider.GetRequiredService<ApplianceCategorySeeder>();
        await applianceCategorySeeder.SeedAsync();

        var applianceSeeder = scope.ServiceProvider.GetRequiredService<ApplianceSeeder>();
        await applianceSeeder.SeedAsync();
        
        var userRoleSeeder = scope.ServiceProvider.GetRequiredService<UserRoleSeeder>(); 
        await userRoleSeeder.SeedAsync();
        
        var userSeeder = scope.ServiceProvider.GetRequiredService<UserSeeder>(); 
        await userSeeder.SeedAsync();
        
        var customerSeeder = scope.ServiceProvider.GetRequiredService<CustomerSeeder>();
        await customerSeeder.SeedAsync();
        
        var orderStatusSeeder = scope.ServiceProvider.GetRequiredService<OrderStatusSeeder>();
        await orderStatusSeeder.SeedAsync();
        
        var paymentMethodSeeder = scope.ServiceProvider.GetRequiredService<PaymentMethodSeeder>();
        await paymentMethodSeeder.SeedAsync();
        
        var shippingMethodSeeder = scope.ServiceProvider.GetRequiredService<ShippingMethodSeeder>();
        await shippingMethodSeeder.SeedAsync();
        
        var orderSeeder = scope.ServiceProvider.GetRequiredService<OrderSeeder>();
        await orderSeeder.SeedAsync();
        
        var orderDetailSeeder = scope.ServiceProvider.GetRequiredService<OrderDetailSeeder>();
        await orderDetailSeeder.SeedAsync();
        
        var supplierSeeder = scope.ServiceProvider.GetRequiredService<SupplierSeeder>();
        await supplierSeeder.SeedAsync();
        
        var inventoryItemSeeder = scope.ServiceProvider.GetRequiredService<InventoryItemSeeder>();
        await inventoryItemSeeder.SeedAsync();
        
        var salesPromotionSeeder = scope.ServiceProvider.GetRequiredService<SalesPromotionSeeder>();
        await salesPromotionSeeder.SeedAsync();
        
        var transactionSeeder = scope.ServiceProvider.GetRequiredService<TransactionSeeder>();
        await transactionSeeder.SeedAsync();
        
        var inventoryLocationSeeder = scope.ServiceProvider.GetRequiredService<InventoryLocationSeeder>();
        await inventoryLocationSeeder.SeedAsync();
        
        var shippingsSeeder = scope.ServiceProvider.GetRequiredService<ShippingsSeeder>();
        await shippingsSeeder.SeedAsync();
    }
});

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();