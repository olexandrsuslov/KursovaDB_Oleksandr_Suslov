using System;
using System.Collections.Generic;

namespace KursovaDBFinal.Models;

public partial class Top3RevenueByCategory
{
    public int? CategoryId { get; set; }

    public decimal? CategoryRevenue { get; set; }
}
