using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Share.Ultils
{
    public class Page
    {
        public double Total { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; } = 15;
        [JsonIgnore]
        public string? BaseUrl { get; set; }

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
