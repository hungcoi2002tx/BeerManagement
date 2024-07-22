using Share.AbtractModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Share.Models.Dtos.SearchDtos
{
    public class ProductSearchDto : SearchModelBase
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public bool? IsEnable { get; set; }
        public bool? IsForSell { get; set; }
        public bool IsIncludeCategory { get; set; }
        public bool IsIncludeSupplier { get; set; }
        public List<int>? Ids { get; set; }
    }
}
