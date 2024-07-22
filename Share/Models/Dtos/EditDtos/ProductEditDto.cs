using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Share.Models.Dtos.EditDtos
{
    public class ProductEditDto
    {
        public int Id { get; set; }
        public string? Image { get; set; }
        [Required]
        public double UnitPrice { get; set; }
        [Required]
        public int? QuantityPerUnit { get; set; }
        [Required]
        public string Name { get; set; } = null!;
        [Required]
        public int CategoryId { get; set; }
        public int? SupplierId { get; set; }
        public bool ForSell { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsEnable { get; set; } = true;

        public string GetImage
        {
            get => $"/images/product/{Image}";
        }
    }
}
