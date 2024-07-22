using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Share.Models.Dtos.AddDtos
{
    public class OrderDetailAddDto
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public double UnitPrice { get; set; }
        public int Quantity { get; set; } = 0;
        public bool IsEnable { get; set; } = true;
    }
}
