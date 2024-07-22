using Share.Models.PagingObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Share.AbtractModel
{
    public abstract class SearchModelBase
    {
        public Page? Page { get; set; } = new Page();
    }
}
