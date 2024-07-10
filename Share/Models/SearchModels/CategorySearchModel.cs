using Share.Ultils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Share.Models.SearchModels
{
    public class CategorySearchModel
    {
        public CategorySearchModel()
        {
            
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public Page Page { get; set; } = new Page();
    }
}
