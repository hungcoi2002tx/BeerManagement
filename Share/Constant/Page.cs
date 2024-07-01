using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Share.Constant
{
    public class Page
    {
        public double Total { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; } = 15;
        public double PageCount
        {
            get
            {
                return Math.Ceiling(Total / PageSize);
            }
            set { }
        }
    }
}
