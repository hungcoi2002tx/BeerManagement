using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Share.Models.Dtos.AddDtos
{
    public class SupplierAddDto
    {
        [Required(ErrorMessage = "Code không được trống")]
        public string PhoneNumber { get; set; } = null!;
        public string? Address { get; set; }
        public string? CompanyName { get; set; }
        [Required]
        public string SupplierName { get; set; } = null!;
        public string? Description { get; set; }
        public bool IsEnable { get; set; } = true;
    }
}
