using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Share.Models.Dtos.SearchDtos
{
    public class OrderSearchDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public DateTime? PaymentDate { get; set; }
        public int PaymentStatus { get; set; }
        public int TableId { get; set; }
    }
}
