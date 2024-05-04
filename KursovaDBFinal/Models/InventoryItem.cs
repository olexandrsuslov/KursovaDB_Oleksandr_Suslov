using System;
using System.Collections.Generic;

namespace KursovaDBFinal.Models;

public partial class InventoryItem
{
    public int InventoryItemId { get; set; }

    public int ApplianceId { get; set; }

    public int SupplierId { get; set; }

    public int Quantity { get; set; }

    public DateTime LastUpdated { get; set; }

    public int? LocationId { get; set; }

    public virtual Appliance Appliance { get; set; } = null!;

    public virtual InventoryLocation? InventoryLocation { get; set; }

    public virtual Supplier Supplier { get; set; } = null!;
}
