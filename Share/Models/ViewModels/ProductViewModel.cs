using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Share.Models.ViewModels
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        [Display(Name = "No")]
        public int Stt {  get; set; }
        [Display(Name = "Image")]
        public string? Image { get; set; }
        [Display(Name = "Unit Price")]
        public double UnitPrice { get; set; }
        [Display(Name = "Quantity Per Unit")]
        public int? QuantityPerUnit { get; set; }
        [Display(Name = "Name")]
        public string Name { get; set; } = null!;
        public int CategoryId { get; set; }
        public int? SupplierId { get; set; }
        public bool ForSell { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsEnable { get; set; }
        public string GetImage
        {
            get => $"/images/product/{Image}";
        }
        public CategoryViewModel Category { get; set; }
        public SupplierViewModel? Supplier { get; set; }
    }
}
