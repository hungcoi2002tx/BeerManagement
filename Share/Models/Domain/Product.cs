﻿using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Share.Models.Domain
{
    public partial class Product
    {
        public Product()
        {
            ImportHistories = new HashSet<ImportHistory>();
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int Id { get; set; }
        public string? Image { get; set; }
        public double UnitPrice { get; set; }
        public int? QuantityPerUnit { get; set; }
        public string Name { get; set; } = null!;
        public int CategoryId { get; set; }
        public int? SupplierId { get; set; }
        public bool ForSell { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsEnable { get; set; }

        public virtual Category Category { get; set; } = null!;
        public virtual Supplier? Supplier { get; set; }
        [JsonIgnore]
        public virtual ICollection<ImportHistory> ImportHistories { get; set; }
        [JsonIgnore]
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
