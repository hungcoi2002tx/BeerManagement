using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Share.Models.Domain
{
    public partial class Table
    {
        public Table()
        {
            Orders = new HashSet<Order>();
        }

        public int Id { get; set; }
        public int Number { get; set; }
        public int Status { get; set; }
        public bool IsEnable { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
