using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Share.Models.Dtos.ViewDtos
{
    public class ImportHistoryViewDto
    {
        public int Stt { get; set; }
        public int Id { get; set; }
        public double Total { get; set; }
        public int Quantity { get; set; }
        public string? Description { get; set; }
        public string DateString { get => Date.ToString("dd/MM/yyyy"); set { } }
        public int ProductId { get => Product.Id; set { } }
        public string ProductName { get => Product.Name; set { } }
        public double UnitPrice { get; set; }
        public bool IsEnable { get; set; }

        public ProductViewDto Product { get; set; }
        public DateTime Date { get; set; }
    }
}
