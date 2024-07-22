using Share.AbtractModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Share.Models.Dtos.SearchDtos
{
    public class OrderSearchDto : SearchModelBase
    {
        public int Id { get; set; }
        public DateTime? Date { get; set; } = null;
        public DateTime? PaymentDate { get; set; } = null;
        public int PaymentStatus { get; set; } = -1;
        public int TableId { get; set; } = -1;
    }
}
