using System;
using System.Collections.Generic;

namespace KursovaDBFinal.Models;

public partial class Shipping
{
    public int ShippingId { get; set; }

    public int OrderId { get; set; }

    public string ShippingAddress { get; set; } = null!;

    public string TrackingNumber { get; set; } = null!;

    public int ShippingMethodId { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual ShippingMethod ShippingMethod { get; set; } = null!;
}
