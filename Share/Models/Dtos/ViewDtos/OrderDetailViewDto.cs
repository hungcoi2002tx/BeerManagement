using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Share.Models.Dtos.ViewDtos
{
    public class OrderDetailViewDto
    {
        public int? OrderId { get; set; } = null;
        public int ProductId { get; set; }
        public double UnitPrice { get; set; }
        public int Quantity { get; set; } = 0;
        public int Stt { get; set; }
        public string? Image { get; set; }
        public int? QuantityPerUnit { get; set; }
        public string Name { get; set; } = null!;

        public string GetImageUrl
        {
            get => $"./images/product/85d72796-91f6-4efb-bee3-c4a2fbd4b7fe.jpg";
            set { }
        }
    }
}
