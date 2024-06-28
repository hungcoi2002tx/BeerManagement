using System;
using System.Collections.Generic;

namespace Share.Models.Domain
{
    public partial class Category
    {
        public Category()
        {
            Products = new HashSet<Product>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string? Image { get; set; }
        public bool IsEnable { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
