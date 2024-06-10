using System;
using System.Collections.Generic;

namespace Share.Models.Domain
{
    public partial class Order
    {
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int Id { get; set; }
        public DateTime Date { get; set; }
        public double? Total { get; set; }
        public string? Description { get; set; }
        public DateTime? PaymentDate { get; set; }
        public int PaymentStatus { get; set; }
        public int TableId { get; set; }

        public virtual Table Table { get; set; } = null!;
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
