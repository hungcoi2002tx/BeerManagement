using Share.AbtractModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Share.Models.Dtos.SearchDtos
{
    public class TableSearchDto : SearchModelBase
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public int Status { get; set; } = -1;
        public bool IsEnable { get; set; } = true;
    }
}
