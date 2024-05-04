using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KursovaDBFinal.Models;

public partial class Customer
{
    public int CustomerId { get; set; }

    [StringLength(50, ErrorMessage = "First name must be between {2} and {1} characters.", MinimumLength = 2)]
    [RegularExpression(@"^[^\d]+$", ErrorMessage = "First name cannot contain digits.")]
    public string FirstName { get; set; } = null!;

    [StringLength(50, ErrorMessage = "Last name must be between {2} and {1} characters.", MinimumLength = 2)]
    [RegularExpression(@"^[^\d]+$", ErrorMessage = "Last name cannot contain digits.")]
    public string LastName { get; set; } = null!;
    
    [RegularExpression(@"^\+380\d{2}\d{3}\d{2}\d{2}$", ErrorMessage = "Phone number must be in format +380957428835.")]
    public string Phone { get; set; } = null!;

    public int? UserId { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual User? User { get; set; }
}
