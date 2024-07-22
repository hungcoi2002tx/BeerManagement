using Share.Models.ResponseObject;
using System;
using System.Collections.Generic;

namespace Share.Models.Domain
{
    public partial class ImportHistory
    {
        public int Id { get; set; }
        public double Total { get; set; }
        public int Quantity { get; set; }
        public string? Description { get; set; }
        public DateTime Date { get; set; }
        public int ProductId { get; set; }
        public double UnitPrice { get; set; }
        public bool IsEnable { get; set; }

        public virtual Product Product { get; set; } = null!;
    }
}
