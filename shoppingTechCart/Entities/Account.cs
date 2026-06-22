using System;
using System.Collections.Generic;

namespace shoppingTechCart.Entities;

public partial class Account
{
    public string Account1 { get; set; } = null!;

    public string Pass { get; set; } = null!;

    public string? LastName { get; set; }

    public string FirstName { get; set; } = null!;

    public DateTime? Birthday { get; set; }

    public bool? Gender { get; set; }

    public string? Phone { get; set; }

    public bool? IsUse { get; set; }

    public int? RoleInSystem { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    public virtual SessionToken? SessionToken { get; set; }

    public virtual ICollection<ViewHistory> ViewHistories { get; set; } = new List<ViewHistory>();
}
