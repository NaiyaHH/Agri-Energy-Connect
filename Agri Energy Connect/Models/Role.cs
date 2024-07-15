using System;
using System.Collections.Generic;

namespace Agri_Energy_Connect.Models;

public partial class Role
{
    public string UserRole { get; set; } = null!;

    public virtual ICollection<UserAccount> UserAccounts { get; set; } = new List<UserAccount>();
}
