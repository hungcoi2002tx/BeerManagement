using Share.Models.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Share.Models.Dtos.ViewDtos
{
    public class TableViewDto
    {
        public int Id { get; set; }
        [Display(Name = "No")]
        public int Stt { get; set; }
        [Display(Name = "Number")]
        public int Number { get; set; }
        [Display(Name = "Status")]
        public int Status { get; set; }
        [Display(Name = "IsEnable")]
        public bool IsEnable { get; set; }

        public int? OrderId { get; set; } = null;
    }
}
