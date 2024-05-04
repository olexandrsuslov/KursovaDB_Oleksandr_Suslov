using System;
using System.Collections.Generic;

namespace KursovaDBFinal.Models;

public partial class Transaction
{
    public int TransactionId { get; set; }

    public int OrderId { get; set; }

    public decimal TotalSum { get; set; }

    public DateTime TransactionDate { get; set; }

    public int PaymentMethodId { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual PaymentMethod PaymentMethod { get; set; } = null!;
}
