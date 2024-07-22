using Share.Models.Dtos.AddDtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Share.Models.Dtos.EditDtos
{
    public class TableEditDto : TableAddDto
    {
        public int Id { get; set; }
    }
}
