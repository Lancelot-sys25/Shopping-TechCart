using System;
using System.Collections.Generic;

namespace shoppingTechCart.Entities;

public partial class Product
{
    public string ProductId { get; set; } = null!;

    public string ProductName { get; set; } = null!;

    public string? ProductImage { get; set; }

    public string? Brief { get; set; }

    public DateTime? PostedDate { get; set; }

    public int TypeId { get; set; }

    public string Account { get; set; } = null!;

    public string? Unit { get; set; }

    public int? Price { get; set; }

    public int? Discount { get; set; }

    public virtual Account AccountNavigation { get; set; } = null!;

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    public virtual Category Type { get; set; } = null!;

    public virtual ICollection<ViewHistory> ViewHistories { get; set; } = new List<ViewHistory>();
}
