using System;
using System.Collections.Generic;

namespace KursovaDBFinal.Models;

public partial class ShippingMethod
{
    public int ShippingMethodId { get; set; }

    public string Type { get; set; } = null!;

    public virtual ICollection<Shipping> Shippings { get; set; } = new List<Shipping>();
}
