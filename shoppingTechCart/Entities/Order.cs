using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shoppingTechCart.Entities
{
    public class Order
    {
        public int OrderId { get; set; }
        public string AccountId { get; set; } = null!;
        public DateTime? CreateAt { get; set; }
        public int TotalAmount { get; set; }

        public string Status { get; set; } = null!;

        public string? PaymentMethod { get; set; }

        public DateTime? PaidAt { get; set; }

        public virtual Account Account { get; set; } = null!;

        public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}
