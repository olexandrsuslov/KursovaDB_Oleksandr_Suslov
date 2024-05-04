using System;
using System.Collections.Generic;

namespace KursovaDBFinal.Models;

public partial class ApplianceCategory
{
    public int CategoryId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Appliance> Appliances { get; set; } = new List<Appliance>();
}
