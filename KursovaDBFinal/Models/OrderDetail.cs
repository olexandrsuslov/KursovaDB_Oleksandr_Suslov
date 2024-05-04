using System;
using System.Collections.Generic;

namespace KursovaDBFinal.Models;

public partial class OrderDetail
{
    public int OrderDetailId { get; set; }

    public int OrderId { get; set; }

    public int ApplianceId { get; set; }

    public int Quantity { get; set; }

    public virtual Appliance Appliance { get; set; } = null!;

    public virtual Order Order { get; set; } = null!;
}
