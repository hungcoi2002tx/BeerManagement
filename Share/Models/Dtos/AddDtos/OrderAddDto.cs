using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Share.Models.Dtos.AddDtos
{
    public class OrderAddDto
    {
        [Required]
        public DateTime Date { get; set; }
        public double? Total { get; set; }
        public string? Description { get; set; }
        public DateTime? PaymentDate { get; set; }
        public int PaymentStatus { get; set; }
        [Required]
        public int TableId { get; set; }
        public bool IsEnable { get; set; }
    }
}
