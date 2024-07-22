using Share.AbtractModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Share.Models.Dtos.SearchDtos
{
    public class UserSearchDto : SearchModelBase
    {
        public int Id { get; set; }
        public string? Account { get; set; }
        public bool? IsEnable { get; set; }
    }
}
