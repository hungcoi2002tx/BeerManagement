using Share.AbtractModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Share.Models.SearchModels
{
    public class ProductSearchModel : SearchModelBase
    {
        public ProductSearchModel() { }
        public int Id { get; set; }
        public string? Name { get; set; }
        public bool IsEnableOnly { get; set; }
        public bool IsIncludeCategory { get; set; }
        public bool IsIncludeSupplier { get; set; }
    }
}
