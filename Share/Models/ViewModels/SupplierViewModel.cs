using Share.AbtractModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Share.Models.ViewModels
{
    public class SupplierViewModel
    {
        public int Id { get; set; }
        [Display(Name = "No")]
        public int Stt { get; set; }
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; } = null!;
        [Display(Name = "Address")]
        public string? Address { get; set; }
        [Display(Name = "Company Name")]
        public string? CompanyName { get; set; }
        [Display(Name = "Supplier Name")]
        public string SupplierName { get; set; } = null!;
        [Display(Name = "Description")]
        public string? Description { get; set; }
    }
}
