using System;
using System.Collections.Generic;

namespace shoppingTechCart.Entities;

public partial class CartItem
{
    public int Id { get; set; }

    public string SessionId { get; set; } = null!;

    public string ProductId { get; set; } = null!;

    public int? Quantity { get; set; }

    public DateTime? AddedAt { get; set; }

    public virtual Product Product { get; set; } = null!;
}
