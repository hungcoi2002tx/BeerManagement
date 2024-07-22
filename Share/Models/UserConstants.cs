using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Share.Models
{
    public class UserConstants
    {
        public static List<UserModel> Users = new List<UserModel>()
        {
            new UserModel()
            {
                UserName = "admin", Email="admin@gmail.com",Password="admin", Role="admin"
            },
            new UserModel()
            {
                UserName = "client", Email="client@gmail.com",Password="client", Role="client"
            },
        };
    }
}
