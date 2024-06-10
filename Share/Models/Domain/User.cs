using System;
using System.Collections.Generic;

namespace Share.Models.Domain
{
    public partial class User
    {
        public int Id { get; set; }
        public string Fullname { get; set; } = null!;
        public string Account { get; set; } = null!;
        public byte[] Password { get; set; } = null!;
        public int Role { get; set; }
    }
}
