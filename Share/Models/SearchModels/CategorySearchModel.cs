using Share.AbtractModel;
using Share.Ultils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Share.Models.SearchModels
{
    public class CategorySearchModel : SearchModelBase
    {
        public CategorySearchModel()
        {
            
        }
        public int Id { get; set; }
        public string? Name { get; set; }
    }
}
