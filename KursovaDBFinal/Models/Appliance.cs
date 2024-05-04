using System;
using System.Collections.Generic;

namespace KursovaDBFinal.Models;

public partial class Appliance
{
    public int ApplianceId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public int CategoryId { get; set; }

    public decimal Price { get; set; }

    public DateTime DateAdded { get; set; }

    public string? PhotoUrl { get; set; }

    public virtual ApplianceCategory Category { get; set; } = null!;

    public virtual InventoryItem? InventoryItem { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual SalesPromotion? SalesPromotion { get; set; }
}
