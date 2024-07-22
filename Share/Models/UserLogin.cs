using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Share.Models
{
    public class UserLogin
    {
        [Required(ErrorMessage = "Require Input")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Require Input")]
        public string Password { get; set; }
    }
}
