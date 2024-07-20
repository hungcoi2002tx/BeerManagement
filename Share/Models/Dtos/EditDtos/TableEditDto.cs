using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Share.Models.Dtos.EditDtos
{
    public class TableEditDto
    {
        public int Id { get; set; }
        [Required]
        public int Number { get; set; }
        public int Status { get; set; }
        public bool IsEnable { get; set; }
    }
}
