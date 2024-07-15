using System;
using System.Collections.Generic;

namespace Agri_Energy_Connect.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public string ProductName { get; set; } = null!;

    public double ProductPrice { get; set; }

    public DateOnly ProdDate { get; set; }

    public int CategoryId { get; set; }

    public string UserId { get; set; } = null!;

    public string? ProductImg { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual UserAccount User { get; set; } = null!;
}
