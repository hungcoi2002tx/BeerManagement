using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Share.Models.Dtos.EditDtos
{
    public class UserEditDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Trường bắt buộc nhập")]
        public string Fullname { get; set; } = null!;
        [Required(ErrorMessage = "Trường bắt buộc nhập")]
        public string Account { get; set; } = null!;
        [Required(ErrorMessage = "Trường bắt buộc nhập")]
        public string Password { get; set; } = null!;
        [Required(ErrorMessage = "Trường bắt buộc nhập")]
        public int Role { get; set; }
        public bool IsEnable { get; set; } = true;
    }
}
