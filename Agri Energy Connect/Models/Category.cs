using System;
using System.Collections.Generic;

namespace Agri_Energy_Connect.Models;

public partial class Category
{
    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = null!;

    public string? CategoryImg { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
