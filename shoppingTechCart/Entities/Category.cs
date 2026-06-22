using System;
using System.Collections.Generic;

namespace shoppingTechCart.Entities;

public partial class Category
{
    public int TypeId { get; set; }

    public string CategoryName { get; set; } = null!;

    public string? Memo { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
