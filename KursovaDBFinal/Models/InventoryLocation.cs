using System;
using System.Collections.Generic;

namespace KursovaDBFinal.Models;

public partial class InventoryLocation
{
    public int InventoryLocationId { get; set; }

    public int InventoryItemId { get; set; }

    public int Row { get; set; }

    public int Shelf { get; set; }

    public virtual InventoryItem? InventoryItem { get; set; }
}
