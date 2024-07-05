using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Share.Models.EditModels
{
    public class DemoEditModel
    {
        [Required]
        public int Id { get; set; }
    }
}
