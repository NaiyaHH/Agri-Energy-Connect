using System;
using System.Collections.Generic;

namespace Agri_Energy_Connect.Models;

public partial class UserAccount
{
    public string UserId { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public string UserRole { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    public virtual Role UserRoleNavigation { get; set; } = null!;
}
