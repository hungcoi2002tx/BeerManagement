using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Share.Models.Dtos.AddDtos
{
    public class OrderDetailAddDto
    {
        public double UnitPrice { get; set; }
        public int Quantity { get; set; }
        public bool IsEnable { get; set; }
    }
}
