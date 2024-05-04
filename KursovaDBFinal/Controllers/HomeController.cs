using System.Diagnostics;
using KursovaDBFinal.Loggers;
using Microsoft.AspNetCore.Mvc;
using KursovaDBFinal.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace KursovaDBFinal.Controllers;

public class HomeController : Controller
{
    private readonly HouseholdAppliancesContext _context;

    public HomeController(HouseholdAppliancesContext context)
    {
        _context = context;
    }
    
    public async Task ExportEntitiesToJSONAsync(string filePath)
    {
        if (string.IsNullOrEmpty(filePath))
        {
            throw new ArgumentNullException(nameof(filePath));
        }

        List<Appliance> appliances;
        List<Customer> customers;
        List<Transaction> transactions;
        using (var context = new HouseholdAppliancesContext()) // Replace with your DbContext name
        {
            appliances = await context.Appliances.ToListAsync();
            customers = await context.Customers.ToListAsync();
            transactions = await context.Transactions.ToListAsync();
        }
        
        var exportData = new
        {
            Appliances = appliances,
            Customers = customers,
            PaymentTransactions = transactions
        };

        var jsonData = JsonConvert.SerializeObject(exportData, Formatting.Indented);
        await System.IO.File.WriteAllTextAsync(filePath, jsonData);
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ExportEntities()
    {
        try
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "JSONData", "export.json"); // Example path

            await ExportEntitiesToJSONAsync(filePath);
            await Logger.Log(User?.Identity?.Name ?? "User", "Exported", "Products, Customers, PaymentTransactions", DateTime.Now);
            TempData["ExportMessage"] = "Entities exported successfully!";
            return RedirectToAction("Profile"); // Redirect to another action after success
        }
        catch (ArgumentNullException ex)
        {
            TempData["ExportError"] = "Error: " + ex.Message;
            return RedirectToAction("Profile"); // Redirect with error message
        }
        catch (Exception ex)
        {
            TempData["ExportError"] = "An error occurred during export: " + ex.Message;
            return RedirectToAction("Profile"); // Redirect with error message
        }
    }

    public IActionResult Index()
    {
        return View();
    }

    [Authorize(Roles = "Admin")]
    public IActionResult InventoryLocations()
    {
        ViewData["InventoryItemsId"] = new SelectList(_context.InventoryItems, "InventoryItemId", "InventoryItemId");
        return View();
    }
    
    [Authorize(Roles = "Admin")]
    public IActionResult GetTopSellingProducts(DateTime startDate, DateTime endDate)
    {
        var topSellingProducts = _context.OrderDetails
            .Include(oi => oi.Appliance)
            .Include(oi => oi.Order)
            .Where(oi => oi.Order.OrderDate >= startDate && oi.Order.OrderDate <= endDate)
            .GroupBy(oi => new { oi.ApplianceId, oi.Appliance.Name })
            .Select(group => new { ProductName = group.Key.Name, Quantity = group.Sum(oi => oi.Quantity) })
            .OrderByDescending(item => item.Quantity)
            .Take(10)
            .ToList();

        var labels = topSellingProducts.Select(item => item.ProductName).ToList();
        var quantities = topSellingProducts.Select(item => item.Quantity).ToList();

        return Json(new { labels, quantities });
    }
    
    [Authorize(Roles = "Admin")]
    public IActionResult GetTotalRevenueByCategory(int categoryId, DateTime startDate, DateTime endDate)
    {
        var totalRevenueByCategory = _context.OrderDetails
            .Include(oi => oi.Appliance)
            .Include(oi => oi.Order)
            .Where(oi => oi.Appliance.CategoryId == categoryId && oi.Order.OrderDate >= startDate && oi.Order.OrderDate <= endDate)
            .GroupBy(oi => oi.Order.OrderDate.Date)
            .Select(group => new { Date = group.Key, TotalRevenue = group.Sum(oi => oi.Quantity * oi.Appliance.Price) })
            .OrderBy(item => item.Date)
            .ToList();

        var dates = totalRevenueByCategory.Select(item => item.Date.ToShortDateString()).ToList();
        var revenue = totalRevenueByCategory.Select(item => item.TotalRevenue).ToList();

        return Json(new { dates, revenue });
    }
    
    [Authorize(Roles = "Admin")]
    public IActionResult GetTransactionDateDistribution(DateTime startDate, DateTime endDate)
    {
        var transactionDateDistribution = _context.Transactions
            .Where(t => t.TransactionDate >= startDate && t.TransactionDate <= endDate)
            .GroupBy(t => t.TransactionDate.Date) // Group by date (ignoring time)
            .Select(g => new { Date = g.Key, TransactionCount = g.Count() })
            .OrderBy(d => d.Date) // Order by date
            .ToList();

        var dates = transactionDateDistribution.Select(item => item.Date.ToShortDateString()).ToList();
        var transactionCounts = transactionDateDistribution.Select(item => item.TransactionCount).ToList();

        return Json(new { dates, transactionCounts });
    }
    
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Dashboard()
    {
        var products = await _context.Appliances.Include(p => p.Category).ToListAsync();
        var orders = await _context.Orders.Include(p => p.Status).ToListAsync();
        var orderItems = await _context.OrderDetails.ToListAsync();
        var transactions = await _context.Transactions.Include(t => t.Order).Include(t => t.PaymentMethod).ToListAsync();
        ViewData["CategoryId"] = new SelectList(_context.ApplianceCategories, "CategoryId", "Name");

        var dashboardData = new DashboardDataViewModel
        {
            Appliances = products,
            Orders = orders,
            OrderDetails = orderItems,
            Transactions = transactions
        };
        await Logger.Log(User?.Identity?.Name ?? "User", "Viewed", "Dashboard", DateTime.Now);
        return View(dashboardData);
    }
    
    private async Task<List<LogEntry>> ReadLogDataFromJSONAsync()
    {
        string filePath = Path.Combine(Directory.GetCurrentDirectory(), "JSONData", "log.json");
        string jsonData = await System.IO.File.ReadAllTextAsync(filePath);

        List<LogEntry> logs;
        using (JsonTextReader reader = new JsonTextReader(new StringReader(jsonData)))
        {
            var serializer = new JsonSerializer();
            logs = serializer.Deserialize<List<LogEntry>>(reader);
        }

        return logs;
    }
    
    [Authorize]
    public async Task<IActionResult> Profile()
    {
        return View();
    }
    
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UserActivity()
    {
        var logData = await ReadLogDataFromJSONAsync();
        return View(logData);
    }
    
    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}