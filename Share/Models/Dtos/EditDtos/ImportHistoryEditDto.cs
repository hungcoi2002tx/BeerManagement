using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Share.Models.Dtos.EditDtos
{
    public class ImportHistoryEditDto
    {
        public double Total { get => Quantity * UnitPrice; set { } }
        public int Quantity { get; set; }
        public string? Description { get; set; }
        public DateTime Date { get; set; }
        public int ProductId { get; set; }
        public double UnitPrice { get; set; }
        public bool IsEnable { get; set; } = true;
    }
}
