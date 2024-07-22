using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Share.Models.Domain
{
    public partial class Supplier
    {
        public Supplier()
        {
            Products = new HashSet<Product>();
        }

        public int Id { get; set; }
        public string PhoneNumber { get; set; } = null!;
        public string? Address { get; set; }
        public string? CompanyName { get; set; }
        public string SupplierName { get; set; } = null!;
        public string? Description { get; set; }
        public bool IsEnable { get; set; }

        [JsonIgnore]
        public virtual ICollection<Product> Products { get; set; }
    }
}
