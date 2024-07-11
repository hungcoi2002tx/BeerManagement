using Share.AbtractModel;
using Share.Ultils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Share.Models.SearchModels
{
    public class SupplierSearchModel : SearchModelBase
    {
        public SupplierSearchModel()
        {
        }

        public int Id { get; set; }
        public string? SupplierName { get; set; }    
        public string? PhoneNumber { get; set; }
    }
}
