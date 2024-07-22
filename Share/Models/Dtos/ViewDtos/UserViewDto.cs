using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Share.Models.Dtos.ViewDtos
{
    public class UserViewDto
    {
        public int Id { get; set; }
        public int Stt { get; set; }
        public string Fullname { get; set; }
        public string Account { get; set; }
        public int Role { get; set; }
        public string RoleStr { get => GetRoleStr(); set { } }
        public bool IsEnable { get; set; }

        private string GetRoleStr()
        {
            switch (Role)
            {
                case 1:
                    return "Admin";
                    break;
                case 2:
                    return "Staff";
                    break;
                case 3:
                    return "Manager";
                    break;
                default:
                    return null;
            }
        }
    }
}
