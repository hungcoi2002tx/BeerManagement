using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Share.Models.Dtos.EditDtos
{
    public class ImportHistoryEditDto
    {
        public double Total { get => Quantity * UnitPrice; set { } }
        [Required(ErrorMessage ="Trường bắt buộc nhập")]
        public int Quantity { get; set; }
        public string? Description { get; set; }
        public DateTime Date { get; set; }
        [Required(ErrorMessage = "Trường bắt buộc nhập")]
        public int ProductId { get; set; }
        [Required(ErrorMessage = "Trường bắt buộc nhập")]
        public double UnitPrice { get; set; }
        public bool IsEnable { get; set; } = true;
    }
}
