using System;
using System.Collections.Generic;

namespace shoppingTechCart.Entities;

public partial class ViewHistory
{
    public int Id { get; set; }

    public string AccountId { get; set; } = null!;

    public string ProductId { get; set; } = null!;

    public DateTime? ViewedAt { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
