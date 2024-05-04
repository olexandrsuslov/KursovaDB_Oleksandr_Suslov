namespace KursovaDBFinal.Models;

public class DashboardDataViewModel
{
    public List<Appliance> Appliances { get; set; }
    public List<Order> Orders { get; set; }
    public List<OrderDetail> OrderDetails { get; set; }
    
    public List<Transaction> Transactions { get; set; }
 }