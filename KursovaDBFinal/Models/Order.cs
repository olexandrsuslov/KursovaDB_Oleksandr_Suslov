using System;
using System.Collections.Generic;

namespace KursovaDBFinal.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public int CustomerId { get; set; }

    public DateTime OrderDate { get; set; }

    public decimal TotalSum { get; set; }

    public int StatusId { get; set; }

    public int? ManagerUserId { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual User? ManagerUser { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual Shipping? Shipping { get; set; }

    public virtual OrderStatus Status { get; set; } = null!;

    public virtual Transaction? Transaction { get; set; }
}
