using System;
using System.Collections.Generic;

namespace KursovaDBFinal.Models;

public partial class SalesPromotion
{
    public int SaleId { get; set; }

    public int ProductId { get; set; }

    public int DiscountPercentage { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public virtual Appliance Product { get; set; } = null!;
}
