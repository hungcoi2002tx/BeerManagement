using Share.AbtractModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Share.Models.Dtos.SearchDtos
{
    public class OrderDetailSearchDto : SearchModelBase
    {
        public int OrderId { get; set; } = -1;
        public int ProductId { get; set; } = -1;
        public bool GetProduct { get; set; } = false;
    }
}
