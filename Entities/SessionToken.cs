using System;
using System.Collections.Generic;

namespace shoppingTechCart.Entities;

public partial class SessionToken
{
    public string AccountId { get; set; } = null!;

    public string Token { get; set; } = null!;

    public DateTime? UpdatedAt { get; set; }

    public virtual Account Account { get; set; } = null!;
}
