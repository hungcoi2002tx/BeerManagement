using Share.Models.Dtos.AddDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Share.Models.Dtos.EditDtos
{
    public class OrderDetailEditDto : OrderDetailAddDto
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
    }
}
